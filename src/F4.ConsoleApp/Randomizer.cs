using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace F4.ConsoleApp
{
    public class Randomizer : IRandomizer
    {
        private readonly Random _random;

        public Randomizer()
        {
            _random = new Random(Environment.TickCount);
        }

        public int Next(int min, int max)
            => _random.Next(min, max);

        public int Next(int max)
            => _random.Next(max);
    }
}
