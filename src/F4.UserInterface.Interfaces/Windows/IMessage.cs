using System;
using System.Collections.Generic;
using System.Text;

namespace F4.UserInterface.Interfaces.Windows
{
    public interface IMessage : IWindow
    {
        void SetText(string message);
    }
}
