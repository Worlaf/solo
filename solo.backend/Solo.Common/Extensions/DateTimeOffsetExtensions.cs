﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Solo.Common.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static bool IsEmpty(this DateTimeOffset self)
        {
            return self == DateTimeOffset.MinValue;
        }
    }
}
