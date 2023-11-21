using System.Collections.Immutable;

namespace StandardLibrary;

//TODO: This should be turned into another subclass of charMap and
//changed so that it can be drawn in a specified position WITHIN another charMap, say.

[ElanStandardLibrary]
public class XYGraph {
    //Character values for different combinations of quarter-blocks, corresponding to binary values 0000 to 1111:
    private static readonly ImmutableList<char> quarterChars = ImmutableList.Create(' ', '\u2596', '\u2597', '\u2584', '\u2598', '\u258c', '\u259a', '\u2599', '\u259D', '\u259e', '\u2590', '\u259f', '\u2580', '\u259b', '\u259c', '\u2588');

    private ElanList<(int, int)> points = new();
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

    public ElanDictionary<(int, int), char> getCharsRepresentingPoints() {
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

        return new ElanDictionary<(int, int), char>(chars.Select(i => (i.Key, i.Value)).ToArray());
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