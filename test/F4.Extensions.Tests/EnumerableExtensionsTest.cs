using F4.Tests.Common;
using F4.Zoo.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace F4.Extensions.Tests
{
    public class EnumerableExtensionsTest
    {
        [Fact]
        public void RandomOrDefault_KnownValue_Expected()
        {
            // the 4th value in the random sequence is the lowest
            // which means itl be the first value in the OrderBy inside RandomOrDefault
            var randomizer = new TestRandomizer(new int[] { 4, 3, 6, 1, 9, 7, 5, 8, 10, 2 });

            var sequence = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var value = sequence.RandomOrDefault(randomizer);

            Assert.Equal(4, value);
        }
    }
}
