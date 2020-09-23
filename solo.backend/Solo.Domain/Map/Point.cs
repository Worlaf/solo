using System;
using System.Collections.Generic;
using System.Text;
using Solo.Common.Attributes;

namespace Solo.Domain.Map
{
    public class Point
    {
        [DecimalPrecision(18,10)]
        public decimal Longitude { get; set; }

        [DecimalPrecision(18,10)]
        public decimal Latitude { get; set; }

        public Point()
        {
            Latitude = 0;
            Longitude = 0;
        }

        public Point(decimal lat, decimal lon)
        {
            Latitude = lat;
            Longitude = lon;
        }

        public static Point FromStrangeCoord(decimal lat, decimal lon)
        {
            // 52.602510 / 39.614164 <- 56.243282 / 93.524950 [-3,640772 / -53,910786] - не точно
            // 52.604548 / 39.605544 <- 56.244223 / 93.524257 [-3,639675 / -53,918713]
            // 52.605478 / 39.602518 <- 56.244280 / 93.523656 [-3,638802 / -53,921138]
            // avg [3.639239 / 53.919926]
            return new Point(lat - 3.639239M, lon - 53.919926M);
        }
    }
}
