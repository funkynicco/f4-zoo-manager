using System;
using Xunit;

namespace F4.Extensions.Tests
{
    public class TimeSpanExtensionsTest
    {
        [Theory]
        [InlineData(1990, 1993, 3)]
        [InlineData(2019, 2020, 1)]
        public void AsYears_ValidateYear(int year1, int year2, int expected)
        {
            var date1 = new DateTime(year1, 1, 1);
            var date2 = new DateTime(year2, 1, 1);

            var timeSpan = date2 - date1;
            var years = timeSpan.AsYears();

            Assert.Equal(expected, years);
        }
    }
}
