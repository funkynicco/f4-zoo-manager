using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Zoo.Animals
{
    public class Lion : Animal
    {
        /// <summary>
        /// Male <see cref="Lion"/> average weight is 190 kg.
        /// </summary>
        public const float AverageWeight = 190.0f;

        public Lion(Guid id, string name, TimeSpan? age = null) :
            base(id, name, AverageWeight, age)
        {
        }

        public override float CalculateRequiredFood()
            => 0.0f; // lions eat other animals, needs no man-made food
    }
}
