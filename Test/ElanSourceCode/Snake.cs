using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals
{
    public static readonly Colour bodyColour = Colour.green;
    public static readonly Colour appleColour = Colour.red;
    public static readonly StandardLibrary.ElanDictionary<char, Direction> directionByKey = new StandardLibrary.ElanDictionary<char, Direction>(('w', Direction.up), ('s', Direction.down), ('a', Direction.left), ('d', Direction.right));
    public const string welcome = @$"Welcome to the Snake game. 

Use the w,a,s, and d keys to control the direction of the snake. Letting the snake get to any edge will lose you the game, as will letting the snake's head pass over its body. Eating an apple will
cause the snake to grow by 

If you want to re-size the window, please do so now, before starting the game.

Click on this window to get 'focus' (and see a flashing cursor). Then press any key to start..";
    public static void playGame()
    {
        var charMap = new CharMapLive();
        charMap.fillBackground();
        var currentDirection = Direction.right;
        var game = new Game(charMap.width, charMap.height, currentDirection);
        var gameOn = true;
        game.setNewApplePosition();
        while (gameOn)
        {
            var head = game.head();
            var _putBlockWithColour_3_0 = head.x;
            var _putBlockWithColour_3_1 = head.y;
            charMap.putBlockWithColour( _putBlockWithColour_3_0,  _putBlockWithColour_3_1,  bodyColour);
            var _putBlockWithColour_4_0 = head.x + 1;
            var _putBlockWithColour_4_1 = head.y;
            charMap.putBlockWithColour( _putBlockWithColour_4_0,  _putBlockWithColour_4_1,  bodyColour);
            var apple = game.apple;
            var _putBlockWithColour_5_0 = apple.x;
            var _putBlockWithColour_5_1 = apple.y;
            charMap.putBlockWithColour( _putBlockWithColour_5_0,  _putBlockWithColour_5_1,  appleColour);
            var _putBlockWithColour_6_0 = apple.x + 1;
            var _putBlockWithColour_6_1 = apple.y;
            charMap.putBlockWithColour( _putBlockWithColour_6_0,  _putBlockWithColour_6_1,  appleColour);
            var priorTail = game.tail();
            StandardLibrary.Functions.pause(200);
            var pressed = keyHasBeenPressed();
            if (pressed)
            {
                var k = readKey();
                currentDirection = directionByKey[k];
            }
            game.clockTick( currentDirection,  gameOn);
            if (game.tail() != priorTail)
            {
                var _clear_8_0 = priorTail.x;
                var _clear_8_1 = priorTail.y;
                charMap.clear( _clear_8_0,  _clear_8_1);
                var _clear_9_0 = priorTail.x + 1;
                var _clear_9_1 = priorTail.y;
                charMap.clear( _clear_9_0,  _clear_9_1);
            }
        }
        clearKeyBuffer();
        var _setCursor_10_0 = 0;
        var _setCursor_10_1 = 0;
        charMap.setCursor( _setCursor_10_0,  _setCursor_10_1);
        System.Console.WriteLine(StandardLibrary.Functions.asString(@$"Game Over! Score: {game.getScore()}"));
    }
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
        public virtual Square head()
        {

            return snake.head;
        }
        public virtual Square tail()
        {

            return snake.tail();
        }
        public virtual bool hasHitEdge()
        {
            var x = snake.head.x;
            var y = snake.head.y;
            return x < 0 || y < 0 || x == width || y == height;
        }
        public virtual int getScore()
        {

            return snake.length() - 2;
        }
        public virtual string asString()
        {

            return @$"Game";
        }
        public virtual void clockTick( Direction d,  bool @continue)
        {
            snake.advanceHeadOneSquare( d);
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
                var ranW = random((width - 2) / 2);
                var rh = random((height - 2) / 2);
                var ranH = rh * 2 - 2;
                sq = new Square(ranW * 2, ranH);
            } while (!(!snake.bodyCovers(sq)));
            apple = sq;
        }
        private record class _DefaultGame : Game
        {
            public _DefaultGame() { }
            protected override int width => default;
            protected override int height => default;
            protected override Snake snake => Snake.DefaultInstance;
            public override Square apple => Square.DefaultInstance;
            public override void clockTick( Direction d,  bool @continue) { }
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
        public virtual int length()
        {

            return StandardLibrary.Functions.length(body);
        }
        public virtual string asString()
        {

            return @$"Snake";
        }
        public virtual void advanceHeadOneSquare( Direction d)
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
            public override void advanceHeadOneSquare( Direction d) { }
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
        System.Console.WriteLine(StandardLibrary.Functions.asString(welcome));
        var k = readKey();
        var newGame = true;
        while (newGame)
        {
            playGame();
            System.Console.WriteLine(StandardLibrary.Functions.asString(@$"Do you want to play again (y/n)?"));
            var answer = ' ';
            do
            {
                answer = readKey();
            } while (!(answer == 'y' || answer == 'n'));
            if (answer == 'n')
            {
                newGame = false;
            }
        }
    }
}
