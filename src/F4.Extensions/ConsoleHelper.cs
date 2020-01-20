using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.Extensions
{
    public static class ConsoleHelper
    {
        public static void Draw(int x, int y, char c, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.SetCursorPosition(x, y);

            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(c);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorVisible = false;
        }

        public static void Draw(int x, int y, string text, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.SetCursorPosition(x, y);

            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorVisible = false;
        }

        public static void Draw(Point location, char c, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
            => Draw(location.X, location.Y, c, foregroundColor, backgroundColor);

        public static void Draw(Point location, string text, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
            => Draw(location.X, location.Y, text, foregroundColor, backgroundColor);
    }
}
