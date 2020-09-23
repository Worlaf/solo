using System;
using System.Collections.Generic;
using System.Text;

namespace Solo.Domain
{
    public class WorkSchedule
    {
        public TimeRange Workdays { get; set; }
        public TimeRange Weekends { get; set; }
        public TimeRange Holidays { get; set; }

        public static WorkSchedule Default => new WorkSchedule
        {
            Workdays = new TimeRange
            {
                Start = new TimeSpan(10, 0, 0),
                End = new TimeSpan(18, 0, 0)
            },
            Weekends = new TimeRange
            {
                Start = new TimeSpan(10, 0, 0),
                End = new TimeSpan(18, 0, 0)
            },
            Holidays = new TimeRange
            {
                Start = new TimeSpan(10, 0, 0),
                End = new TimeSpan(18, 0, 0)
            }
        };
    }
}
