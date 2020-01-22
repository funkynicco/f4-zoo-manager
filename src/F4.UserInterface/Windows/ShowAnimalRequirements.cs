using F4.UserInterface.Interfaces.Buffering;
using F4.UserInterface.Interfaces.Windows;
using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace F4.UserInterface.Windows
{
    internal class ShowAnimalRequirements : Window, IShowAnimalRequirements
    {
        private readonly IZooManager _zooManager;

        public ShowAnimalRequirements(IConsoleManagerInternal manager, IZooManager zooManager) :
            base(manager)
        {
            _zooManager = zooManager;
            Title = "Animal Food Requirements";
        }

        public override void UpdateWindowRect()
        {
            var rect = new Rectangle(0, 0, 40, 10);
            CenterRectangle(ref rect);
            WindowRectangle = rect;
        }

        public override void Draw(IScreenBuffer screenBuffer)
        {
            base.Draw(screenBuffer);

            var totalFoodRequirement = _zooManager.CalculateWeekTotalFoodRequirement();

            var rc = ClientRectangle;
            int y = 0;
            screenBuffer.Draw(rc.X, rc.Y + y, "Animal food requirements this week");
            y += 2;

            // print out each individual species and their food requirement
            foreach (var kv in _zooManager.CalculateIndividualSpeciesFoodRequirement().OrderBy(a => a.Key))
            {
                var species = kv.Key;
                var requirement = kv.Value;
                screenBuffer.Draw(rc.X, rc.Y + y++, $"[{species}] {requirement:0.00} kg");
            }

            ++y;
            screenBuffer.Draw(rc.X, rc.Y + y++, $"Total required food: {totalFoodRequirement:0.00} kg");
        }

        public override void OnInputKey(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                Close();
                return;
            }
        }
    }
}
