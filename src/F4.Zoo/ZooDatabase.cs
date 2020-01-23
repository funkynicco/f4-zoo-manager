using F4.Zoo.Animals;
using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace F4.Zoo
{
    public class ZooDatabase : IZooDatabase
    {
        private readonly IRandomizer _randomizer;
        private readonly IAnimalNamesDatabase _animalNamesDatabase;
        private readonly string _filename;
        private readonly Dictionary<Guid, IAnimal> _animals = new Dictionary<Guid, IAnimal>();

        public IEnumerable<IAnimal> Animals => _animals.Values;

        private ZooDatabase(IRandomizer randomizer, IAnimalNamesDatabase animalNamesDatabase, string filename)
        {
            _randomizer = randomizer;
            _animalNamesDatabase = animalNamesDatabase;
            _filename = filename;
        }

        private static Animal ConstructAnimal(Type type, Guid id, string name, TimeSpan? age = null)
        {
            // get the constructor that takes a string
            var constructor = type.GetConstructor(new Type[] { typeof(Guid), typeof(string), typeof(TimeSpan?) });
            if (constructor != null)
            {
                Animal animal;
                try
                {
                    animal = (Animal)constructor.Invoke(new object[] { id, name, age });
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }

                return animal;
            }

            throw new InvalidOperationException($"There is no suitable constructor for type {type.FullName} that takes in 'Guid id' and 'string name'.");
        }

        private static IReadOnlyList<Type> GetAnimalTypes()
        {
            var result = new List<Type>();

            // go through each type in this assembly (F4.Zoo) and find types that inherits from Animal
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsClass &&
                    type.IsSubclassOf(typeof(Animal)))
                    result.Add(type);
            }

            return result;
        }

        public IAnimal Create(string animalType, string name, TimeSpan? age = null)
        {
            var type = GetAnimalTypes()
                .Where(a => string.Compare(a.Name, animalType, true, CultureInfo.InvariantCulture) == 0)
                .FirstOrDefault();

            if (type == null)
                throw new AnimalTypeNotFoundException(animalType);

            var animal = ConstructAnimal(type, Guid.NewGuid(), name, age);

            // add animal to database
            _animals.Add(animal.Id, animal);

            // save database
            Save();

            return animal;
        }

        public IAnimal Get(Guid id)
        {
            _animals.TryGetValue(id, out IAnimal animal);
            return animal;
        }

        public bool Delete(Guid id)
        {
            if (!_animals.Remove(id))
                return false;

            Save();
            return true;
        }

        public void DeleteAll()
        {
            if (_animals.Count == 0)
                return;

            _animals.Clear();
            Save();
        }

        public void Reset(int numberOfAnimals = 10)
        {
            _animals.Clear();

            var animalTypes = GetAnimalTypes();
            for (int i = 0; i < numberOfAnimals; ++i)
            {
                var type = animalTypes[_randomizer.Next(animalTypes.Count)];
                var name = _animalNamesDatabase.GetRandomName();
                var age = TimeSpan.FromDays(_randomizer.Next(3650));

                var animal = ConstructAnimal(type, Guid.NewGuid(), name, age);
                _animals.Add(animal.Id, animal);
            }

            Save();
        }

        private void Save()
        {
            var doc = new XmlDocument();

            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            var animals = doc.CreateElement("Animals");
            foreach (var animal in _animals.Values)
            {
                var node = doc.CreateElement("Animal");
                node.SetAttribute("Type", animal.GetType().FullName);
                node.SetAttribute("Id", animal.Id.ToString());
                node.SetAttribute("Name", animal.Name);
                node.SetAttribute("Age", animal.Age.Ticks.ToString());
                animals.AppendChild(node);
            }

            var root = doc.CreateElement("Zoo");
            root.AppendChild(animals);
            doc.AppendChild(root);

            doc.Save(_filename);
        }

        public static IZooDatabase FromFile(IRandomizer randomizer, IAnimalNamesDatabase animalNamesDatabase, string filename, bool createIfNotExist = true)
        {
            var database = new ZooDatabase(randomizer, animalNamesDatabase, filename);

            if (createIfNotExist &&
                !File.Exists(filename))
            {
                database.Save(); // force generate an empty database file
                return database;
            }

            var doc = new XmlDocument();
            doc.Load(filename);

            foreach (XmlNode node in doc.SelectNodes("Zoo/Animals/Animal"))
            {
                // attempt to find the animal type by full name
                var type = Assembly.GetExecutingAssembly().GetType(node.Attributes["Type"].InnerText);
                if (type != null)
                {
                    var id = Guid.Parse(node.Attributes["Id"].InnerText);
                    var name = node.Attributes["Name"].InnerText;
                    var age = new TimeSpan(long.Parse(node.Attributes["Age"].InnerText));

                    // construct an instance of the animal
                    var animal = ConstructAnimal(type, id, name, age);

                    // add animal to database (not saved yet)
                    database._animals.Add(animal.Id, animal);
                }
            }

            return database;
        }
    }
}
