using F4.UserInterface.Buffering;
using F4.UserInterface.Interfaces;
using F4.UserInterface.Interfaces.Windows;
using F4.UserInterface.Windows;
using F4.Zoo.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IZooManager _zooManager;
        private readonly IRandomizer _randomizer;

        private readonly LinkedList<Window> _windows = new LinkedList<Window>();
        private Size _oldWindowSize = new Size(0, 0);
        private DateTime _lastWindowSizeUpdated = DateTime.UtcNow;
        private bool _dirty = false;

        private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        public IServiceProvider ServiceProvider { get; private set; }

        private readonly ScreenBufferManager _screenBufferManager;

        public Rectangle ConsoleRectangle => new Rectangle(0, 0, _screenBufferManager.Backbuffer.Width, _screenBufferManager.Backbuffer.Height);

        private ConsoleManager(IZooManager zooManager, IRandomizer randomizer)
        {
            _zooManager = zooManager;
            _randomizer = randomizer;

            RegisterDependences();

            _screenBufferManager = new ScreenBufferManager(Console.WindowWidth, Console.WindowHeight);

            CreateWindow<IDesktop>();
            foreach (var window in _windows)
            {
                window.UpdateWindowRect();
            }
        }

        public bool Process()
        {
            // did the Process method do something (in effect the callee wont sleep)
            var did_something = false;

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                _windows.Last.Value.OnInputKey(key);
                did_something = true;
            }

            var size = _screenBufferManager.GetConsoleWindowSize();
            if (size != _oldWindowSize)
            {
                _oldWindowSize = size;
                _lastWindowSizeUpdated = DateTime.UtcNow;
                _screenBufferManager.Resize(size.Width, size.Height);

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

            Console.CursorVisible = false;

            return did_something;
        }

        public void Redraw()
        {
            _screenBufferManager.Backbuffer.Clear();
            foreach (var window in _windows)
            {
                if (window.IsWindowRectInsideConsole())
                    window.Draw(_screenBufferManager.Backbuffer);
            }

            _screenBufferManager.Commit();
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

        private void RegisterDependences()
        {
            _serviceCollection.AddSingleton<IConsoleManagerInternal>(this);
            _serviceCollection.AddSingleton<IZooManager>(_zooManager);
            _serviceCollection.AddSingleton<IZooDatabase>(_zooManager.Database);
            _serviceCollection.AddSingleton<IRandomizer>(_randomizer);
            _serviceCollection.AddTransient<IDesktop, Desktop>();
            _serviceCollection.AddTransient<IMessage, Message>();
            _serviceCollection.AddTransient<IAnimalList, AnimalList>();
            _serviceCollection.AddTransient<IAddAnimal, AddAnimal>();
            _serviceCollection.AddTransient<IShowAnimalRequirements, ShowAnimalRequirements>();
            ServiceProvider = _serviceCollection.BuildServiceProvider();
        }

        public T CreateWindow<T>() where T : IWindow
        {
            var service = ServiceProvider.GetService<T>();
            if (service == null)
                throw new ArgumentException($"The window of type {typeof(T).Name} could not be instantiated.");

            var window = (Window)(object)service;

            _windows.AddLast(window);
            window.UpdateWindowRect();
            _dirty = true;

            return service;
        }

        // static

        public static IConsoleManager Create(IZooManager zooManager, IRandomizer randomizer)
            => new ConsoleManager(zooManager, randomizer);
    }
}
