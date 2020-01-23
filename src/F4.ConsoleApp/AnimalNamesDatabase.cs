using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace F4.ConsoleApp
{
    public class AnimalNamesDatabase : IAnimalNamesDatabase
    {
        private readonly IRandomizer _randomizer;
        private readonly List<string> _names;

        public AnimalNamesDatabase(IRandomizer randomizer)
        {
            _randomizer = randomizer;
            _names = new List<string>(
                Media.animal_names.Split(new char[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries)
                .Where(a => a.Length != 0));
        }

        public string GetRandomName()
        {
            Debug.Assert(_names.Count != 0);

            var index = _randomizer.Next(_names.Count);
            var name = _names[index];
            
            _names.RemoveAt(index);
            return name;
        }
    }
}
