using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Zoo.Interfaces
{
    public interface IZooDatabase
    {
        /// <summary>
        /// Gets the animals in the database.
        /// </summary>
        IEnumerable<IAnimal> Animals { get; }

        /// <summary>
        /// Creates an animal.
        /// </summary>
        /// <param name="animalType">Type of animal</param>
        /// <param name="name">Name of the animal</param>
        /// <param name="age">Optional age of animal</param>
        IAnimal Create(string animalType, string name, TimeSpan? age = null);

        /// <summary>
        /// Gets an animal by its unique id.
        /// </summary>
        /// <param name="id">Animal id</param>
        IAnimal Get(Guid id);

        /// <summary>
        /// Deletes an animal by its unique id.
        /// </summary>
        /// <param name="id">Animal id</param>
        bool Delete(Guid id);
    }
}
