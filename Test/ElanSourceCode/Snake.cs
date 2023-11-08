using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals
{
    public record class Game
    {
        public static Game DefaultInstance { get; } = new Game._DefaultGame();
        private Game() { }
        public Game(int width, int height, Direction startingDirection)
        {
            this.width = width;
            this.height = height;
            var halfW = width / 2;
            var halfH = height / 2;
            var centreW = halfW;
            if ((halfW % 2) != 0)
            {
                centreW = halfW + 1;
            }
            var centreH = halfH;
            if ((halfH % 2) != 0)
            {
                centreH = halfH + 1;
            }
            snake = new Snake(centreW, centreH, startingDirection);
            setNewApplePosition();
        }
        protected virtual int width { get; set; } = default;
        protected virtual int height { get; set; } = default;
        protected virtual Snake snake { get; set; } = Snake.DefaultInstance;
        public virtual Square apple { get; set; } = Square.DefaultInstance;
        public virtual bool hasHitEdge()
        {
            var x = snake.head.x;
            var y = snake.head.y;
            return x < 0 || y < 0 || x == width || y == height;
        }
        public virtual int getScore()
        {

            return snake.numberOfSegments() - 2;
        }
        public virtual string asString()
        {

            return @$"Game";
        }
        public virtual void clockTick(ref Direction d, ref bool @continue)
        {
            snake.advanceHeadOneSquare(ref d);
            if (snake.head == apple)
            {
                setNewApplePosition();
            }
            else
            {
                snake.advanceTailOneSquare();
            }
            @continue = !hasHitEdge() && !snake.hasHitItself();
        }
        public virtual void setNewApplePosition()
        {
            var sq = new Square(0, 0);
            var collision = false;
            do
            {
                var ranW = random(Compiler.WrapperFunctions.FloatDiv((width - 2), 2));
                var ranH = random(Compiler.WrapperFunctions.FloatDiv((height - 2), 2)) * 2 - 2;
                sq = new Square(ranW * 2, ranH);
                collision = snake.bodyCovers(sq);
            } while (!(!collision));
            apple = sq;
        }
        private record class _DefaultGame : Game
        {
            public _DefaultGame() { }
            protected override int width => default;
            protected override int height => default;
            protected override Snake snake => Snake.DefaultInstance;
            public override Square apple => Square.DefaultInstance;
            public override void clockTick(ref Direction d, ref bool @continue) { }
            public override void setNewApplePosition() { }
            public override string asString() { return "default Game"; }
        }
    }
    public record class Snake
    {
        public static Snake DefaultInstance { get; } = new Snake._DefaultSnake();
        private Snake() { }
        public Snake(int x, int y, Direction startingDirection)
        {
            var tail = new Square(x, y);
            body = new StandardLibrary.ElanList<Square>(tail);
            head = tail.getAdjacentSquare(startingDirection);
        }
        protected virtual StandardLibrary.ElanList<Square> body { get; set; } = StandardLibrary.ElanList<Square>.DefaultInstance;
        public virtual Square head { get; set; } = Square.DefaultInstance;
        public virtual Square tail()
        {

            return body[StandardLibrary.Functions.length(body) - 1];
        }
        public virtual bool hasHitItself()
        {

            return bodyCovers(head);
        }
        public virtual bool bodyCovers(Square sq)
        {
            var result = false;
            foreach (var seg in body)
            {
                if ((seg == sq))
                {
                    result = true;
                }
            }
            return result;
        }
        public virtual int numberOfSegments()
        {

            return StandardLibrary.Functions.length(body);
        }
        public virtual string asString()
        {

            return @$"Snake";
        }
        public virtual void advanceHeadOneSquare(ref Direction d)
        {
            body = body + head;
            head = head.getAdjacentSquare(d);
        }
        public virtual void advanceTailOneSquare()
        {
            body = body[(1)..];
        }
        private record class _DefaultSnake : Snake
        {
            public _DefaultSnake() { }
            protected override StandardLibrary.ElanList<Square> body => StandardLibrary.ElanList<Square>.DefaultInstance;
            public override Square head => Square.DefaultInstance;
            public override void advanceHeadOneSquare(ref Direction d) { }
            public override void advanceTailOneSquare() { }
            public override string asString() { return "default Snake"; }
        }
    }
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

    }
}
