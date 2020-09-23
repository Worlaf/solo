using System;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Solo.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToJson(this object self, bool enforceCamelCase = false)
        {
            if (self == null)
                return null;

            var settings = new JsonSerializerSettings
            {
                ContractResolver = enforceCamelCase ? new CamelCasePropertyNamesContractResolver() : new DefaultContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(self, settings);
        }

        public static T FromJson<T>(this string self)
        {
            if (self.IsEmpty())
                return default(T);

            return JsonConvert.DeserializeObject<T>(self);
        }

        public static object FromJson(this string self, Type type, bool safe = false)
        {
            if (self.IsEmpty())
                return null;

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
            };

            if (safe)
                settings.Error = (sender, args) => args.ErrorContext.Handled = true;

            if (type == typeof(ExpandoObject))
                settings.Converters.Add(new ExpandoObjectConverter());

            return JsonConvert.DeserializeObject(self, type, settings);
        }

        public static bool IsEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }

        public static string SplitUpperCase(this string self)
        {
            return Regex.Replace(self, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }

        public static string ToBase64(this string self, bool safe = false)
        {
            if (!safe)
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(self));

            if (self == null)
                return null;

            try
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(self));
            }
            catch
            {
                return null;
            }
        }

        public static string FirstLetterToLowerCase(this string self, string separator = null)
        {
            if (self.IsEmpty())
                return null;

            self = self.Trim();
            if (self.Length == 1)
                return self.ToLower();

            if (!separator.IsEmpty())
                return string.Join(separator, self.SplitText(separator).Select(s => s.FirstLetterToLowerCase()));

            return Char.ToLower(self[0]) + self.Substring(1);
        }

        public static string FirstLetterToUpperCase(this string self)
        {
            if (self.IsEmpty())
                return null;

            self = self.Trim();
            if (self.Length == 1)
                return self.ToUpper();

            return Char.ToUpper(self[0]) + self.Substring(1);
        }

        public static string[] SplitText(this string self, string symbol)
        {
            return self.IsEmpty() ? new string[] { } : self.Split(new[] { symbol }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool IsWebUri(this string self)
        {
            return Uri.TryCreate(self, UriKind.Absolute, out var validatedUri) && (validatedUri.Scheme == Uri.UriSchemeHttp || validatedUri.Scheme == Uri.UriSchemeHttps);
        }

        public static byte[] ToBytes(this string self)
        {
            return self.IsEmpty() ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(self);
        }

        public static string ToCamelCase(this string self, bool upperFirstLetter = true)
        {
            if (self == null || self.Length < 2)
                return self;

            var words = self.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);

            var result = words[0].ToLower();
            for (var i = 1; i < words.Length; i++)
            {
                result += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1);
            }

            return upperFirstLetter ? result.FirstLetterToUpperCase() : result.FirstLetterToLowerCase();
        }

        public static int AsInt(this object self, bool safe = false)
        {
            if (!safe)
            {
                if (self == null)
                    throw new ArgumentNullException(nameof(self));

                return Convert.ToInt32(self);
            }

            if (self is string selfStr)
                return !int.TryParse(selfStr, out var value) ? 0 : value;

            try
            {
                return Convert.ToInt32(self);
            }
            catch
            {
                return 0;
            }
        }

        #region - AsEnum -

        public static T AsEnum<T>(this object self, bool safe = false)
        {
            if (self is int) return (T)self;

            if (!safe)
            {
                if (self == null)
                    throw new ArgumentNullException(nameof(self));

                return (T)Enum.Parse(typeof(T), (string)self, true);
            }

            try
            {
                return (T)Enum.Parse(typeof(T), (string)self, true);
            }
            catch
            {
                return default(T);
            }
        }

        #endregion
    }
}
