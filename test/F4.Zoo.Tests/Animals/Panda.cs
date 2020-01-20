using F4.Zoo.Animals;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace F4.Zoo.Tests.Animals
{
    public class Panda
    {
        [Theory]
        [InlineData(0, 0)] // 0 food required if 0 years
        [InlineData(1, 2)] // 2 kg food required if 1 year
        [InlineData(2, 4)] // 4 kg food required if 2 years
        [InlineData(3, 6)] // 6 kg food required if 3 years
        public void CalculateRequiredFood_Expected(int year, float expected)
        {
            var panda = new Zoo.Animals.Panda(Guid.NewGuid(), "some name");
            panda.Age = TimeSpan.FromDays(365 * year);

            var requiredFood = panda.CalculateRequiredFood();

            Assert.Equal(expected, requiredFood);
        }
    }
}
