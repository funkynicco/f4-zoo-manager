using System;
using System.Collections.Generic;
using System.Text;

namespace F4.Zoo.Interfaces.Models
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
