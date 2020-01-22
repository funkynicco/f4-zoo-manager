using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace F4.UserInterface.Buffering
{
    internal class ScreenBufferManager : IDisposable
    {
        private readonly IntPtr _defaultBuffer;
        private IntPtr _screenBuffer;

        private ScreenBuffer _display;

        public ScreenBuffer Backbuffer { get; private set; }

        public ScreenBufferManager(int width, int height)
        {
            _defaultBuffer = Win32Console.GetStdHandle(Win32Console.STD_OUTPUT_HANDLE);
            Resize(width, height);
        }

        public void Dispose()
        {
            Debug.Assert(Win32Console.SetConsoleActiveScreenBuffer(_defaultBuffer));
            Debug.Assert(Win32Console.CloseHandle(_screenBuffer));
        }

        public Size GetConsoleWindowSize()
        {
            Debug.Assert(Win32Console.GetConsoleScreenBufferInfo(_screenBuffer, out Win32Console.CONSOLE_SCREEN_BUFFER_INFO csbi));
            return new Size(
                csbi.srWindow.Right - csbi.srWindow.Left,
                csbi.srWindow.Bottom - csbi.srWindow.Top);
        }

        public void Resize(int width, int height)
        {
            _display = new ScreenBuffer(width, height);
            Backbuffer = new ScreenBuffer(width, height);

            var newBuffer = Win32Console.CreateConsoleScreenBuffer(
                Win32Console.GENERIC_READ | Win32Console.GENERIC_WRITE,
                Win32Console.FILE_SHARE_READ,
                IntPtr.Zero,
                Win32Console.CONSOLE_TEXTMODE_BUFFER,
                IntPtr.Zero);

            Win32Console.SetConsoleScreenBufferSize(newBuffer, new Win32Console.COORD((short)width, (short)height));

            var cfi = new Win32Console.CONSOLE_FONT_INFOEX();
            cfi.cbSize = Marshal.SizeOf(cfi);
            cfi.nFont = 0;
            cfi.dwFontSize = new Win32Console.COORD(8, 14);
            cfi.FaceName = "Lucida Console";
            cfi.FontFamily = 0;
            cfi.FontWeight = 0;
            Debug.Assert(Win32Console.SetCurrentConsoleFontEx(newBuffer, false, ref cfi));

            Debug.Assert(Win32Console.SetConsoleActiveScreenBuffer(newBuffer));
            if (_screenBuffer != IntPtr.Zero)
                Debug.Assert(Win32Console.CloseHandle(_screenBuffer));
            _screenBuffer = newBuffer;
        }

        private short GetAttributesFromColors(ConsoleColor foreground, ConsoleColor background)
        {
            short attributes = 0;

            switch (foreground)
            {
                case ConsoleColor.DarkBlue: attributes |= Win32Console.FOREGROUND_BLUE; break;
                case ConsoleColor.DarkGreen: attributes |= Win32Console.FOREGROUND_GREEN; break;
                case ConsoleColor.DarkCyan: attributes |= Win32Console.FOREGROUND_GREEN | Win32Console.FOREGROUND_BLUE; break;
                case ConsoleColor.DarkRed: attributes |= Win32Console.FOREGROUND_RED; break;
                case ConsoleColor.DarkMagenta: attributes |= Win32Console.FOREGROUND_RED | Win32Console.FOREGROUND_BLUE; break;
                case ConsoleColor.DarkYellow: attributes |= Win32Console.FOREGROUND_RED | Win32Console.FOREGROUND_GREEN; break;
                case ConsoleColor.Gray: attributes |= Win32Console.FOREGROUND_RED | Win32Console.FOREGROUND_GREEN | Win32Console.FOREGROUND_BLUE; break;
                case ConsoleColor.DarkGray: attributes |= Win32Console.FOREGROUND_RED | Win32Console.FOREGROUND_GREEN | Win32Console.FOREGROUND_BLUE; break;
                case ConsoleColor.Blue: attributes |= Win32Console.FOREGROUND_BLUE | Win32Console.FOREGROUND_INTENSITY; break;
                case ConsoleColor.Green: attributes |= Win32Console.FOREGROUND_GREEN | Win32Console.FOREGROUND_INTENSITY; break;
                case ConsoleColor.Cyan: attributes |= Win32Console.FOREGROUND_GREEN | Win32Console.FOREGROUND_BLUE | Win32Console.FOREGROUND_INTENSITY; break;
                case ConsoleColor.Red: attributes |= Win32Console.FOREGROUND_RED | Win32Console.FOREGROUND_INTENSITY; break;
                case ConsoleColor.Magenta: attributes |= Win32Console.FOREGROUND_RED | Win32Console.FOREGROUND_BLUE | Win32Console.FOREGROUND_INTENSITY; break;
                case ConsoleColor.Yellow: attributes |= Win32Console.FOREGROUND_RED | Win32Console.FOREGROUND_GREEN | Win32Console.FOREGROUND_INTENSITY; break;
                case ConsoleColor.White: attributes |= Win32Console.FOREGROUND_RED | Win32Console.FOREGROUND_GREEN | Win32Console.FOREGROUND_BLUE | Win32Console.FOREGROUND_INTENSITY; break;
            }

            switch (background)
            {
                case ConsoleColor.DarkBlue: attributes |= Win32Console.BACKGROUND_BLUE; break;
                case ConsoleColor.DarkGreen: attributes |= Win32Console.BACKGROUND_GREEN; break;
                case ConsoleColor.DarkCyan: attributes |= Win32Console.BACKGROUND_GREEN | Win32Console.BACKGROUND_BLUE; break;
                case ConsoleColor.DarkRed: attributes |= Win32Console.BACKGROUND_RED; break;
                case ConsoleColor.DarkMagenta: attributes |= Win32Console.BACKGROUND_RED | Win32Console.BACKGROUND_BLUE; break;
                case ConsoleColor.DarkYellow: attributes |= Win32Console.BACKGROUND_RED | Win32Console.BACKGROUND_GREEN; break;
                case ConsoleColor.Gray: attributes |= Win32Console.BACKGROUND_RED | Win32Console.BACKGROUND_GREEN | Win32Console.BACKGROUND_BLUE; break;
                case ConsoleColor.DarkGray: attributes |= Win32Console.BACKGROUND_RED | Win32Console.BACKGROUND_GREEN | Win32Console.BACKGROUND_BLUE; break;
                case ConsoleColor.Blue: attributes |= Win32Console.BACKGROUND_BLUE | Win32Console.BACKGROUND_INTENSITY; break;
                case ConsoleColor.Green: attributes |= Win32Console.BACKGROUND_GREEN | Win32Console.BACKGROUND_INTENSITY; break;
                case ConsoleColor.Cyan: attributes |= Win32Console.BACKGROUND_GREEN | Win32Console.BACKGROUND_BLUE | Win32Console.BACKGROUND_INTENSITY; break;
                case ConsoleColor.Red: attributes |= Win32Console.BACKGROUND_RED | Win32Console.BACKGROUND_INTENSITY; break;
                case ConsoleColor.Magenta: attributes |= Win32Console.BACKGROUND_RED | Win32Console.BACKGROUND_BLUE | Win32Console.BACKGROUND_INTENSITY; break;
                case ConsoleColor.Yellow: attributes |= Win32Console.BACKGROUND_RED | Win32Console.BACKGROUND_GREEN | Win32Console.BACKGROUND_INTENSITY; break;
                case ConsoleColor.White: attributes |= Win32Console.BACKGROUND_RED | Win32Console.BACKGROUND_GREEN | Win32Console.BACKGROUND_BLUE | Win32Console.BACKGROUND_INTENSITY; break;
            }

            return attributes;
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

                        var ci = new Win32Console.CHAR_INFO();
                        ci.Character = (byte)displayChar.Character;
                        ci.Attributes = GetAttributesFromColors(displayChar.Foreground, displayChar.Background);

                        var ci_array = new Win32Console.CHAR_INFO[1] { ci };

                        var rc = new Win32Console.SMALL_RECT((short)x, (short)y, (short)(x + 1), (short)(y + 1));
                        Debug.Assert(Win32Console.WriteConsoleOutput(
                            _screenBuffer,
                            ci_array,
                            new Win32Console.COORD(1, 1), // dwBufferSize - width/height of the 2d ci_array
                            new Win32Console.COORD(), // dwBufferCoord
                            ref rc)); // lpWriteRegion - where to write to
                    }
                }
            }
        }
    }
}
