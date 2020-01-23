using F4.UserInterface.Interfaces.Buffering;
using F4.UserInterface.Interfaces.Windows;
using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.UserInterface.Windows
{
    internal class WipeDatabase : Window, IWipeDatabase
    {
        private readonly IZooDatabase _zooDatabase;

        public WipeDatabase(IConsoleManagerInternal manager, IZooDatabase zooDatabase) :
            base(manager)
        {
            _zooDatabase = zooDatabase;
            Title = "Confirm wipe database";
        }

        public override void UpdateWindowRect()
        {
            var rect = new Rectangle(0, 0, 40, 4);
            CenterRectangle(ref rect);
            WindowRectangle = rect;
        }

        public override void Draw(IScreenBuffer screenBuffer)
        {
            base.Draw(screenBuffer);

            var rc = ClientRectangle;

            var text1 = "Do you want to reset database?";
            var text2 = "Press Y for yes or Escape to cancel.";
            screenBuffer.Draw(rc.X + rc.Width / 2 - text1.Length / 2, rc.Y, text1);
            screenBuffer.Draw(rc.X + rc.Width / 2 - text2.Length / 2, rc.Y + 1, text2);
        }

        public override void OnInputKey(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                Close();
                return;
            }

            if (key.Key == ConsoleKey.Y)
            {
                _zooDatabase.Reset();
                Close();
                return;
            }
        }
    }
}
