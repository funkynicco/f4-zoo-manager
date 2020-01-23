using F4.Zoo.Animals;
using F4.Zoo.Interfaces;
using F4.Zoo.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace F4.Zoo
{
    public class ZooManager : IZooManager
    {
        public IRandomizer Randomizer { get; }

        public IZooDatabase Database { get; }

        public ZooManager(IRandomizer randomizer, IZooDatabase database)
        {
            Randomizer = randomizer;
            Database = database;
        }

        public float CalculateWeekTotalFoodRequirement()
        {
            var total = 0.0f;
            foreach (var animal in Database.Animals)
            {
                total += animal.CalculateRequiredFood();
            }

            return total;
        }

        public IReadOnlyDictionary<string, float> CalculateIndividualSpeciesFoodRequirement()
        {
            var speciesFoodMap = new Dictionary<string, float>();

            foreach (var animal in Database.Animals)
            {
                // calculate food requirement for current animal
                var required = animal.CalculateRequiredFood();

                // try to get the species food requirement and if value is not found in map, the total is by default set to 0.0f
                speciesFoodMap.TryGetValue(animal.Species, out float total);
                total += required;
                speciesFoodMap[animal.Species] = total; // add or replace value in map
            }

            return speciesFoodMap;
        }

        public IReadOnlyList<IAnimal> GetLions()
        {
            var lions = new List<IAnimal>();

            foreach (var animal in Database.Animals)
            {
                if (animal is Lion)
                    lions.Add(animal);
            }

            return lions;
        }

        public IAnimal GetRandomLion()
        {
            var lions = GetLions();
            if (lions.Count == 0)
                return null;

            return lions[Randomizer.Next(lions.Count)];
        }

        public IAnimal FindTargetForLion(IAnimal lion)
        {
            Debug.Assert(lion is Lion);

            var targets = new List<IAnimal>();
            foreach (var animal in Database.Animals)
            {
                // can lion eat this animal?
                if (lion.CanEat(animal))
                    targets.Add(animal);
            }

            if (targets.Count == 0)
                return null;

            return targets[Randomizer.Next(targets.Count)];
        }

        // process logic for lion eating an animal here - AdvanceWeek returns the animal that was killed (if any)
        public KilledAnimalResult AdvanceWeek()
        {
            KilledAnimalResult result = null;

            // find a lion that will be the killer of a poor little zoo inhabitant
            var lion = GetRandomLion();
            if (lion != null)
            {
                // we have a lion, find a target for the hungry furrball
                var target = FindTargetForLion(lion);
                if (target != null)
                {
                    // target found and killed
                    result = new KilledAnimalResult(lion, target);
                    Database.Delete(target.Id);
                }
            }

            foreach (var animal in Database.Animals)
            {
                animal.AdvanceWeek();
            }

            return result;
        }

        public static IZooManager FromDatabase(IRandomizer randomizer, IZooDatabase database)
            => new ZooManager(randomizer, database);
    }
}
