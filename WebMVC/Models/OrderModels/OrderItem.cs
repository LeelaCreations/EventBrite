﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models.OrderModels
{
    public class OrderItem
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public decimal TicketPrice { get; set; }
        public int NumberOfTickets { get; set; }
        public string PictureUrl { get; set; }
    }
}
