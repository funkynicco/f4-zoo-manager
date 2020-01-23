using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.UserInterface.Interfaces.Buffering
{
    public interface IScreenBuffer
    {
        bool EnableUnderline { get; set; }

        void Clear(char ch, ConsoleColor foreground, ConsoleColor background);
        
        void Clear(char ch, ConsoleColor foreground);
        
        void Clear(char ch);
        
        void Clear();

        void Clear(Rectangle rectangle);

        void Draw(int x, int y, char ch, ConsoleColor foreground, ConsoleColor background);
        
        void Draw(int x, int y, string text, ConsoleColor foreground, ConsoleColor background);
        
        void Draw(int x, int y, char ch, ConsoleColor foreground);
        
        void Draw(int x, int y, string text, ConsoleColor foreground);
        
        void Draw(int x, int y, char ch);
        
        void Draw(int x, int y, string text);

        void SetCursorPosition(int x, int y);
    }
}
