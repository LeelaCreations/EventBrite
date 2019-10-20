using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models.CartModels
{
    public class CartItem
    {
        public string Id { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public decimal TicketPrice { get; set; }
        public decimal OldTicketPrice { get; set; }
        public int NumberOfTickets { get; set; }
        public string PictureUrl { get; set; }
    }
}
