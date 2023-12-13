using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T96_Snake {
    #region Passes

    [TestMethod]
    public void Pass_Snake_OOP() {
        var code = ReadCodeFile("snake_OOP.elan");

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,Direction> directionByKey = new StandardLibrary.ElanDictionary<char,Direction>(('w', Direction.up), ('s', Direction.down), ('a', Direction.left), ('d', Direction.right));
  public const string welcome = @$""Welcome to the Snake game. 

Use the w,a,s, and d keys to control the direction of the snake. Letting the snake get to any edge will lose you the game, as will letting the snake's head pass over its body. Eating an apple will
cause the snake to grow by one segment. 

If you want to re-size the window, please do so now, before starting the game.

Press any key to start.."";
  public static void playGame() {
    var charMap = new CharMap();
    charMap.fillBackground();
    var currentDirection = Direction.right;
    var snake = new Snake(charMap.width / 2, charMap.height, currentDirection);
    var gameOn = true;
    while (gameOn) {
      Globals.draw(charMap, snake.head, Colour.green);
      Globals.draw(charMap, snake.apple, Colour.red);
      var priorTail = snake.tail();
      StandardLibrary.Procedures.pause(200);
      var pressed = StandardLibrary.SystemAccessors.keyHasBeenPressed();
      if (pressed) {
        var k = StandardLibrary.SystemAccessors.readKey();
        currentDirection = directionByKey[k];
      }
      snake.clockTick(currentDirection, ref gameOn);
      if (snake.tail() != priorTail) {
        Globals.draw(charMap, priorTail, charMap.backgroundColour);
      }
    }
    charMap.setCursor(0, 0);
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""Game Over! Score: {snake.length() - 2}""));
  }
  public static void draw(CharMap cm, Square sq, Colour colour) {
    var col = sq.x * 2;
    var row = sq.y;
    cm.putBlockWithColour(col, row, colour);
    cm.putBlockWithColour(col + 1, row, colour);
  }
  public record class Snake {
    public static Snake DefaultInstance { get; } = new Snake._DefaultSnake();
    private Snake() {}
    public Snake(int boardWidth, int boardHeight, Direction startingDirection) {
      this.boardWidth = boardWidth;
      this.boardHeight = boardHeight;
      var tail = new Square(boardWidth / 2, boardHeight / 2);
      body = new StandardLibrary.ElanList<Square>(tail);
      head = tail.getAdjacentSquare(startingDirection);
      setNewApplePosition();
    }
    public virtual int boardWidth { get; set; } = default;
    public virtual int boardHeight { get; set; } = default;
    public virtual Square head { get; set; } = Square.DefaultInstance;
    protected virtual StandardLibrary.ElanList<Square> body { get; set; } = StandardLibrary.ElanList<Square>.DefaultInstance;
    public virtual Square apple { get; set; } = Square.DefaultInstance;
    public virtual Square tail() {

      return body[0];
    }
    public virtual int length() {

      return StandardLibrary.Functions.length(body);
    }
    public virtual bool bodyCovers(Square sq) {
      var result = false;
      foreach (var seg in body) {
        if ((seg == sq)) {
          result = true;
        }
      }
      return result;
    }
    public virtual bool hasHitEdge() {

      return head.x < 0 || head.y < 0 || head.x == boardWidth || head.y == boardHeight;
    }
    public virtual string asString() {

      return @$""Snake"";
    }
    public virtual void clockTick(Direction d, ref bool @continue) {
      body = body + head;
      head = head.getAdjacentSquare(d);
      if (head == apple) {
        setNewApplePosition();
      }
      else {
        body = body[(1)..];
      }
      @continue = !hasHitEdge() && !bodyCovers(head);
    }
    public virtual void setNewApplePosition() {
      do {
        var ranW = StandardLibrary.SystemAccessors.random(boardWidth - 1);
        var ranH = StandardLibrary.SystemAccessors.random(boardHeight - 1);
        apple = new Square(ranW, ranH);
      } while (!(!bodyCovers(apple)));
    }
    private record class _DefaultSnake : Snake {
      public _DefaultSnake() { }
      public override int boardWidth => default;
      public override int boardHeight => default;
      public override Square head => Square.DefaultInstance;
      protected override StandardLibrary.ElanList<Square> body => StandardLibrary.ElanList<Square>.DefaultInstance;
      public override Square apple => Square.DefaultInstance;
      public override void clockTick(Direction d, ref bool @continue) { }
      public override void setNewApplePosition() { }
      public override string asString() { return ""default Snake"";  }
    }
  }
  public record class Square {
    public static Square DefaultInstance { get; } = new Square._DefaultSquare();
    private Square() {}
    public Square(int x, int y) {
      this.x = x;
      this.y = y;
    }
    public virtual int x { get; set; } = default;
    public virtual int y { get; set; } = default;
    public virtual Square getAdjacentSquare(Direction d) {
      var newX = x;
      var newY = y;
      switch (d) {
        case Direction.left:
          newX = newX - 1;
          break;
        case Direction.right:
          newX = newX + 1;
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
    public virtual string asString() {

      return @$""{x},{y}"";
    }
    private record class _DefaultSquare : Square {
      public _DefaultSquare() { }
      public override int x => default;
      public override int y => default;

      public override string asString() { return ""default Square"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(welcome));
    var k = StandardLibrary.SystemAccessors.readKey();
    var newGame = true;
    while (newGame) {
      Globals.playGame();
      System.Console.WriteLine(StandardLibrary.Functions.asString(@$""Do you want to play again (y/n)?""));
      var answer = ' ';
      do {
        answer = StandardLibrary.SystemAccessors.readKey();
      } while (!(answer == 'y' || answer == 'n'));
      if (answer == 'n') {
        newGame = false;
      }
    }
  }
}";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
    }

    [TestMethod]
    public void Pass_Snake_PP()
    {
        var code = ReadCodeFile("snake_PP.elan");

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,Direction> directionByKey = new StandardLibrary.ElanDictionary<char,Direction>(('w', Direction.up), ('s', Direction.down), ('a', Direction.left), ('d', Direction.right));
  public const string welcome = @$""Welcome to the Snake game. 

Use the w,a,s, and d keys to control the direction of the snake. Letting the snake get to any edge will lose you the game, as will letting the snake's head pass over its body. Eating an apple will
cause the snake to grow by one segment. 

If you want to re-size the window, please do so now, before starting the game.

Press any key to start.."";
  public static void playGame() {
    var charMap = new CharMap();
    charMap.fillBackground();
    var boardWidth = charMap.width / 2;
    var boardHeight = charMap.height;
    var currentDirection = Direction.right;
    var startTail = (boardWidth / 2, boardHeight / 2);
    var body = new StandardLibrary.ElanList<(int, int)>();
    body = body + startTail;
    var head = Globals.getAdjacentSquare(startTail, currentDirection);
    var apple = (0, 0);
    Globals.setNewApplePosition(body, ref apple, boardWidth, boardHeight);
    var gameOn = true;
    while (gameOn) {
      Globals.draw(charMap, head, Colour.green);
      Globals.draw(charMap, apple, Colour.red);
      var priorTail = Globals.snakeTail(body);
      StandardLibrary.Procedures.pause(200);
      var pressed = StandardLibrary.SystemAccessors.keyHasBeenPressed();
      if (pressed) {
        var k = StandardLibrary.SystemAccessors.readKey();
        currentDirection = directionByKey[k];
      }
      Globals.clockTick(ref body, ref head, ref apple, currentDirection, boardWidth, boardHeight, ref gameOn);
      if (Globals.snakeTail(body) != priorTail) {
        Globals.draw(charMap, priorTail, charMap.backgroundColour);
      }
    }
    charMap.setCursor(0, 0);
    var score = StandardLibrary.Functions.length(body) - 2;
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""Game Over! Score: {score}""));
  }
  public static void draw(CharMap cm, (int, int) sq, Colour colour) {
    var col = sq.Item1 * 2;
    var row = sq.Item2;
    cm.putBlockWithColour(col, row, colour);
    cm.putBlockWithColour(col + 1, row, colour);
  }
  public static void clockTick(ref StandardLibrary.ElanList<(int, int)> body, ref (int, int) head, ref (int, int) apple, Direction d, int boardWidth, int boardHeight, ref bool @continue) {
    body = body + head;
    head = Globals.getAdjacentSquare(head, d);
    if (head == apple) {
      Globals.setNewApplePosition(body, ref apple, boardWidth, boardHeight);
    }
    else {
      body = body[(1)..];
    }
    @continue = !Globals.hasHitEdge(head, boardWidth, boardHeight) && !Globals.bodyCovers(body, head);
  }
  public static void setNewApplePosition(StandardLibrary.ElanList<(int, int)> body, ref (int, int) apple, int boardWidth, int boardHeight) {
    do {
      var ranW = StandardLibrary.SystemAccessors.random(boardWidth - 1);
      var ranH = StandardLibrary.SystemAccessors.random(boardHeight - 1);
      var newPos = (ranW, ranH);
      apple = newPos;
    } while (!(!Globals.bodyCovers(body, apple)));
  }
  public static (int, int) snakeTail(StandardLibrary.ElanList<(int, int)> body) {

    return body[0];
  }
  public static bool bodyCovers(StandardLibrary.ElanList<(int, int)> body, (int, int) sq) {
    var result = false;
    foreach (var seg in body) {
      if ((seg == sq)) {
        result = true;
      }
    }
    return result;
  }
  public static bool hasHitEdge((int, int) head, int boardWidth, int boardHeight) {
    var x = head.Item1;
    var y = head.Item2;
    return x < 0 || y < 0 || x == boardWidth || y == boardHeight;
  }
  public static (int, int) getAdjacentSquare((int, int) sq, Direction d) {
    var newX = sq.Item1;
    var newY = sq.Item2;
    switch (d) {
      case Direction.left:
        newX = newX - 1;
        break;
      case Direction.right:
        newX = newX + 1;
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
    return (newX, newY);
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(welcome));
    var k = StandardLibrary.SystemAccessors.readKey();
    var newGame = true;
    while (newGame) {
      Globals.playGame();
      System.Console.WriteLine(StandardLibrary.Functions.asString(@$""Do you want to play again (y/n)?""));
      var answer = ' ';
      do {
        answer = StandardLibrary.SystemAccessors.readKey();
      } while (!(answer == 'y' || answer == 'n'));
      if (answer == 'n') {
        newGame = false;
      }
    }
  }
}";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
    }
    #endregion

    #region Fails

    #endregion
}