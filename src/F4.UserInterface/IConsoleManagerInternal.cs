using F4.UserInterface.Buffering;
using F4.UserInterface.Interfaces;
using F4.UserInterface.Interfaces.Windows;
using F4.UserInterface.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.UserInterface
{
    /// <summary>
    /// Console manager interface to access internal things
    /// </summary>
    internal interface IConsoleManagerInternal : IConsoleManager
    {
        Rectangle ConsoleRectangle { get; }

        void Redraw();
        
        void Close(IWindow window);
        
        void SendToBack(IWindow window);
        
        void BringToFront(IWindow window);

        void Invalidate(IWindow window);
    }
}
