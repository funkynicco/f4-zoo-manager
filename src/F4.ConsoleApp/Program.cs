using F4.Extensions;
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
        static void Main(string[] args)
        {
            Console.Title = "Zoo Manager 2020";

            var randomizer = new Randomizer();
            var animalNamesDatabase = new AnimalNamesDatabase(randomizer);
            var database = ZooDatabase.FromFile(randomizer, animalNamesDatabase, "zoo.xml");
            var zoo = ZooManager.FromDatabase(randomizer, database);

            var consoleManager = ConsoleManager.Create(zoo, randomizer);

            while (true)
            {
                if (!consoleManager.Process())
                    Thread.Sleep(50);
            }
        }
    }
}
