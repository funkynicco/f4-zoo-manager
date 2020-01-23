using F4.UserInterface.Interfaces.Buffering;
using F4.UserInterface.Interfaces.Windows;
using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.UserInterface.Windows
{
    internal class Desktop : Window, IDesktop
    {
        private readonly IZooManager _zooManager;

        public Desktop(IConsoleManagerInternal manager, IZooManager zooManager) :
            base(manager)
        {
            _zooManager = zooManager;
        }

        public override void UpdateWindowRect()
            => WindowRectangle = GetConsoleRect(); // fill the entire console with the root "desktop"

        public override void Draw(IScreenBuffer screenBuffer)
        {
            base.Draw(screenBuffer);

            var clientRect = ClientRectangle;
            var y = 0;
            screenBuffer.Draw(clientRect.X, clientRect.Y + y++, "F1 - List animals");
            screenBuffer.Draw(clientRect.X, clientRect.Y + y++, "F2 - Advance week");
            screenBuffer.Draw(clientRect.X, clientRect.Y + y++, "F3 - Show this weeks food requirement");
            screenBuffer.Draw(clientRect.X, clientRect.Y + y++, "F4 - Wipe and restart database");
        }

        public override void OnInputKey(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.F1)
            {
                ConsoleManager.CreateWindow<IAnimalList>();
            }
            else if (key.Key == ConsoleKey.F2)
            {
                var killedAnimalResult = _zooManager.AdvanceWeek();

                var message =
                    killedAnimalResult != null ?
                    $"Week advanced, a poor {killedAnimalResult.Target.Species} named {killedAnimalResult.Target.Name} was killed by {killedAnimalResult.Killer.Name}!" :
                    "Week advanced, no animals were harmed!";

                ConsoleManager.CreateWindow<IMessage>()
                    .SetText(message);
            }
            else if (key.Key == ConsoleKey.F3)
            {
                ConsoleManager.CreateWindow<IShowAnimalRequirements>();
            }
            else if (key.Key == ConsoleKey.F4)
            {
                ConsoleManager.CreateWindow<IWipeDatabase>();
            }
        }
    }
}
