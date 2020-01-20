using F4.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Zoo.Animals
{
    public class Panda : Animal
    {
        /// <summary>
        /// Male <see cref="Panda"/> average weight is 115 kg.
        /// </summary>
        public const float AverageWeight = 115.0f;

        /// <summary>
        /// Maximum amount of years to base food calculation on.
        /// </summary>
        public const int MaximumFoodYears = 6;

        /// <summary>
        /// The base amount of food (2 kg) the <see cref="Panda"/> requires per week.
        /// </summary>
        public const float BaseFoodRequirement = 2.0f;

        public Panda(Guid id, string name) :
            base(id, name, AverageWeight)
        {
        }

        public override float CalculateRequiredFood()
        {
            // calculate the maximum amount of years to base food on
            var years = Math.Min(MaximumFoodYears, Age.AsYears());
            
            // food is in kilograms
            var requiredFood = BaseFoodRequirement * years;
            return requiredFood;
        }
    }
}
