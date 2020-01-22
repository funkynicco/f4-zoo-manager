using F4.UserInterface.Interfaces.Buffering;
using F4.UserInterface.Interfaces.Windows;
using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.UserInterface.Windows
{
    internal class AddAnimal : Window, IAddAnimal
    {
        private readonly IZooDatabase _database;

        public AddAnimal(IConsoleManagerInternal manager, IZooDatabase database) :
            base(manager)
        {
            _database = database;
            Title = "Add animal";
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

            screenBuffer.Draw(ClientRectangle.X, ClientRectangle.Y, "Add an animal");

            screenBuffer.Draw(ClientRectangle.X, ClientRectangle.Y + 2, "             ", ConsoleColor.Gray, ConsoleColor.Blue);
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
