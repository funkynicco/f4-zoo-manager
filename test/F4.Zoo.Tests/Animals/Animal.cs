using F4.Zoo.Animals;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace F4.Zoo.Tests.Animals
{
    public class Animal
    {
        class TestAnimal : Zoo.Animals.Animal
        {
            public TestAnimal(Guid id, string name) :
                base(id, name, 0.0f)
            {
            }

            public override float CalculateRequiredFood()
                => throw new NotImplementedException();
        }

        [Theory]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(null, typeof(ArgumentNullException))]
        public void ConstructorNameArgument_EmptyNull_Throws(string name, Type expectedException)
        {
            Assert.Throws(expectedException, () => new TestAnimal(Guid.NewGuid(), name));
        }

        [Fact]
        public void ConstructorIdArgument_Empty_Throws()
        {
            Assert.Throws<ArgumentException>(() => new TestAnimal(Guid.Empty, "test"));
        }
    }
}
