using System;
using System.Text.RegularExpressions;

namespace TmsCollectorAndroid.Extensions
{
    public static class StringExtension
    {
        public static bool IsInt(this string text)
        {
            if(!string.IsNullOrEmpty(text))
                return int.TryParse(text, out var intVal);

            return false;
        }

        public static int ToInt(this string text)
        {
            if (text.IsInt())
            {
                int.TryParse(text, out var intVal);
                return intVal;
            }

            return 0;
        }

        public static string ToDigits(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var regex = new Regex(@"[^\d]");
                var digits = regex.Replace(text, "");
                return digits;
            }

            return String.Empty;
        }

        public static bool IsDateFormated(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            var regex = new Regex(@"^\d{2}/\d{2}/\d{4}$");
            var result = regex.Match(text);
            return result.Success;
        }

        public static bool IsTimeFormated(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            var regex = new Regex(@"^\d{2}:\d{2}$");
            var result = regex.Match(text);
            return result.Success;
        }
    }
}