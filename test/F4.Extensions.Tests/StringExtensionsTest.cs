using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace F4.Extensions.Tests
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("ab...", "abcdefghijk", 5, true)]
        [InlineData("abcde", "abcdefghijk", 5, false)]
        [InlineData("abcdefghijk", "abcdefghijk", 15, false)]
        [InlineData("..", "abcdefghijk", 2, true)]
        [InlineData("...", "abcdefghijk", 3, true)]
        public void Clip_ProvidedString_Expected(string expected, string str, int max_length, bool appendDots)
        {
            var clipped = str.Clip(max_length, appendDots);

            Assert.Equal(expected, clipped);
        }
    }
}
