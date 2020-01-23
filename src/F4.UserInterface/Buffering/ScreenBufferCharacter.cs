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

        public bool Underline { get; set; }

        public ScreenBufferCharacter(
            char character,
            ConsoleColor foreground = ConsoleColor.Gray,
            ConsoleColor background = ConsoleColor.Black,
            bool underline = false)
        {
            Character = character;
            Foreground = foreground;
            Background = background;
            Underline = underline;
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
                left.Background == right.Background &&
                left.Underline == right.Underline;
        }

        public static bool operator !=(ScreenBufferCharacter left, ScreenBufferCharacter right)
        {
            return
                left.Character != right.Character ||
                left.Foreground != right.Foreground ||
                left.Background != right.Background ||
                left.Underline != right.Underline;
        }
    }
}
