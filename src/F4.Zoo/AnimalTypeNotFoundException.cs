using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Zoo
{
    public class AnimalTypeNotFoundException : Exception
    {
        public AnimalTypeNotFoundException(string animalType) :
            base($"The animal of type {animalType} was not found.")
        {
        }
    }
}
