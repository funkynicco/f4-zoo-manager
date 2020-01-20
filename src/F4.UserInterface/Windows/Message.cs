using F4.Extensions;
using F4.UserInterface.Interfaces.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.UserInterface.Windows
{
    internal class Message : Window, IMessage
    {
        private string _message = string.Empty;

        public Message(IConsoleManagerInternal manager) :
            base(manager)
        {
        }

        public override void UpdateWindowRect()
        {
            var clientRect = GetConsoleRect();

            // center window
            var width = _message.Length + 2;
            var height = 3;
            var x = clientRect.X + clientRect.Width / 2 - width / 2;
            var y = clientRect.Y + clientRect.Height / 2 - height / 2;
            WindowRectangle = new Rectangle(x, y, width, height);
        }

        public void SetText(string message)
        {
            _message = message;
            UpdateWindowRect();
            ConsoleManager.Redraw();
        }

        public override void Draw()
        {
            base.Draw();
            ConsoleHelper.Draw(ClientRectangle.X, ClientRectangle.Y, _message);
        }

        public override void OnInputKey(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
                Close(); // close message box
        }
    }
}
