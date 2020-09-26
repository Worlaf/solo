using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solo.Api.Models.ParkObject
{
    public class ParkObjectViewModel : ParkObjectSaveModel
    {
        public int Id { get; set; }
        public int TicketsTotal { get; set; }
        public int TicketsClosed { get; set; }
    }
}