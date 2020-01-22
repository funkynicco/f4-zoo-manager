using F4.Zoo.Interfaces.Models;
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

        /// <summary>
        /// Calculates the total food requirement of all animals this week.
        /// </summary>
        float CalculateWeekTotalFoodRequirement();

        /// <summary>
        /// Calculates the total food requirement of all animals this week divided into a species map.
        /// </summary>
        IReadOnlyDictionary<string, float> CalculateIndividualSpeciesFoodRequirement();

        /// <summary>
        /// Gets a list of all the lions in the zoo.
        /// </summary>
        IReadOnlyList<IAnimal> GetLions();

        /// <summary>
        /// Gets a random lion from the zoo.
        /// </summary>
        IAnimal GetRandomLion();

        /// <summary>
        /// Finds a target for the lion.
        /// </summary>
        IAnimal FindTargetForLion(IAnimal lion);

        /// <summary>
        /// Advances the week of the zoo, incrementing all animals age. If there is a lion in the zoo, a random valid animal may be killed.
        /// <para>Returns the animal that was killed by a lion and the lion itself or null if no animal was killed.</para>
        /// </summary>
        KilledAnimalResult AdvanceWeek();
    }
}
