using F4.UserInterface.Interfaces.Buffering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace F4.UserInterface.Buffering
{
    internal class ScreenBuffer : IScreenBuffer
    {
        public ScreenBufferManager Manager { get; }

        public int Width { get; }

        public int Height { get; }

        public ScreenBufferCharacter[] Buffer { get; }

        public bool EnableUnderline { get; set; } = false;

        public ScreenBuffer(ScreenBufferManager manager, int width, int height)
        {
            Manager = manager;
            Width = width;
            Height = height;
            Buffer = new ScreenBufferCharacter[width * height];
            for (int i = 0; i < Buffer.Length; i++)
            {
                Buffer[i] = new ScreenBufferCharacter(' ', ConsoleColor.Gray, ConsoleColor.Black, EnableUnderline);
            }
        }

        public void Clear(char ch, ConsoleColor foreground, ConsoleColor background)
        {
            for (int i = 0; i < Width * Height; i++)
            {
                Buffer[i] = new ScreenBufferCharacter(ch, foreground, background, EnableUnderline);
            }
        }

        public void Clear(char ch, ConsoleColor foreground)
            => Clear(ch, foreground, ConsoleColor.Black);

        public void Clear(char ch)
            => Clear(ch, ConsoleColor.Gray, ConsoleColor.Black);

        public void Clear()
            => Clear(' ', ConsoleColor.Gray, ConsoleColor.Black);

        public void Clear(Rectangle rectangle)
        {
            Debug.Assert(
                rectangle.Left >= 0 &&
                rectangle.Top >= 0 &&
                rectangle.Right <= Width &&
                rectangle.Height <= Height);

            for (int y = rectangle.Top; y < rectangle.Bottom; y++)
            {
                for (int x = rectangle.Left; x < rectangle.Right; x++)
                {
                    Buffer[y * Width + x] = new ScreenBufferCharacter(' ', ConsoleColor.Gray, ConsoleColor.Black, EnableUnderline);
                }
            }
        }

        public void Draw(int x, int y, char ch, ConsoleColor foreground, ConsoleColor background)
        {
            Debug.Assert(x < Width);
            Debug.Assert(y < Height);
            Buffer[y * Width + x] = new ScreenBufferCharacter(ch, foreground, background, EnableUnderline);
        }

        public void Draw(int x, int y, string text, ConsoleColor foreground, ConsoleColor background)
        {
            int len = text.Length;
            if (x + len > Width)
                len = Math.Max(0, Width - x);

            Debug.Assert(x + len <= Width);
            Debug.Assert(y < Height);

            for (int i = 0; i < len; i++)
            {
                Buffer[y * Width + x + i] = new ScreenBufferCharacter(text[i], foreground, background, EnableUnderline);
            }
        }

        public void Draw(int x, int y, char ch, ConsoleColor foreground)
            => Draw(x, y, ch, foreground, ConsoleColor.Black);

        public void Draw(int x, int y, string text, ConsoleColor foreground)
            => Draw(x, y, text, foreground, ConsoleColor.Black);

        public void Draw(int x, int y, char ch)
            => Draw(x, y, ch, ConsoleColor.Gray, ConsoleColor.Black);

        public void Draw(int x, int y, string text)
            => Draw(x, y, text, ConsoleColor.Gray, ConsoleColor.Black);

        public void SetCursorPosition(int x, int y)
            => Manager.SetCursorPosition(x, y);
    }
}
