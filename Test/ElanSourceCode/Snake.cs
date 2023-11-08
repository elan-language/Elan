using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;
using StandardLibrary;

public static partial class Globals
{
    public record class Square
    {
        public static Square DefaultInstance { get; } = new Square._DefaultSquare();
        private Square() { }
        public Square(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public virtual int x { get; set; } = default;
        public virtual int y { get; set; } = default;
        public virtual Square getAdjacentSquare(Direction d)
        {
            var newX = x;
            var newY = y;
            switch (d)
            {
                case Direction.left:
                    newX = newX - 2;
                    break;
                case Direction.right:
                    newX = newX + 2;
                    break;
                case Direction.up:
                    newY = newY - 1;
                    break;
                case Direction.down:
                    newY = newY + 1;
                    break;
                default:

                    break;
            }
            return new Square(newX, newY);
        }
        public virtual string asString()
        {

            return @$"{x},{y}";
        }
        private record class _DefaultSquare : Square
        {
            public _DefaultSquare() { }
            public override int x => default;
            public override int y => default;

            public override string asString() { return "default Square"; }
        }
    }
}

public static class Program
{
    private static void Main(string[] args)
    {
        var sq1 = new Square(3, 4);
        printLine(sq1);
        var sq2 = new Square(3, 4);
        printLine(sq1 == sq2);
        printLine(sq1.getAdjacentSquare(Direction.up));
        printLine(sq1.getAdjacentSquare(Direction.down));
        printLine(sq1.getAdjacentSquare(Direction.left));
        printLine(sq1.getAdjacentSquare(Direction.right));
    }
}
