using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace F4.UserInterface.Interfaces.Buffering
{
    public interface IScreenBuffer
    {
        public void Clear(char ch, ConsoleColor foreground, ConsoleColor background);
        
        public void Clear(char ch, ConsoleColor foreground);
        
        public void Clear(char ch);
        
        public void Clear();

        public void Clear(Rectangle rectangle);

        public void Draw(int x, int y, char ch, ConsoleColor foreground, ConsoleColor background);
        
        public void Draw(int x, int y, string text, ConsoleColor foreground, ConsoleColor background);
        
        public void Draw(int x, int y, char ch, ConsoleColor foreground);
        
        public void Draw(int x, int y, string text, ConsoleColor foreground);
        
        public void Draw(int x, int y, char ch);
        
        public void Draw(int x, int y, string text);
    }
}
