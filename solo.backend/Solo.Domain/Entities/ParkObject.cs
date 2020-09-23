using System.Collections.Generic;
using Solo.Domain.Map;

namespace Solo.Domain.Entities
{
    public class ParkObject : EntityBase
    {
        public int ParkId { get; set; }
        public Park Park { get; set; }

        public string Name { get; set; }
        public string PublicDescriptionMarkdown { get; set; }
        public string AdministrationDescriptionMarkdown { get; set; }
        public Point Location { get; set; }
        public decimal PriceForAdults { get; set; }
        public decimal PriceForChildren { get; set; }
        public string ImageUrl { get; set; }
        public ObjectType Type { get; set; }

        /// <summary>
        /// <see cref="WorkSchedule"/>
        /// </summary>
        public string WorkScheduleJson { get; set; }

        public ICollection<Discount> Discounts { get; set; }

    }

    public enum ObjectType
    {
        None,
        Sight,
        Attraction,
        Shop,
        Food
    }
}
