using F4.Zoo.Animals;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace F4.Zoo.Tests.Animals
{
    public class Wombat
    {
        [Theory]
        [InlineData(0, Zoo.Animals.Wombat.AverageWeight)]
        [InlineData(1, Zoo.Animals.Wombat.AverageWeight * 0.9f)]
        [InlineData(2, Zoo.Animals.Wombat.AverageWeight * 0.8f)]
        [InlineData(3, Zoo.Animals.Wombat.AverageWeight * 0.7f)]
        [InlineData(4, Zoo.Animals.Wombat.AverageWeight * 0.6f)]
        [InlineData(5, Zoo.Animals.Wombat.AverageWeight * 0.5f)]
        [InlineData(6, Zoo.Animals.Wombat.AverageWeight * 0.4f)]
        public void CalculateRequiredFood_Expected(int year, float expected)
        {
            var wombat = new Zoo.Animals.Wombat(Guid.NewGuid(), "some name");
            wombat.Age = TimeSpan.FromDays(365 * year);

            var requiredFood = wombat.CalculateRequiredFood();

            // normalize the values to prevent floating point errors down to 2 decimal points e.g: 0.01 kg
            expected = (float)(Math.Round(expected * 100) / 100);
            requiredFood = (float)(Math.Round(requiredFood * 100) / 100);

            Assert.Equal(expected, requiredFood);
        }
    }
}
