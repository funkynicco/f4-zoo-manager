using F4.Zoo.Animals;
using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Zoo
{
    public class ZooManager : IZooManager
    {
        private readonly IZooDatabase _database;

        public IZooDatabase Database => _database;

        private ZooManager(IZooDatabase database)
        {
            _database = database;
        }

        public static IZooManager FromDatabase(IZooDatabase database)
            => new ZooManager(database);
    }
}
