using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Zoo.Animals
{
    public abstract class Animal : IAnimal
    {
        public Guid Id { get; }

        public string Species => GetType().Name;

        public string Name { get; }

        public float Weight { get; }

        public TimeSpan Age { get; set; }

        public Animal(Guid id, string name, float weight)
        {
            if (id == Guid.Empty)
                throw new ArgumentException($"The {nameof(id)} parameter cannot be empty.");

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length == 0)
                throw new ArgumentException($"{nameof(name)} must be a valid name.");

            Id = id;
            Name = name;
            Weight = weight;
        }

        public void AdvanceWeek() // proceed to next week by adding 7 days to the animals age
            => Age += TimeSpan.FromDays(7);

        public abstract float CalculateRequiredFood();
    }
}
