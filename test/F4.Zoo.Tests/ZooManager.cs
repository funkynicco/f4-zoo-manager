using F4.Tests.Common;
using F4.Zoo.Animals;
using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace F4.Zoo.Tests
{
    public class ZooManager
    {
        private const int AmountOfAnimals = 12; // 4 species * 3 per species = 12

        private readonly TestRandomizer _randomizer = new TestRandomizer(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        private readonly TestZooDatabase _database = new TestZooDatabase();
        private readonly Zoo.ZooManager _manager;

        public ZooManager()
        {
            // create 3 of each animal species
            foreach (var species in new string[] { "lion", "panda", "penguin", "wombat" })
            {
                for (int i = 0; i < 3; ++i)
                {
                    _database.Create(species, $"{species}{i + 1}");
                }
            }

            Debug.Assert(_database.Animals.Count() == AmountOfAnimals);

            _manager = new Zoo.ZooManager(_randomizer, _database);
        }

        [Fact]
        public void AdvanceWeek_TargetExists_TargetKilled()
        {
            var killedAnimalResult = _manager.AdvanceWeek();

            Assert.NotNull(killedAnimalResult);
            Assert.NotNull(killedAnimalResult.Killer);
            Assert.NotNull(killedAnimalResult.Target);
            Assert.IsType<Lion>(killedAnimalResult.Killer);
            Assert.IsNotType<Lion>(killedAnimalResult.Target);
            Assert.IsNotType<Panda>(killedAnimalResult.Target);
            Assert.Equal(AmountOfAnimals - 1, _database.Animals.Count());
        }

        [Fact]
        public void AdvanceWeek_NoLions_NullResult()
        {
            // delete all lions
            var toDelete = new List<Guid>();
            toDelete.AddRange(_database.Animals.Where(a => a is Lion).Select(a => a.Id));
            toDelete.ForEach(id => _database.Delete(id));

            // advance week, should be null as there are no lions
            var killedAnimalResult = _manager.AdvanceWeek();

            Assert.Null(killedAnimalResult);
            Assert.Equal(AmountOfAnimals - 3, _database.Animals.Count()); // -3 animals as we deleted the 3 only lions
        }

        [Fact]
        public void AdvanceWeek_NoTargets_NullResult()
        {
            // delete all penguins and wombats
            var toDelete = new List<Guid>();
            toDelete.AddRange(_database.Animals.Where(a => (a is Penguin) || (a is Wombat)).Select(a => a.Id));
            toDelete.ForEach(id => _database.Delete(id));

            // advance week, should be null as there are no lions
            var killedAnimalResult = _manager.AdvanceWeek();

            Assert.Null(killedAnimalResult);
            Assert.Equal(AmountOfAnimals - 6, _database.Animals.Count()); // -6 animals as we deleted the penguins and wombats
        }

        [Fact]
        public void AdvanceWeek_AgesIncreased_Expected()
        {
            // generate an "expected" age dictionary from zoo animals
            var animalAgeExpected = new Dictionary<Guid, long>();
            foreach (var animal in _database.Animals)
            {
                animalAgeExpected[animal.Id] = (animal.Age + TimeSpan.FromDays(7)).Ticks;
            }

            // advance the week
            _manager.AdvanceWeek();

            // make sure that every animals age was increased to the expected value in the dictionary
            foreach (var kv in animalAgeExpected)
            {
                var animal = _database.Get(kv.Key);
                if (animal == null) // a lion killed this animal
                    continue;

                Assert.Equal(kv.Value, animal.Age.Ticks);
            }
        }
    }
}
