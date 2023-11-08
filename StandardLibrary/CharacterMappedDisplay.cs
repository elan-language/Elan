using System.Collections.Immutable;
using System.Text;
using static StandardLibrary.Constants;

namespace StandardLibrary;

//TODO: Rename  so we have several subclasses of e.g. Display
public class CharMapLive : CharMap {
    public override void putChar(int col, int row, char c, Colour foreground, Colour background) {
        var currentForeground = Console.ForegroundColor;
        var currentBackground = Console.BackgroundColor;
        var (currentX, currentY) = Console.GetCursorPosition();
        var bufx = Console.BufferWidth;
        var bufy = Console.BufferWidth;
        Console.SetCursorPosition(col, row);
        Console.ForegroundColor = (ConsoleColor)(int)foreground;
        Console.BackgroundColor = (ConsoleColor)(int)background;
        Console.Write(c);
        Console.SetCursorPosition(currentX, currentY);
        Console.ForegroundColor = currentForeground;
        Console.BackgroundColor = currentBackground;
    }
}

public class CharMapBuffered : CharMap {
    private readonly Character[,] buffer;

    public CharMapBuffered() => buffer = new Character[width, height];

    //Overload all versions without colours
    public override void putChar(int col, int row, char c, Colour foreground, Colour background) {
        buffer[col, row] = new Character(c, foreground, background);
    }

    public void setQuarterBlock(double col, double row, Colour foreground, Colour background) {
        throw new NotImplementedException();
        //Get int values for row col.
        //Set standard monchrome colours for this char location
        //Get existing char & map to binary - or 0 as default
        //Get binary value for new quarter
        //And with existing
        //Set char position with char for the new binary value
        // Note that you cannot have quarters of differnet colours within one character position
    }

    public void DrawHighResBar(int col, int row, Direction d, double length, Colour foreground, Colour background) {
        throw new NotImplementedException();
    }

    public void DrawLine(int col, int row, Direction d, int length, Colour foreground, Colour background) {
        throw new NotImplementedException();
    }

    public void DrawSolid(int col, int row, int width, int height, Colour foreground, Colour background) {
        throw new NotImplementedException();
    }

    public void DrawThinFrame(int col, int row, int width, int height, Colour foreground, Colour background) {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Clears the console screen and displays the current contents of this buffer
    /// </summary>
    public void display() {
        Console.Clear();
        for (var row = 0; row < buffer.GetLength(1); row++) {
            for (var col = 0; col < buffer.GetLength(0); col++) {
                var c = buffer[col, row];
                if (c.foreground != foregroundColour) {
                    foregroundColour = c.foreground;
                    Console.ForegroundColor = (ConsoleColor)foregroundColour;
                }

                if (c.background != backgroundColour) {
                    backgroundColour = c.background;
                    Console.BackgroundColor = (ConsoleColor)backgroundColour;
                }

                if (c.ch != empty) { }

                Console.Write(c.ch);
            }
        }
    }
}

public abstract class CharMap {
    public const char empty = ' ';
    public readonly int height;

    public readonly int width;

    protected int cursorCol;
    protected int cursorRow;

    public CharMap() {
        //TODO specify size in constructor (with default option) and make the console window that size.
        width = Console.WindowWidth;
        height = Console.WindowHeight;
        Console.OutputEncoding = Encoding.UTF8;
    }

    public Colour foregroundColour { get; set; }
    public Colour backgroundColour { get; set; }

    public void setCursor(int col, int row) {
        cursorCol = col;
        cursorRow = row;
    }

    //Clears all characters and then paints whole screen with background color. Clear at start, after setting background, if you want a different background.
    public void fillBackground() {
        for (var row = 0; row < height; row++) {
            for (var col = 0; col < width; col++) {
                putChar(col, row, empty);
            }
        }
    }

    public void clearAll() {
        fillBackground();
    }

    public void putBlock(int col, int row) {
        putChar(col, row, Symbol.block, foregroundColour);
    }

    public void putBlock(int col, int row, Colour colour) {
        putChar(col, row, Symbol.block, colour);
    }

    public void putChar(int col, int row, char c) {
        putChar(col, row, c, foregroundColour);
    }

    public void putChar(int col, int row, char c, Colour foreground) {
        putChar(col, row, c, foreground, backgroundColour);
    }

    public abstract void putChar(int col, int row, char c, Colour foreground, Colour background);

    public void clear(int col, int row) {
        putChar(col, row, empty);
    }

    protected class Character {
        public Colour background;
        public char ch;
        public Colour foreground;

        public Character(char ch, Colour foreground, Colour background) {
            this.ch = ch;
            this.foreground = foreground;
            this.background = background;
        }

