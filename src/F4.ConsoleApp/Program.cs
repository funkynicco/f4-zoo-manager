using F4.UserInterface;
using F4.UserInterface.Interfaces.Windows;
using F4.Zoo;
using F4.Zoo.Animals;
using F4.Zoo.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace F4.ConsoleApp
{
    class Program
    {
        static void CreateRandomAnimals(IZooDatabase database) // temporary placement
        {
            database.DeleteAll();

            const int NumberOfAnimalsToAdd = 30;
            var random = new Random(Environment.TickCount);
            var animalSpecies = new string[] { "lion", "panda", "penguin", "wombat" };
            var animalNames = new List<string>(
                Media.animal_names.Split(new char[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries)
                .Where(a => a.Length != 0));

            for (int i = 0; i < NumberOfAnimalsToAdd; i++)
            {
                var species = animalSpecies[random.Next(animalSpecies.Length)];
                var name_index = random.Next(animalNames.Count);
                var name = animalNames[name_index];
                animalNames.RemoveAt(name_index);

                database.Create(species, name);
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Zoo Manager 2020";

            var zoo = ZooManager.FromDatabase(ZooDatabase.FromFile("zoo.xml"));

            CreateRandomAnimals(zoo.Database);

            var consoleManager = ConsoleManager.Create(zoo);

            //consoleManager.CreateWindow<IAnimalList>()
            //    .SetZooDatabase(zoo.Database);

            //consoleManager.CreateWindow<IMessage>()
            //    .SetText("Johnny the Crazy Elephant was killed by Furrball.");

            //consoleManager.CreateWindow<IMessage>()
            //    .SetText("hello world");

            while (true)
            {
                if (!consoleManager.Process())
                    Thread.Sleep(50);
            }
        }
    }
}
