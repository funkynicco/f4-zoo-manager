using F4.UserInterface.Interfaces.Windows;
using System;

namespace F4.UserInterface.Interfaces
{
    public interface IConsoleManager
    {
        /// <summary>
        /// Processes the console manager logic.
        /// </summary>
        bool Process();

        /// <summary>
        /// Creates a window and adds it to the window stack.
        /// </summary>
        /// <typeparam name="T">Type of window</typeparam>
        T CreateWindow<T>() where T : IWindow;
    }
}
