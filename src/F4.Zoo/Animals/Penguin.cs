using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Zoo.Animals
{
    public class Penguin : Animal
    {
        /// <summary>
        /// Male <see cref="Penguin"/> average weight is 45 kg.
        /// </summary>
        public const float AverageWeight = 45.0f;

        public Penguin(Guid id, string name, TimeSpan? age = null) :
            base(id, name, AverageWeight, age)
        {
        }

        public override float CalculateRequiredFood()
            => 0.0f; // penguins source of food comes natural from a lake of fishes, needs no man-made food
    }
}
