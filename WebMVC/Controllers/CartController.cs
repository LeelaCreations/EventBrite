using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using WebMVC.Models;
using WebMVC.Models.CartModels;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {//cart controller
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;
        private readonly IIdentityService<ApplicationUser> _identityService;

        public CartController(IIdentityService<ApplicationUser> identityService, ICartService cartService, ICatalogService catalogService)
        {
            _identityService = identityService;
            _cartService = cartService;
            _catalogService = catalogService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Dictionary<string, int> quantities, string action)
        {
            if (action == "[ Checkout ]")
            {
                return RedirectToAction("Create", "Order");
            }
            try
            {
                var user = _identityService.Get(HttpContext.User);
                var basket = await _cartService.SetQuantities(user, quantities);
                var vm = await _cartService.UpdateCart(basket);
            }
            catch (BrokenCircuitException)
            {
                //catch error when cartapi is in open circut mode
                HandleBrokenCircuitException();
            }
            return View();
        }
        public async Task<IActionResult> AddToCart(CatalogEvent eventDetails)
        {
            try
            {
                if (eventDetails.Id != 0)
                {
                    var user = _identityService.Get(HttpContext.User);
                    var evnt = new CartItem()
                    {
                        Id = Guid.NewGuid().ToString(),
                        NumberOfTickets = 1,
                        EventName = eventDetails.Name,
                        PictureUrl = eventDetails.PictureUrl,
                        TicketPrice = eventDetails.Price,
                        EventId = eventDetails.Id
                    };
                    await _cartService.AddItemToCart(user, evnt);
                }
            }
            catch (BrokenCircuitException)
            {
                HandleBrokenCircuitException();
            }
            return RedirectToAction("Index", "Catalog");
        }
        private void HandleBrokenCircuitException()
        {
            TempData["BasketInoperativeMsg"] = "cart Service is inoperative, please try later on.(Business Msg Due toCircuit-Breaker)";
        }
    }
}