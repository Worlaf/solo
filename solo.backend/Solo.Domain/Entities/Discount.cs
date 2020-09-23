using System;

namespace Solo.Domain.Entities
{
    public class Discount : EntityBase
    {
        public int ParkObjectId { get; set; }
        public ParkObject ParkObject { get; set; }

        public string DescriptionMarkdown { get; set; }

        public decimal Amount { get; set; }
        public bool IsPercent { get; set; }
        public DateTimeOffset StartsAt { get;set; }
        public DateTimeOffset EndsAt { get;set; }
        public DateTimeOffset? DayTimeStart { get; set; }
        public DateTimeOffset? DayTimeEnd { get; set; }
    }
}
