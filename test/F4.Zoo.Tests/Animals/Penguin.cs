using F4.Zoo.Animals;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace F4.Zoo.Tests.Animals
{
    public class Penguin
    {
        [Theory]
        [InlineData(0, 0)] // penguin never needs food
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(3, 0)]
        public void CalculateRequiredFood_Expected(int year, int expected)
        {
            var penguin = new Zoo.Animals.Penguin(Guid.NewGuid(), "some name");
            penguin.Age = TimeSpan.FromDays(365 * year);

            float requiredFood = penguin.CalculateRequiredFood();

            Assert.Equal(expected, requiredFood);
        }
    }
}
