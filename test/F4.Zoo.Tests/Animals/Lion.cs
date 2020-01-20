using F4.Zoo.Animals;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace F4.Zoo.Tests.Animals
{
    public class Lion
    {
        [Theory]
        [InlineData(0, 0)] // lion never needs food
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(3, 0)]
        public void CalculateRequiredFood_Expected(int year, int expected)
        {
            var lion = new Zoo.Animals.Lion(Guid.NewGuid(), "some name");
            lion.Age = TimeSpan.FromDays(365 * year);

            float requiredFood = lion.CalculateRequiredFood();

            Assert.Equal(expected, requiredFood);
        }
    }
}
