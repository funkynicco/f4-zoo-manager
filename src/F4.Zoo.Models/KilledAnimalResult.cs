using F4.Zoo.Interfaces;
using System;

namespace F4.Zoo.Models
{
    public class KilledAnimalResult
    {
        public IAnimal Killer { get; }
        
        public IAnimal Target { get; }

        public KilledAnimalResult(IAnimal killer, IAnimal target)
        {
            Killer = killer;
            Target = target;
        }
    }
}
