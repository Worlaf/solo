using System;
using System.Collections.Generic;
using System.Text;

namespace Solo.Domain.Map
{
    public class Region
    {
        public IReadOnlyCollection<Point> Points { get; set; }
    }
}
