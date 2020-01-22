using F4.UserInterface.Interfaces.Buffering;
using F4.UserInterface.Interfaces.Windows;
using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            var rect = new Rectangle(0, 0, 30, 30);
            CenterRectangle(ref rect);
            WindowRectangle = rect;
        }

        public override void Draw(IScreenBuffer screenBuffer)
        {
            base.Draw(screenBuffer);

            screenBuffer.Draw(ClientRectangle.X, ClientRectangle.Y, "Animal food requirements");
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
