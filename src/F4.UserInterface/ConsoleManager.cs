using F4.UserInterface.Interfaces;
using F4.UserInterface.Interfaces.Windows;
using F4.UserInterface.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.UserInterface
{
    // "screen" classes to switch screen with?
    // 

    public class ConsoleManager : IConsoleManagerInternal, IConsoleManager
    {
        private readonly LinkedList<Window> _windows = new LinkedList<Window>();
        private Size _oldWindowSize = new Size(0, 0);
        private DateTime _lastWindowSizeUpdated = DateTime.UtcNow;
        private bool _dirty = false;

        private ConsoleManager()
        {
            _windows.AddLast(new Desktop(this));
            foreach (var window in _windows)
            {
                window.UpdateWindowRect();
            }
        }

        public void Process()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                _windows.Last.Value.OnInputKey(key);
            }

            var size = new Size(Console.WindowWidth, Console.WindowHeight);
            if (size != _oldWindowSize)
            {
                _oldWindowSize = size;
                _lastWindowSizeUpdated = DateTime.UtcNow;

                foreach (var window in _windows)
                {
                    window.UpdateWindowRect();
                }

                _dirty = true;
            }

            if (_dirty &&
                (DateTime.UtcNow - _lastWindowSizeUpdated).TotalMilliseconds >= 200) // don't redraw too fast or the window size may be updated before we manage to render all
            {
                _dirty = false;
                Redraw();
            }
        }

        public void Redraw()
        {
            Console.Clear();
            foreach (var window in _windows)
            {
                if (window.IsWindowRectInsideConsole())
                    window.Draw();
            }
        }

        public void Close(IWindow window)
        {
            if (_windows.Remove((Window)window))
                _dirty = true;
        }

        public void SendToBack(IWindow window)
        {
            var node = _windows.Find((Window)window);
            if (node == null)
                throw new ArgumentException("Window not found in the window stack.");

            // check if the window is the second in the stack
            if (_windows.Count <= 2 ||
                node == _windows.First.Next)
                return;

            // remove node and add it after the first (Desktop window)
            _windows.Remove(node);
            _windows.AddAfter(_windows.First, node);

            _dirty = true;
        }

        public void BringToFront(IWindow window)
        {
            var node = _windows.Find((Window)window);
            if (node == null)
                throw new ArgumentException("Window not found in the window stack.");

            if (_windows.Count <= 2 ||
                node == _windows.Last)
                return;

            // remove node and add it to the back
            _windows.Remove(node);
            _windows.AddLast(node);

            _dirty = true;
        }

        public void Invalidate(IWindow window)
        {
            _dirty = true;
        }

        public T CreateWindow<T>() where T : IWindow
        {
            Window window = null;

            if (typeof(T) == typeof(IMessage))
                window = new Message(this);
            else if (typeof(T) == typeof(IAnimalList))
                window = new AnimalList(this);
            else
                throw new ArgumentException($"The window of type {typeof(T).Name} could not be instantiated.");

            _windows.AddLast(window);
            window.UpdateWindowRect();
            _dirty = true;

            return (T)(object)window;
        }

        // static

        public static IConsoleManager Create()
            => new ConsoleManager();
    }
}
