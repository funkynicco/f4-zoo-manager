﻿using F4.Extensions;
using F4.UserInterface.Interfaces.Windows;
using F4.UserInterface.Interfaces.Buffering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using F4.UserInterface.Buffering;

namespace F4.UserInterface.Windows
{
    internal abstract class Window : IWindow
    {
        protected IConsoleManagerInternal ConsoleManager { get; }

        public Rectangle WindowRectangle { get; protected set; }

        public Rectangle ClientRectangle
        {
            get
            {
                var rect = WindowRectangle;
                return new Rectangle(
                    rect.X + 1,
                    rect.Y + 1,
                    Math.Max(0, rect.Width - 2),
                    Math.Max(0, rect.Height - 2));
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                ConsoleManager.Redraw(); // TODO: don't redraw entire stack (very inefficient)
            }
        }

        public Window(IConsoleManagerInternal manager)
        {
            ConsoleManager = manager;
            _title = GetType().Name;
        }

        protected Rectangle GetConsoleRect()
            => ConsoleManager.ConsoleRectangle;

        public void Close()
            => ConsoleManager.Close(this);

        public void SendToBack()
            => ConsoleManager.SendToBack(this);

        public void BringToFront()
            => ConsoleManager.BringToFront(this);

        protected void Invalidate()
            => ConsoleManager.Invalidate(this);

        private void DrawWindowBorders(IScreenBuffer screenBuffer)
        {
            screenBuffer.Draw(WindowRectangle.X, WindowRectangle.Y, BorderCharacters.TopLeftCorner);
            screenBuffer.Draw(WindowRectangle.X, WindowRectangle.Y + WindowRectangle.Height - 1, BorderCharacters.BottomLeftCorner);
            for (int x = 0; x < WindowRectangle.Width - 2; ++x)
            {
                screenBuffer.Draw(WindowRectangle.X + x + 1, WindowRectangle.Y, BorderCharacters.HorizontalLine);
                screenBuffer.Draw(WindowRectangle.X + x + 1, WindowRectangle.Y + WindowRectangle.Height - 1, BorderCharacters.HorizontalLine);
            }
            screenBuffer.Draw(WindowRectangle.X + WindowRectangle.Width - 1, WindowRectangle.Y, BorderCharacters.TopRightCorner);
            screenBuffer.Draw(WindowRectangle.X + WindowRectangle.Width - 1, WindowRectangle.Y + WindowRectangle.Height - 1, BorderCharacters.BottomRightCorner);

            // vertical bars
            for (int y = 0; y < WindowRectangle.Height - 2; ++y)
            {
                screenBuffer.Draw(WindowRectangle.X, WindowRectangle.Y + y + 1, BorderCharacters.VerticalLine);
                screenBuffer.Draw(WindowRectangle.X + WindowRectangle.Width - 1, WindowRectangle.Y + y + 1, BorderCharacters.VerticalLine);
            }

            // draw window title if its not too long
            if (_title.Length <= WindowRectangle.Width - 2)
            {
                // draw at the center top
                screenBuffer.Draw(
                    WindowRectangle.X + WindowRectangle.Width / 2 - _title.Length / 2,
                    WindowRectangle.Y,
                    _title);
            }
        }

        protected void CenterRectangle(ref Rectangle rect)
        {
            var consoleRect = GetConsoleRect();
            rect.X = consoleRect.X + consoleRect.Width / 2 - rect.Width / 2;
            rect.Y = consoleRect.Y + consoleRect.Height / 2 - rect.Height / 2;
        }

        public abstract void UpdateWindowRect();

        public bool IsWindowRectInsideConsole()
        {
            var consoleRect = GetConsoleRect();
            return
                WindowRectangle.X >= 0 &&
                WindowRectangle.Y >= 0 &&
                WindowRectangle.Right <= consoleRect.Right &&
                WindowRectangle.Bottom <= consoleRect.Bottom;
        }

        public virtual void Draw(IScreenBuffer screenBuffer)
        {
            screenBuffer.Clear(WindowRectangle);
            DrawWindowBorders(screenBuffer);
        }

        public virtual void OnInputKey(ConsoleKeyInfo key)
        {
        }
    }
}
