using F4.UserInterface.Interfaces.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.UserInterface.Windows
{
    internal class Desktop : Window, IDesktop
    {
        internal Desktop(IConsoleManagerInternal manager) :
            base(manager)
        {
        }

        public override void UpdateWindowRect()
            => WindowRectangle = GetConsoleRect(); // fill the entire console with the root "desktop"
    }
}
