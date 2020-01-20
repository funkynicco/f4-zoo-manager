using F4.UserInterface;
using F4.UserInterface.Interfaces.Windows;
using F4.Zoo;
using F4.Zoo.Animals;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

// database file of zoo animals / store state? xml?
// can add animals / menu system? list animals like a db?
// maybe a menu like f1 to advance week / f2 to add animal (enter parameters)
// 

namespace F4.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Zoo Manager 2020";

            var zoo = ZooManager.FromDatabase(ZooDatabase.FromFile("zoo.xml"));

            var consoleManager = ConsoleManager.Create();

            consoleManager.CreateWindow<IAnimalList>()
                .SetZooDatabase(zoo.Database);

            consoleManager.CreateWindow<IMessage>()
                .SetText("Johnny the Crazy Elephant was killed by Furrball.");

            consoleManager.CreateWindow<IMessage>()
                .SetText("hello world");

            while (true)
            {
                consoleManager.Process();
                Thread.Sleep(50);
            }
        }
    }
}