        public override string ToString() => $"{ch} + ({foreground}, {background})";
    }
}

//TODO: This should be turned into another subclass of characterMappedDisplay

public class XYGraph {
    //Character values for different combinations of quarter-blocks, corresponding to binary values 0000 to 1111:
    private static readonly ImmutableList<char> quarterChars = ImmutableList.Create(' ', '\u2596', '\u2597', '\u2584', '\u2598', '\u258c', '\u259a', '\u2599', '\u259D', '\u259e', '\u2590', '\u259f', '\u2580', '\u259b', '\u259c', '\u2588');

    private ImmutableList<(int, int)> points = ImmutableList.Create<(int, int)>();
    private double yIncrementPerPoint;

    private double yMax;

    private double yMin;

    // Configuration properties
    public int offsetCol { get; set; } = 8; //of lower-left char
    public int offsetRow { get; set; } = 27; //of lower-left char

    public int xPoints { get; set; } = 201;
    public int yPoints { get; set; } = 51;

    public double xMin { get; set; } = 0;
    public double xMax { get; set; } = 10;
    private double xIncrementPerPoint => (xMax - xMin) / (xPoints - 1);
    public string xAxisName { get; set; } = "X";
    public string yAxisName { get; set; } = "Y";

    public int originCol => (int)(xMin >= 0 ? offsetCol : offsetCol - xMin / xIncrementPerPoint / 2);
    public int originRow => (int)(yMin >= 0 ? offsetRow : offsetRow + yMin / yIncrementPerPoint / 2);

    public void SetPoint(int x, int y) {
        points = points.Add((x, y));
    }

    public void ClearPoint(int x, int y) {
        points.Remove((x, y), null);
    }

    public void ClearAllPoints() {
        points.Clear();
    }

    public void PlotFunction(Func<double, double> f) {
        var yValues = new double[xPoints];
        for (var i = 0; i < xPoints; i++) {
            var xValue = xMin + i * xIncrementPerPoint;
            yValues[i] = f(xValue);
        }

        yMax = yValues.Max();
        yMin = yValues.Min();
        yIncrementPerPoint = (yMax - yMin) / (yPoints - 1);
        for (var i = 0; i < xPoints; i++) {
            var y = (int)((yValues[i] - yMin) / yIncrementPerPoint);
            SetPoint(i, y);
        }
    }

    public Dictionary<(int, int), char> getCharsRepresentingPoints() {
        var chars = new Dictionary<(int, int), char>();
        foreach (var pixel in points) {
            var x = pixel.Item1 + 1;
            var y = pixel.Item2 + 1;
            var charPos = (x / 2, y / 2);
            var newBit = (x % 2 + 1) * (y % 2 * 3 + 1);
            if (chars.ContainsKey(charPos)) {
                chars[charPos] = charWithNewBitAdded(chars[charPos], newBit);
            }
            else {
                chars[charPos] = quarterChars[newBit];
            }
        }

        return chars;
    }

    private char charWithNewBitAdded(char existing, int newBit) => quarterChars[quarterChars.IndexOf(existing) | newBit];

    #region Draw

    public void Draw() {
        DrawXYAxes();
        DrawAllXYPoints();
    }

    private void DrawXYAxes() //TODO: Add start end values
    {
        Console.SetCursorPosition(originCol, originRow);
        Console.Write(Symbol.cross);
        for (var c = offsetCol + 1; c <= offsetCol + xPoints / 2; c++) {
            Console.SetCursorPosition(c, originRow);
            Console.Write(Symbol.line_H);
        }

        Console.SetCursorPosition(offsetCol + xPoints / 2 + 1, originRow);
        Console.Write(" " + xAxisName);

        for (var r = offsetRow - 1; r >= offsetRow - yPoints / 2; r--) {
            Console.SetCursorPosition(originCol, r);
            Console.Write(Symbol.line_V);
        }

        Console.SetCursorPosition(originCol - 3, offsetRow - yPoints / 2 - 1);
        Console.Write(yAxisName);
    }

    private void DrawAllXYPoints() {
        var chars = getCharsRepresentingPoints();
        foreach (var key in chars.Keys) {
            Console.SetCursorPosition(key.Item1 + offsetCol, offsetRow - key.Item2);
            Console.Write(chars[key]);
        }
    }

    #endregion
}

// A test file for XYGraph
//public static void Main()
//{
//    var g = new XYGraph() { xMin = 0, xMax = Math.PI * 2 };
//    g.PlotFunction(Math.Sin);
//    g.PlotFunction(Math.Cos);
//    g.Draw();
//    readKey();
//}