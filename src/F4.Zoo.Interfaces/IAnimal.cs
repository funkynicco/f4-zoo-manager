using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Zoo.Interfaces
{
    /// <summary>
    /// Represents an animal.
    /// </summary>
    public interface IAnimal
    {
        /// <summary>
        /// Gets the unique id of the animal.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the animal species.
        /// </summary>
        string Species { get; }

        /// <summary>
        /// The name of the animal.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The weight in kilograms of the animal regardless of the age.
        /// </summary>
        float Weight { get; }

        /// <summary>
        /// The age of the animal.
        /// </summary>
        TimeSpan Age { get; }

        /// <summary>
        /// Adds 7 days to the animals age.
        /// </summary>
        void AdvanceWeek();

        /// <summary>
        /// Calculates the required food to feed the animal based on its type, age and weight.
        /// </summary>
        float CalculateRequiredFood();
    }
}
