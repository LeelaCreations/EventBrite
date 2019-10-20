using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Models
{
    public class CartItem
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string EventName { get; set; }
        public decimal TicketPrice { get; set; }
        public decimal OldTicketPrice { get; set; }
        public int NumberOfTickets { get; set; }
        public string PictureUrl { get; set; }
    }
}
