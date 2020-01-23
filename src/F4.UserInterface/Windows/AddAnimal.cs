using F4.UserInterface.Interfaces.Buffering;
using F4.UserInterface.Interfaces.Windows;
using F4.Zoo;
using F4.Zoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace F4.UserInterface.Windows
{
    internal class AddAnimal : Window, IAddAnimal
    {
        class TextBoxComponent // temporary placement for testing structure
        {
            public string Title { get; }

            public StringBuilder Value { get; } = new StringBuilder();

            public int MaxLength { get; }

            public TextBoxComponent(string title, int maxLength)
            {
                Title = title;
                MaxLength = maxLength;
            }

            public void Draw(IScreenBuffer screenBuffer, Point location)
            {
                screenBuffer.Draw(location.X, location.Y, Title, ConsoleColor.Magenta);

                screenBuffer.EnableUnderline = true;

                screenBuffer.Draw(
                    location.X,
                    location.Y + 1,
                    Value.ToString().PadRight(MaxLength, ' '),
                    ConsoleColor.Gray);

                screenBuffer.EnableUnderline = false;
            }
        }

        private readonly IZooDatabase _database;
        private readonly IRandomizer _randomizer;

        private readonly List<TextBoxComponent> _components = new List<TextBoxComponent>();
        private int _focusedComponentIndex = 0;

        private readonly TextBoxComponent _speciesComponent;
        private readonly TextBoxComponent _nameComponent;
        private readonly TextBoxComponent _ageComponent;

        public AddAnimal(IConsoleManagerInternal manager, IZooDatabase database, IRandomizer randomizer) :
            base(manager)
        {
            _database = database;
            _randomizer = randomizer;
            Title = "Add animal";

            _components.Add(_speciesComponent = new TextBoxComponent("Species", 28));
            _components.Add(_nameComponent = new TextBoxComponent("Name", 28));
            _components.Add(_ageComponent = new TextBoxComponent("Age", 28));
        }

        public override void UpdateWindowRect()
        {
            var rect = new Rectangle(0, 0, 30, 10);
            CenterRectangle(ref rect);
            WindowRectangle = rect;
        }

        public override void Draw(IScreenBuffer screenBuffer)
        {
            base.Draw(screenBuffer);

            var y = ClientRectangle.Y;
            foreach (var component in _components)
            {
                component.Draw(screenBuffer, new Point(ClientRectangle.X, y));

                if (component == _components[_focusedComponentIndex])
                    screenBuffer.Draw(ClientRectangle.X + component.Value.Length, y + 1, ' ', ConsoleColor.Gray, ConsoleColor.Gray);

                y += 3;
            }
        }

        public override void OnInputKey(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                Close();
                return;
            }

            if (key.Key == ConsoleKey.Tab)
            {
                // select next "component"
                ++_focusedComponentIndex;
                if (_focusedComponentIndex >= _components.Count)
                    _focusedComponentIndex = 0;

                Invalidate();
                return;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                var component = _components[_focusedComponentIndex];
                if (component.Value.Length > 0)
                {
                    component.Value.Remove(component.Value.Length - 1, 1);
                    Invalidate();
                }

                return;
            }

            if (key.Key == ConsoleKey.Enter)
            {
                if (_speciesComponent.Value.Length == 0)
                {
                    ConsoleManager.CreateWindow<IMessage>().SetText("Species must be set.");
                    return;
                }

                if (_nameComponent.Value.Length == 0)
                {
                    ConsoleManager.CreateWindow<IMessage>().SetText("Name must be set.");
                    return;
                }

                TimeSpan? age = null;
                if (_ageComponent.Value.Length != 0)
                {
                    if (!TryParseAge(_ageComponent.Value.ToString(), out TimeSpan tsAge))
                    {
                        ConsoleManager.CreateWindow<IMessage>().SetText("Age is not valid.");
                        return;
                    }

                    age = tsAge;
                }

                if (age == null)
                    age = TimeSpan.FromDays(_randomizer.Next(1, 365 * 10)); // up to 10 years old random age

                try
                {
                    _database.Create(_speciesComponent.Value.ToString(), _nameComponent.Value.ToString(), age);
                }
                catch (AnimalTypeNotFoundException)
                {
                    ConsoleManager.CreateWindow<IMessage>().SetText("Species not found.");
                    return;
                }

                Close();
                return;
            }

            if (IsValidTextCharacter(key.KeyChar))
            {
                var component = _components[_focusedComponentIndex];
                if (component.Value.Length < component.MaxLength)
                {
                    component.Value.Append(key.KeyChar);
                    Invalidate();
                }
            }
        }

        private bool TryParseAge(string age, out TimeSpan result)
        {
            result = new TimeSpan();

            Match match;
            if (!(match = Regex.Match(age, @"^(\d+)\s+(day|week|month|year)s?$", RegexOptions.IgnoreCase)).Success)
                return false;

            var value = int.Parse(match.Groups[1].Value);

            switch (match.Groups[2].Value.ToLowerInvariant())
            {
                case "day": result = TimeSpan.FromDays(value); break;
                case "week": result = TimeSpan.FromDays(value * 7); break;
                case "month": result = TimeSpan.FromDays((double)value * (365 / 12)); break;
                case "year": result = TimeSpan.FromDays(value * 365); break;
            }

            return true;
        }

        private bool IsValidTextCharacter(char ch)
        {
            if (ch >= 'a' &&
                ch <= 'z')
                return true;

            if (ch >= 'A' &&
                ch <= 'Z')
                return true;

            if (ch >= '0' &&
                ch <= '9')
                return true;

            switch (ch)
            {
                case ' ':
                case '.':
                case ',':
                case ':':
                case ';':
                case '@':
                case '"':
                case '\'':
                case '!':
                case '?':
                    return true;
            }

            return false;
        }
    }
}
