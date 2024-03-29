﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Models.CartModels;
using WebMVC.Models.OrderModels;

namespace WebMVC.Services
{
    public class CartService:ICartService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;
        private readonly ILogger _logger;
        public CartService(IConfiguration config, IHttpContextAccessor httpContextAccessor, IHttpClient httpClient, ILoggerFactory logger)
        {
            _config = config;
            _remoteServiceBaseUrl = $"{_config["CartUrl"]}/api/v1/cart";
            _apiClient = httpClient;
            _logger = logger.CreateLogger<CartService>();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddItemToCart(ApplicationUser user, CartItem evnt)
        {
            var cart = await GetCart(user);
            _logger.LogDebug("User Name:" + user.Id);
            if (cart == null)
            {
                cart = new Cart()
                {
                    BuyerId = user.Id,
                    Events = new List<CartItem>()
                };
            }
            var basketItem = cart.Events
                .Where(p => p.EventId == evnt.EventId)
                .FirstOrDefault();
            if (basketItem == null)
            {
                cart.Events.Add(evnt);
            }
            else
            {
                basketItem.NumberOfTickets += 1;
            }
            await UpdateCart(cart);
        }

        public async Task<Cart> UpdateCart(Cart cart)
        {
            var token = await GetUserTokenAsync();
            _logger.LogDebug("Service url: " + _remoteServiceBaseUrl);
            var updateBaskeUri = ApiPaths.Basket.UpdateBasket(_remoteServiceBaseUrl);
            _logger.LogDebug("Update Basket url: " + updateBaskeUri);
            var response = await _apiClient.PostAsync(updateBaskeUri, cart, token);
            response.EnsureSuccessStatusCode();
            return cart;
        }
        public async Task<Cart> GetCart(ApplicationUser user)
        {
            var token = await GetUserTokenAsync();
            _logger.LogInformation("We are in get and user id" + user.Id);
            _logger.LogInformation(_remoteServiceBaseUrl);

            var getBasketUri = ApiPaths.Basket.GetBasket(_remoteServiceBaseUrl, user.Id);
            _logger.LogInformation(getBasketUri);
            var dataString = await _apiClient.GetStringAsync(getBasketUri, token);
            _logger.LogInformation(dataString);

            var response = JsonConvert.DeserializeObject<Cart>(dataString.ToString()) ??
                new Cart()
                {
                    BuyerId = user.Id
                };
            return response;
        }
        async Task<string> GetUserTokenAsync()
        {
            var context = _httpContextAccessor.HttpContext;
            return await context.GetTokenAsync("access_token");
        }
        public async Task<Cart> SetQuantities(ApplicationUser user, Dictionary<string, int> quantities)
        {
            var basket = await GetCart(user);
            basket.Events.ForEach(x =>
            {
                if (quantities.TryGetValue(x.Id, out var quantity))
                {
                    x.NumberOfTickets = quantity;
                }
            });
            return basket;
        }
        public async Task ClearCart(ApplicationUser user)
        {
            var token = await GetUserTokenAsync();
            var cleanBasketUri = ApiPaths.Basket.CleanBasket(_remoteServiceBaseUrl, user.Id);
            _logger.LogDebug("Clean Basket uri: " + cleanBasketUri);
            var response = await _apiClient.DeleteAsync(cleanBasketUri);
            _logger.LogDebug("Basket cleaned");
        }
        public Order MapCartToOrder(Cart cart)
        {
            var order = new Order();
            order.OrderTotal = 0;
            cart.Events.ForEach(x =>
            {
                order.OrderItems.Add(new OrderItem()
                {
                    EventId = x.EventId,
                    PictureUrl = x.PictureUrl,
                    EventName = x.EventName,
                    NumberOfTickets = x.NumberOfTickets,
                    TicketPrice = x.TicketPrice
                });
                order.OrderTotal += (x.NumberOfTickets * x.TicketPrice);
            });
            return order;
        }
    }
}
