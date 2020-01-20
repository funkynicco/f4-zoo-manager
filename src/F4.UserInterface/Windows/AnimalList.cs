﻿using F4.Extensions;
using F4.UserInterface.Interfaces.Windows;
using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace F4.UserInterface.Windows
{
    internal class AnimalList : Window, IAnimalList
    {
        private IZooDatabase _database;
        private int _selectedIndex = 0;

        public AnimalList(IConsoleManagerInternal manager) :
            base(manager)
        {
            Title = "Animals";
        }

        public override void UpdateWindowRect()
        {
            var consoleRect = GetConsoleRect();
            var rect = new Rectangle(
                0,
                0,
                Math.Min(100, consoleRect.Width),
                Math.Min(30, consoleRect.Height));

            CenterRectangle(ref rect);

            WindowRectangle = rect;
        }

        public void SetZooDatabase(IZooDatabase database)
        {
            _database = database;
        }

        public override void Draw()
        {
            base.Draw();

            if (_database == null)
                return;

            ConsoleHelper.Draw(ClientRectangle.X, ClientRectangle.Y, "Species");
            ConsoleHelper.Draw(ClientRectangle.X + 10, ClientRectangle.Y, "Name");
            ConsoleHelper.Draw(ClientRectangle.X + 30, ClientRectangle.Y, "Age");

            int index = 0;

            int y = ClientRectangle.Y + 2;
            foreach (var animal in _database.Animals)
            {
                var line = $"{animal.Species.PadRight(10)}{animal.Name.PadRight(20)}{animal.Age}";
                line = line.PadRight(ClientRectangle.Width, ' ');

                var fc = ConsoleColor.Gray;
                var bc = ConsoleColor.Black;

                if (index == _selectedIndex)
                    bc = ConsoleColor.Blue;

                ConsoleHelper.Draw(ClientRectangle.X, y, line, fc, bc);

                if (++y >= ClientRectangle.Bottom)
                    break;

                ++index;
            }
        }

        public override void OnInputKey(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.UpArrow)
            {
                if (_selectedIndex > 0)
                {
                    _selectedIndex--;
                    Invalidate();
                }
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                if (_selectedIndex + 1 < _database.Animals.Count())
                {
                    _selectedIndex++;
                    Invalidate();
                }
            }
        }
    }
}