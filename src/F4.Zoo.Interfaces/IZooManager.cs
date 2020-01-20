using System;
using System.Collections.Generic;

namespace F4.Zoo.Interfaces
{
    public interface IZooManager
    {
        /// <summary>
        /// Gets the database of the zoo.
        /// </summary>
        IZooDatabase Database { get; }
    }
}
