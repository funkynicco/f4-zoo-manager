using System;

namespace F4.Extensions
{
    public static class TimeSpanExtensions
    {
        public static int AsYears(this TimeSpan timeSpan)
        {
            // appropximate the years
            return (int)(timeSpan.TotalDays / 365);
        }
    }
}
