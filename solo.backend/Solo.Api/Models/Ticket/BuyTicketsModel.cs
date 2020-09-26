using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solo.Api.Models.Ticket
{
    public class BuyTicketsModel
    {
        public int ParkObjectId { get;set; }
        public int AdultCount { get;set; }
        public int ChildCount { get; set; }
        public int PrivilegedCount { get;set; }

        public decimal Price { get; set; }
    }
}
