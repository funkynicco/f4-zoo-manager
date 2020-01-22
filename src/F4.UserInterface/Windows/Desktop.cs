using F4.UserInterface.Interfaces.Buffering;
using F4.UserInterface.Interfaces.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.UserInterface.Windows
{
    internal class Desktop : Window, IDesktop
    {
        public Desktop(IConsoleManagerInternal manager) :
            base(manager)
        {
        }

        public override void UpdateWindowRect()
            => WindowRectangle = GetConsoleRect(); // fill the entire console with the root "desktop"

        public override void Draw(IScreenBuffer screenBuffer)
        {
            base.Draw(screenBuffer);

            var clientRect = ClientRectangle;
            screenBuffer.Draw(clientRect.X, clientRect.Y, "F1 - List animals");
            screenBuffer.Draw(clientRect.X, clientRect.Y + 1, "F2 - Add animal");
            screenBuffer.Draw(clientRect.X, clientRect.Y + 2, "F3 - Advance week");
            screenBuffer.Draw(clientRect.X, clientRect.Y + 3, "F4 - Show this weeks food requirement");
        }

        public override void OnInputKey(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.F1)
            {
                ConsoleManager.CreateWindow<IAnimalList>();
            }
            else if (key.Key == ConsoleKey.F2)
            {
                ConsoleManager.CreateWindow<IAddAnimal>();
            }
            else if (key.Key == ConsoleKey.F3)
            {
                //ConsoleManager.CreateWindow<>
            }
            else if (key.Key == ConsoleKey.F4)
            {
                ConsoleManager.CreateWindow<IShowAnimalRequirements>();
            }
        }
    }
}
