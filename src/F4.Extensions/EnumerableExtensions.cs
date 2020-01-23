using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F4.Extensions
{
    public static class EnumerableExtensions
    {
        private static readonly Random _random = new Random(Environment.TickCount);

        /// <summary>
        /// Picks a random item from an IEnumerable.
        /// </summary>
        public static T RandomOrDefault<T>(this IEnumerable<T> enumerable)
            => enumerable.OrderBy(a => _random.Next(10000)).FirstOrDefault();

        /// <summary>
        /// Picks a random item from an IEnumerable using a provided <see cref="IRandomizer"/>.
        /// </summary>
        public static T RandomOrDefault<T>(this IEnumerable<T> enumerable, IRandomizer randomizer)
            => enumerable.OrderBy(a => randomizer.Next(10000)).FirstOrDefault();
    }
}
