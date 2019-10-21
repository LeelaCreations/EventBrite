using OrderApi.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EventName { get; set; }
        public string PictureUrl { get; set; }
        public decimal TicketPrice { get; set; }
        public int NumberOfTickets { get; set; }
        public int EventId { get; private set; }

        protected OrderItem() { }
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public OrderItem(int eventId, string eventName, decimal ticketPrice, string pictureUrl, int numberOfTickets = 1)
        {
            if (numberOfTickets <= 0)
            {
                throw new OrderingDomainException("Invalid number of units");
            }
            EventId = eventId;
            EventName = eventName;
            TicketPrice = ticketPrice;
            NumberOfTickets = numberOfTickets;
            PictureUrl = pictureUrl;
        }
        public void SetPictureUri(string pictureUri)
        {
            if (!String.IsNullOrWhiteSpace(pictureUri))
            {
                PictureUrl = pictureUri;
            }
        }

        public void AddTickets(int numberOfTickets)
        {
            if (numberOfTickets < 0)
            {
                throw new OrderingDomainException("Invalid units");
            }
            NumberOfTickets += numberOfTickets;
        }
    }
}
