using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Zoo.Interfaces
{
    public interface IAnimalNamesDatabase
    {
        /// <summary>
        /// Gets a random name from the database.
        /// </summary>
        string GetRandomName();
    }
}
