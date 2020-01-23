using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace F4.UserInterface.Buffering
{
    internal class Win32Console
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct CHAR_INFO
        {
            public byte Character;
            byte pad;
            public short Attributes;

            public CHAR_INFO(byte character, short attributes)
            {
                Character = character;
                pad = 0;
                Attributes = attributes;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;

            public COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;

            public SMALL_RECT(short left, short top, short right, short bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CONSOLE_FONT_INFOEX
        {
            public int cbSize;
            public int nFont;
            public COORD dwFontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FaceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public short wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_CURSOR_INFO
        {
            public int dwSize;

            [MarshalAs(UnmanagedType.Bool)]
            public bool bVisible;
        }

        public const int STD_OUTPUT_HANDLE = -11;

        public const int GENERIC_READ = unchecked((int)0x80000000);
        public const int GENERIC_WRITE = 0x40000000;

        public const int FILE_SHARE_READ = 0x00000001;

        public const int CONSOLE_TEXTMODE_BUFFER = 1;

        public const short FOREGROUND_BLUE = 0x0001;
        public const short FOREGROUND_GREEN = 0x0002;
        public const short FOREGROUND_RED = 0x0004;
        public const short FOREGROUND_INTENSITY = 0x0008;
        public const short BACKGROUND_BLUE = 0x0010;
        public const short BACKGROUND_GREEN = 0x0020;
        public const short BACKGROUND_RED = 0x0040;
        public const short BACKGROUND_INTENSITY = 0x0080;

        public const short COMMON_LVB_UNDERSCORE = unchecked((short)0x8000);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateConsoleScreenBuffer(
            int dwDesiredAccess,
            int dwShareMode,
            IntPtr lpSecurityAttributes,
            int dwFlags,
            IntPtr lpScreenBufferData);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", EntryPoint = "WriteConsoleOutputA", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WriteConsoleOutput(
            IntPtr hConsoleOutput,
            [MarshalAs(UnmanagedType.LPArray), In]
            CHAR_INFO[] lpBuffer,
            COORD dwBufferSize,
            COORD dwBufferCoord,
            ref SMALL_RECT lpWriteRegion);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetConsoleActiveScreenBuffer(IntPtr hConsoleOutput);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, COORD coord);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCurrentConsoleFontEx(
            IntPtr hConsoleOutput,
            [MarshalAs(UnmanagedType.Bool)]
            bool bMaximumWindow,
            ref CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetConsoleCursorPosition(IntPtr hConsoleOutput, COORD coord);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetConsoleCursorInfo(IntPtr hConsoleOutput, ref CONSOLE_CURSOR_INFO cci);
    }
}
