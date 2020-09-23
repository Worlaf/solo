using System;
using System.Collections.Generic;
using System.Text;

namespace Solo.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DecimalPrecisionAttribute : Attribute
    {
        public byte Precision { get; private set; }
        public byte Scale { get; private set; }

        public DecimalPrecisionAttribute(byte precision, byte scale)
        {
            Precision = precision;
            Scale = scale;
        }
    }
}
