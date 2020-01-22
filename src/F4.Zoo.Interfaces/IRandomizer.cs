using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Zoo.Interfaces
{
    /// <summary>
    /// Represents a randomizer.
    /// </summary>
    public interface IRandomizer
    {
        /// <summary>
        /// Gets the next random value within a user provided range.
        /// </summary>
        /// <param name="min">Minimum random value</param>
        /// <param name="max">Maximum random value (exclusive)</param>
        int Next(int min, int max);

        /// <summary>
        /// Gets the next random value between 0 and max (exclusive).
        /// </summary>
        /// <param name="max">Maximum random value (exclusive)</param>
        int Next(int max);
    }
}
