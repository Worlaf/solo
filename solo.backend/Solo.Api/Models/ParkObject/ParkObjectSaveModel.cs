using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Solo.Domain.Entities;
using Solo.Domain.Map;

namespace Solo.Api.Models.ParkObject
{
    public class ParkObjectSaveModel
    {
        public int ParkId { get; set; }
        public string Name { get; set; }
        public string PublicDescriptionMarkdown { get; set; }
        public string AdministrationDescriptionMarkdown { get; set; }
        public Point Location { get; set; }
        public decimal PriceForAdults { get; set; }
        public decimal PriceForChildren { get; set; }
        public string WorkScheduleJson { get; set; }
        public string ImageUrl { get; set; }
        public ObjectType Type { get; set; }
    }
}