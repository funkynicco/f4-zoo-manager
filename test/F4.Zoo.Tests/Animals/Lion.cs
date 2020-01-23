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

        [Theory]
        [InlineData("lion", false)]
        [InlineData("panda", false)]
        [InlineData("penguin", true)]
        [InlineData("wombat", true)]
        public void CanEat_Animals_Expected(string animalType, bool expected)
        {
            var lion = new Zoo.Animals.Lion(Guid.NewGuid(), "some name");

            var canEat = false;
            switch (animalType)
            {
                case "lion": canEat = lion.CanEat(new Zoo.Animals.Lion(Guid.NewGuid(), "target")); break;
                case "panda": canEat = lion.CanEat(new Zoo.Animals.Panda(Guid.NewGuid(), "target")); break;
                case "penguin": canEat = lion.CanEat(new Zoo.Animals.Penguin(Guid.NewGuid(), "target")); break;
                case "wombat": canEat = lion.CanEat(new Zoo.Animals.Wombat(Guid.NewGuid(), "target")); break;
            }

            Assert.Equal(expected, canEat);
        }
    }
}
