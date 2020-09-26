using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Solo.Domain.Entities;

namespace Solo.Api.Models.Ticket
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool Closed { get; set; }
        public TicketType Type { get; set; }
        public int ParkObjectId { get; set; }
        public int QueueNumber { get; set; }
    }
}