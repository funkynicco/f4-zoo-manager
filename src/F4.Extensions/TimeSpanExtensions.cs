using System;

namespace F4.Extensions
{
    public static class TimeSpanExtensions
    {
        private const double DaysPerYear = 365; // approximation
        private const double MonthsPerYear = 12;
        private const double DaysPerMonth = DaysPerYear / MonthsPerYear;

        public static int AsYears(this TimeSpan timeSpan)
        {
            // WARN: only an appropximate of the years
            return (int)(timeSpan.TotalDays / DaysPerYear);
        }

        public static int AsMonths(this TimeSpan timeSpan)
        {
            // [INCORRECT MONTH CALCULATION] // Time calculation by hand is horrible difficult (see leap years etc)
            // uses approx. 30.41666 days as a constant average of days per month
            return (int)(timeSpan.TotalDays / DaysPerMonth);
        }

        /// <summary>
        /// Converts this TimeSpan into a birthday style format, e.g: "2 years old" or "5 months old"
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static string ToBirthdayFormat(this TimeSpan timeSpan)
        {
            var years = timeSpan.AsYears();
            if (years != 0)
                return $"{years} year{(years == 1 ? "" : "s")} old";

            var months = timeSpan.AsMonths();
            if (months != 0)
                return $"{months} month{(months == 1 ? "" : "s")} old";

            var days = (int)Math.Floor(timeSpan.TotalDays);
            return $"{days} day{(days == 1 ? "" : "s")} old";
        }
    }
}
