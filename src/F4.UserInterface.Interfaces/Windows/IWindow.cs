using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.UserInterface.Interfaces.Windows
{
    public interface IWindow
    {
        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Closes this window.
        /// </summary>
        void Close();

        /// <summary>
        /// Sends the window to the back of the drawing stack.
        /// </summary>
        void SendToBack();

        /// <summary>
        /// Brings the window to the front of the drawing stack.
        /// </summary>
        void BringToFront();
    }
}
