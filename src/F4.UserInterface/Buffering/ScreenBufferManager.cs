using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.UserInterface.Buffering
{
    internal class ScreenBufferManager
    {
        private ScreenBuffer _display;

        public ScreenBuffer Backbuffer { get; private set; }

        public ScreenBufferManager(int width, int height)
        {
            Resize(width, height);
        }

        public void Resize(int width, int height)
        {
            _display = new ScreenBuffer(width, height);
            Backbuffer = new ScreenBuffer(width, height);
            Console.Clear();
        }

        public void Commit()
        {
            for (int y = 0; y < _display.Height; y++)
            {
                for (int x = 0; x < _display.Width; x++)
                {
                    var i = y * _display.Width + x;
                    ref ScreenBufferCharacter displayChar = ref _display.Buffer[i];
                    ref ScreenBufferCharacter backbufferChar = ref Backbuffer.Buffer[i];

                    if (displayChar != backbufferChar)
                    {
                        displayChar = backbufferChar;

                        Console.SetCursorPosition(x, y);
                        Console.ForegroundColor = displayChar.Foreground;
                        Console.BackgroundColor = displayChar.Background;
                        Console.Write(displayChar.Character);
                    }
                }
            }
        }
    }
}
