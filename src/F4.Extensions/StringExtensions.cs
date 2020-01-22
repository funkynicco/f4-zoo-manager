using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Clips a string and optionally appends three dots to match a given maximum length.
        /// <para>The length includes the three dots.</para>
        /// </summary>
        /// <param name="max_length">Maximum length of string</param>
        /// <param name="appendDots">Whether to append dots</param>
        public static string Clip(this string str, int max_length, bool appendDots = true)
        {
            if (str.Length <= max_length)
                return str;

            var len = max_length;
            if (appendDots)
                len -= 3;

            if (len < 0)
                return string.Empty.PadRight(max_length, '.');

            if (appendDots)
                return $"{str.Substring(0, len)}...";

            return str.Substring(0, len);
        }
    }
}
