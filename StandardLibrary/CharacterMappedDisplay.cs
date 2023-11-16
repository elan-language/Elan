﻿namespace StandardLibrary;

public class CharMap {
    public const char empty = ' ';
    public readonly int height;
    public readonly int width;

    protected int cursorCol;
    protected int cursorRow;

    public CharMap() {
        //TODO specify size in constructor (with default option) and make the console window that size.
        width = Console.WindowWidth;
        height = Console.WindowHeight;
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
        putCharWithColour(col, row, Symbol.block, foregroundColour);
    }

    public void putBlockWithColour(int col, int row, Colour colour) {
        putCharWithColour(col, row, Symbol.block, colour);
    }

    public void putChar(int col, int row, char c) {
        putCharWithColour(col, row, c, foregroundColour);
    }

    public void putCharWithColour(int col, int row, char c, Colour foreground) {
        putCharWithColours(col, row, c, foreground, backgroundColour);
    }

    public virtual void putCharWithColours(int col, int row, char c, Colour foreground, Colour background)
    {
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

    [ElanStandardLibrary]
    public class CharMapBuffered : CharMap
    {
        private readonly Character[,] buffer;

        public CharMapBuffered() => buffer = new Character[width, height];

        public override void putCharWithColours(int col, int row, char c, Colour foreground, Colour background)
        {
            buffer[col, row] = new Character(c, foreground, background);
        }

        /// <summary>
        ///     Clears the console screen and displays the current contents of this buffer
        /// </summary>
        public void display()
        {
            Console.Clear();
            for (var row = 0; row < buffer.GetLength(1); row++)
            {
                for (var col = 0; col < buffer.GetLength(0); col++)
                {
                    var c = buffer[col, row];
                    if (c.foreground != foregroundColour)
                    {
                        foregroundColour = c.foreground;
                        Console.ForegroundColor = (ConsoleColor)foregroundColour;
                    }

                    if (c.background != backgroundColour)
                    {
                        backgroundColour = c.background;
                        Console.BackgroundColor = (ConsoleColor)backgroundColour;
                    }

                    if (c.ch != empty) { }

                    Console.Write(c.ch);
                }
            }
        }
    }


    #region QuarterCharacter
    //public void setQuarterBlock(double col, double row, Colour foreground, Colour background)
    //{
    //    throw new NotImplementedException();
    //    //Get int values for row col.
    //    //Set standard monchrome colours for this char location
    //    //Get existing char & map to binary - or 0 as default
    //    //Get binary value for new quarter
    //    //And with existing
    //    //Set char position with char for the new binary value
    //    // Note that you cannot have quarters of differnet colours within one character position
    //}

    //public void DrawHighResBar(int col, int row, Direction d, double length, Colour foreground, Colour background)
    //{
    //    throw new NotImplementedException();
    //}

    //public void DrawLine(int col, int row, Direction d, int length, Colour foreground, Colour background)
    //{
    //    throw new NotImplementedException();
    //}

    //public void DrawSolid(int col, int row, int width, int height, Colour foreground, Colour background)
    //{
    //    throw new NotImplementedException();
    //}

    //public void DrawThinFrame(int col, int row, int width, int height, Colour foreground, Colour background)
    //{
    //    throw new NotImplementedException();
    //}

    #endregion
}

