using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace F4.Tests.Common
{
    public class TestRandomizer : IRandomizer
    {
        private readonly int[] _predictable_sequence;
        private int _next = 0;

        public IEnumerable<int> Sequence => _predictable_sequence;

        public TestRandomizer(int[] predictable_sequence)
        {
            Debug.Assert(predictable_sequence != null);
            Debug.Assert(predictable_sequence.Length > 0);
            _predictable_sequence = predictable_sequence;
        }

        public int Next(int min, int max)
        {
            var random = _predictable_sequence[_next++];
            if (_next == _predictable_sequence.Length)
                _next = 0;

            var range = max - min;
            random %= range;
            return min + random;
        }

        public int Next(int max)
            => Next(0, max);
    }
}
