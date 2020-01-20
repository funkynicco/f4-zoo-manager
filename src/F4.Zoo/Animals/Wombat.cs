using F4.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Zoo.Animals
{
    public class Wombat : Animal
    {
        /// <summary>
        /// <see cref="Wombat"/> average weight is 26 kg.
        /// </summary>
        public const float AverageWeight = 26.0f;

        /// <summary>
        /// Maximum amount of years to base food calculation on.
        /// </summary>
        public const int MaximumFoodYears = 6;

        /// <summary>
        /// The initial factor (100%) of food based on their weight that <see cref="Wombat"/> eats every week.
        /// </summary>
        public const float InitialFoodFactor = 1.0f;

        /// <summary>
        /// The factor (10%) to subtract from <see cref="InitialFoodFactor"/> every year the <see cref="Wombat"/> grows.
        /// </summary>
        public const float FactorMinusPerYear = 0.1f;

        public Wombat(Guid id, string name) :
            base(id, name, AverageWeight)
        {
        }

        public override float CalculateRequiredFood()
        {
            // calculate the maximum amount of years to base food on
            var years = Math.Min(MaximumFoodYears, Age.AsYears());

            // calculate the factor to multiply weight with
            var factor = InitialFoodFactor - (FactorMinusPerYear * years);

            var requiredFood = Weight * factor;
            return requiredFood;
        }
    }
}
