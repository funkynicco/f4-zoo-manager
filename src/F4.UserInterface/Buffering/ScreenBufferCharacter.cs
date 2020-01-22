using System;
using System.Collections.Generic;
using System.Text;

namespace F4.UserInterface.Buffering
{
    internal struct ScreenBufferCharacter
    {
        public char Character { get; set; }

        public ConsoleColor Foreground { get; set; }

        public ConsoleColor Background { get; set; }

        public ScreenBufferCharacter(char character)
        {
            Character = character;
            Foreground = ConsoleColor.Gray;
            Background = ConsoleColor.Black;
        }

        public ScreenBufferCharacter(char character, ConsoleColor foreground)
        {
            Character = character;
            Foreground = foreground;
            Background = ConsoleColor.Black;
        }

        public ScreenBufferCharacter(char character, ConsoleColor foreground, ConsoleColor background)
        {
            Character = character;
            Foreground = foreground;
            Background = background;
        }

        public override bool Equals(object obj)
        {
            if (obj is ScreenBufferCharacter)
                return this == (ScreenBufferCharacter)obj;

            return false;
        }

        public override int GetHashCode()
            => Character.GetHashCode() + Foreground.GetHashCode() + Background.GetHashCode();

        public static bool operator ==(ScreenBufferCharacter left, ScreenBufferCharacter right)
        {
            return
                left.Character == right.Character &&
                left.Foreground == right.Foreground &&
                left.Background == right.Background;
        }

        public static bool operator !=(ScreenBufferCharacter left, ScreenBufferCharacter right)
        {
            return
                left.Character != right.Character ||
                left.Foreground != right.Foreground ||
                left.Background != right.Background;
        }
    }
}
