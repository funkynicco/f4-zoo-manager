using F4.Zoo.Animals;
using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F4.Tests.Common
{
    public class TestZooDatabase : IZooDatabase
    {
        private List<IAnimal> _animals = new List<IAnimal>();

        public IEnumerable<IAnimal> Animals => _animals;

        public IAnimal Create(string animalType, string name, TimeSpan? age = null)
        {
            IAnimal animal = null;
            switch (animalType.ToLowerInvariant())
            {
                case "lion": animal = new Lion(Guid.NewGuid(), name, age); break;
                case "panda": animal = new Panda(Guid.NewGuid(), name, age); break;
                case "penguin": animal = new Penguin(Guid.NewGuid(), name, age); break;
                case "wombat": animal = new Wombat(Guid.NewGuid(), name, age); break;
            }

            if (animal == null)
                throw new ArgumentException($"Species {animalType} not found.");

            _animals.Add(animal);
            return animal;
        }

        public bool Delete(Guid id)
            => _animals.RemoveAll(a => a.Id == id) != 0;

        public void DeleteAll()
            => _animals.Clear();

        public IAnimal Get(Guid id)
            => _animals.Where(a => a.Id == id).FirstOrDefault();
    }
}
