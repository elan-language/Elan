using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] 
public class TT139_RomanNumerals
{
    #region Passes

    [TestMethod]
    public void Pass_asMainOnly() {
        var code = @"
main
    var d set to 1789
    var result set to """"
    while d >= 1000            
        set d to d - 1000
        set result to result + ""M""
    end while
    while d >= 900           
        set d to d - 900
        set result to result + ""CM""
    end while
    while d >= 500            
        set d to d - 500
        set result to result + ""D""
    end while
    while d >= 400           
        set d to d - 400
        set result to result + ""CD""
    end while
    while d >= 100            
        set d to d - 100
        set result to result + ""C""
    end while
    while d >= 90           
        set d to d - 90
        set result to result + ""XC""
    end while
    while d >= 50            
        set d to d - 50
        set result to result + ""L""
    end while
    while d >= 40            
        set d to d - 40
        set result to result + ""XL""
    end while
    while d >= 10           
        set d to d - 10
        set result to result + ""X""
    end while
    while d >= 9          
        set d to d - 9
        set result to result + ""IX""
    end while
    while d >= 5
        set d to d - 5
        set result to result + ""V""
    end while
    while d >= 4
        set d to d - 4
        set result to result + ""IV""
    end while
    while d >= 1
        set d to d - 1
        set result to result + ""I""
    end while
    print result
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var d = 1789;
    var result = @$"""";
    while (d >= 1000) {
      d = d - 1000;
      result = result + @$""M"";
    }
    while (d >= 900) {
      d = d - 900;
      result = result + @$""CM"";
    }
    while (d >= 500) {
      d = d - 500;
      result = result + @$""D"";
    }
    while (d >= 400) {
      d = d - 400;
      result = result + @$""CD"";
    }
    while (d >= 100) {
      d = d - 100;
      result = result + @$""C"";
    }
    while (d >= 90) {
      d = d - 90;
      result = result + @$""XC"";
    }
    while (d >= 50) {
      d = d - 50;
      result = result + @$""L"";
    }
    while (d >= 40) {
      d = d - 40;
      result = result + @$""XL"";
    }
    while (d >= 10) {
      d = d - 10;
      result = result + @$""X"";
    }
    while (d >= 9) {
      d = d - 9;
      result = result + @$""IX"";
    }
    while (d >= 5) {
      d = d - 5;
      result = result + @$""V"";
    }
    while (d >= 4) {
      d = d - 4;
      result = result + @$""IV"";
    }
    while (d >= 1) {
      d = d - 1;
      result = result + @$""I"";
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(result));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MDCCLXXXIX\r\n");
    }

    [TestMethod]
    public void Pass_oneProcedure()
    {
        var code = @"
main
    var d set to 1066
    var result set to """"
    call processSymbol(1000, ""M"", d, result)
    call processSymbol(900, ""CM"", d, result)
    call processSymbol(500, ""D"", d, result)
    call processSymbol(400, ""CD"", d, result)
    call processSymbol(100, ""C"", d, result)
    call processSymbol(90, ""XC"", d, result)
    call processSymbol(50, ""L"", d, result)
    call processSymbol(40, ""XL"", d, result)
    call processSymbol(10, ""X"", d, result)
    call processSymbol(9, ""IX"", d, result)
    call processSymbol(5, ""V"", d, result)
    call processSymbol(4, ""IV"", d, result)
    call processSymbol(1, ""I"", d, result)
    print result
end main

procedure processSymbol(n Int, x String, out d Int, out result String)
    while d >= n
        set d to d - n
        set result to result + x
    end while
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void processSymbol(int n, string x, ref int d, ref string result) {
    while (d >= n) {
      d = d - n;
      result = result + x;
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var d = 1066;
    var result = @$"""";
    Globals.processSymbol(1000, @$""M"", ref d, ref result);
    Globals.processSymbol(900, @$""CM"", ref d, ref result);
    Globals.processSymbol(500, @$""D"", ref d, ref result);
    Globals.processSymbol(400, @$""CD"", ref d, ref result);
    Globals.processSymbol(100, @$""C"", ref d, ref result);
    Globals.processSymbol(90, @$""XC"", ref d, ref result);
    Globals.processSymbol(50, @$""L"", ref d, ref result);
    Globals.processSymbol(40, @$""XL"", ref d, ref result);
    Globals.processSymbol(10, @$""X"", ref d, ref result);
    Globals.processSymbol(9, @$""IX"", ref d, ref result);
    Globals.processSymbol(5, @$""V"", ref d, ref result);
    Globals.processSymbol(4, @$""IV"", ref d, ref result);
    Globals.processSymbol(1, @$""I"", ref d, ref result);
    System.Console.WriteLine(StandardLibrary.Functions.asString(result));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MLXVI\r\n");
    }

    [TestMethod]
    public void Pass_twoProcedures()
    {
        var code = @"
main
    var d set to 1999
    var result set to """"
    call roman(d, result)
    print result
end main

procedure roman(out d Int, out result String) 
    call processSymbol(1000, ""M"", d, result)
    call processSymbol(900, ""CM"", d, result)
    call processSymbol(500, ""D"", d, result)
    call processSymbol(400, ""CD"", d, result)
    call processSymbol(100, ""C"", d, result)
    call processSymbol(90, ""XC"", d, result)
    call processSymbol(50, ""L"", d, result)
    call processSymbol(40, ""XL"", d, result)
    call processSymbol(10, ""X"", d, result)
    call processSymbol(9, ""IX"", d, result)
    call processSymbol(5, ""V"", d, result)
    call processSymbol(4, ""IV"", d, result)
    call processSymbol(1, ""I"", d, result)
end procedure

procedure processSymbol(n Int, x String, out d Int, out result String)
    while d >= n
        set d to d - n
        set result to result + x
    end while
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void roman(ref int d, ref string result) {
    Globals.processSymbol(1000, @$""M"", ref d, ref result);
    Globals.processSymbol(900, @$""CM"", ref d, ref result);
    Globals.processSymbol(500, @$""D"", ref d, ref result);
    Globals.processSymbol(400, @$""CD"", ref d, ref result);
    Globals.processSymbol(100, @$""C"", ref d, ref result);
    Globals.processSymbol(90, @$""XC"", ref d, ref result);
    Globals.processSymbol(50, @$""L"", ref d, ref result);
    Globals.processSymbol(40, @$""XL"", ref d, ref result);
    Globals.processSymbol(10, @$""X"", ref d, ref result);
    Globals.processSymbol(9, @$""IX"", ref d, ref result);
    Globals.processSymbol(5, @$""V"", ref d, ref result);
    Globals.processSymbol(4, @$""IV"", ref d, ref result);
    Globals.processSymbol(1, @$""I"", ref d, ref result);
  }
  public static void processSymbol(int n, string x, ref int d, ref string result) {
    while (d >= n) {
      d = d - n;
      result = result + x;
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var d = 1999;
    var result = @$"""";
    Globals.roman(ref d, ref result);
    System.Console.WriteLine(StandardLibrary.Functions.asString(result));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MCMXCIX\r\n");
    }

    [TestMethod]
    public void Pass_functions()
    {
        var code = @"
main
    var d set to 1999
    print roman(d)
end main

function roman(d Int) as String
    var result set to """"
    var d2 set to d
    set (d2, result) to processSymbol(1000, ""M"", d2, result)
    set (d2, result) to processSymbol(900, ""CM"", d2, result)
    set (d2, result) to processSymbol(900, ""CM"", d2, result)
    set (d2, result) to processSymbol(500, ""D"", d2, result)
    set (d2, result) to processSymbol(400, ""CD"", d2, result)
    set (d2, result) to processSymbol(100, ""C"", d2, result)
    set (d2, result) to processSymbol(90, ""XC"", d2, result)
    set (d2, result) to processSymbol(50, ""L"", d2, result)
    set (d2, result) to processSymbol(40, ""XL"", d2, result)
    set (d2, result) to processSymbol(10, ""X"", d2, result)
    set (d2, result) to processSymbol(9, ""IX"", d2, result)
    set (d2, result) to processSymbol(5, ""V"", d2, result)
    set (d2, result) to processSymbol(4, ""IV"", d2, result)
    set (d2, result) to processSymbol(1, ""I"", d2, result)
    return result
end function

function processSymbol(n Int, x String, d Int, result String) as (Int, String)
    var d2 set to d
    var result2 set to result
    while d2 >= n
        set d2 to d2 - n
        set result2 to result2 + x
    end while
    return (d2, result2)
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static string roman(int d) {
    var result = @$"""";
    var d2 = d;
    (d2, result) = Globals.processSymbol(1000, @$""M"", d2, result);
    (d2, result) = Globals.processSymbol(900, @$""CM"", d2, result);
    (d2, result) = Globals.processSymbol(900, @$""CM"", d2, result);
    (d2, result) = Globals.processSymbol(500, @$""D"", d2, result);
    (d2, result) = Globals.processSymbol(400, @$""CD"", d2, result);
    (d2, result) = Globals.processSymbol(100, @$""C"", d2, result);
    (d2, result) = Globals.processSymbol(90, @$""XC"", d2, result);
    (d2, result) = Globals.processSymbol(50, @$""L"", d2, result);
    (d2, result) = Globals.processSymbol(40, @$""XL"", d2, result);
    (d2, result) = Globals.processSymbol(10, @$""X"", d2, result);
    (d2, result) = Globals.processSymbol(9, @$""IX"", d2, result);
    (d2, result) = Globals.processSymbol(5, @$""V"", d2, result);
    (d2, result) = Globals.processSymbol(4, @$""IV"", d2, result);
    (d2, result) = Globals.processSymbol(1, @$""I"", d2, result);
    return result;
  }
  public static (int, string) processSymbol(int n, string x, int d, string result) {
    var d2 = d;
    var result2 = result;
    while (d2 >= n) {
      d2 = d2 - n;
      result2 = result2 + x;
    }
    return (d2, result2);
  }
}

public static class Program {
  private static void Main(string[] args) {
    var d = 1999;
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.roman(d)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MCMXCIX\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_ListsOfValuesAndSymbols()
    {
        var code = @"
main
    var d set to 1999
    print roman(d)
end main

function roman(d Int) as String
    var result set to """"
    for i from 0 to values.length() - 1
        var value set to values[i]
        while d >= value
            set result to result + symbols[i]
            set d to d - value
        end while
    end for
    return result
end function

constant values set to { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 }
constant symbols set to { ""M"", ""CM"", ""D"", ""CD"", ""C"", ""XC"", ""L"", ""XL"", ""X"", ""IX"", ""V"", ""IV"", ""I""}
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MCMXCIX\r\n");
    }

    [TestMethod]
    public void Pass_ListOfTuples()
    {
        var code = @"
main
    var d set to 1999
    print roman(d)
end main

function roman(d Int) as String
    var result set to """"
    var d2 set to d
    foreach vs in valueSymbols
        var (value, symbol) set to vs
        while d2 >= value
            set result to result + symbol
            set d2 to d2 - value
        end while
    end foreach
    return result
end function

constant valueSymbols set to { (1000,""M""), (900,""CM""), (500,""D""), (400,""CD""), (100,""C""), (90,""XC""), (50,""L""), (40,""XL""), (10,""X""), (9,""IX""), (5,""V""), (4,""IV""), (1,""I"") }
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<(int, string)> valueSymbols = new StandardLibrary.ElanList<(int, string)>((1000, @$""M""), (900, @$""CM""), (500, @$""D""), (400, @$""CD""), (100, @$""C""), (90, @$""XC""), (50, @$""L""), (40, @$""XL""), (10, @$""X""), (9, @$""IX""), (5, @$""V""), (4, @$""IV""), (1, @$""I""));
  public static string roman(int d) {
    var result = @$"""";
    var d2 = d;
    foreach (var vs in valueSymbols) {
      var (value, symbol) = vs;
      while (d2 >= value) {
        result = result + symbol;
        d2 = d2 - value;
      }
    }
    return result;
  }
}

public static class Program {
  private static void Main(string[] args) {
    var d = 1999;
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.roman(d)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MCMXCIX\r\n");
    }

    [TestMethod]
    public void Pass_Recursion()
    {
        var code = @"
main
    var d set to 1999
    print roman(d, valueSymbols)
end main

function roman(d Int, valueSymbols List<of(Int, String)>) as String
    var result set to """"
    var (value, symbol) set to valueSymbols[0]
    if d > 0 then
        if (d >= value) then
           set result to result + symbol + roman(d - value, valueSymbols)
        else
           set result to roman(d, valueSymbols[1..])
        end if
    end if
    return result
end function

constant valueSymbols set to { (1000,""M""), (900,""CM""), (500,""D""), (400,""CD""), (100,""C""), (90,""XC""), (50,""L""), (40,""XL""), (10,""X""), (9,""IX""), (5,""V""), (4,""IV""), (1,""I"") }
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<(int, string)> valueSymbols = new StandardLibrary.ElanList<(int, string)>((1000, @$""M""), (900, @$""CM""), (500, @$""D""), (400, @$""CD""), (100, @$""C""), (90, @$""XC""), (50, @$""L""), (40, @$""XL""), (10, @$""X""), (9, @$""IX""), (5, @$""V""), (4, @$""IV""), (1, @$""I""));
  public static string roman(int d, StandardLibrary.ElanList<(int, string)> valueSymbols) {
    var result = @$"""";
    var (value, symbol) = valueSymbols[0];
    if (d > 0) {
      if ((d >= value)) {
        result = result + symbol + Globals.roman(d - value, valueSymbols);
      }
      else {
        result = Globals.roman(d, valueSymbols[(1)..]);
      }
    }
    return result;
  }
}

public static class Program {
  private static void Main(string[] args) {
    var d = 1999;
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.roman(d, valueSymbols)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MCMXCIX\r\n");
    }

    [TestMethod]
    public void Pass_MostElegant()
    {
        var code = @"
main
    var d set to 1789
    print roman(d)
end main

function roman(d Int) as String
   var ds set to d.asString()
   var rm set to """"
   for i from ds.length() -1 to 0 step -1
		set rm to symbols[i][ds[i].asInt()-48] + rm
   end for
   return rm
end function

constant symbols set to {{"""",""M"",""MM""},{"""",""C"",""CC"",""CCC"",""CD"",""D"",""DC"",""DCC"",""DCCC"",""CX""},{"""","""",""XX"",""XXX"",""XL"",""L"",""LX"",""LXX"",""LXXX"",""XX""},{"""",""I"",""II"",""III"",""IV"",""V"",""VI"",""VII"",""VIII"",""IX""}}
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<StandardLibrary.ElanList<string>> symbols = new StandardLibrary.ElanList<StandardLibrary.ElanList<string>>(new StandardLibrary.ElanList<string>(@$"""", @$""M"", @$""MM""), new StandardLibrary.ElanList<string>(@$"""", @$""C"", @$""CC"", @$""CCC"", @$""CD"", @$""D"", @$""DC"", @$""DCC"", @$""DCCC"", @$""CX""), new StandardLibrary.ElanList<string>(@$"""", @$"""", @$""XX"", @$""XXX"", @$""XL"", @$""L"", @$""LX"", @$""LXX"", @$""LXXX"", @$""XX""), new StandardLibrary.ElanList<string>(@$"""", @$""I"", @$""II"", @$""III"", @$""IV"", @$""V"", @$""VI"", @$""VII"", @$""VIII"", @$""IX""));
  public static string roman(int d) {
    var ds = StandardLibrary.Functions.asString(d);
    var rm = @$"""";
    for (var i = StandardLibrary.Functions.length(ds) - 1; i >= 0; i = i - 1) {
      rm = symbols[i][StandardLibrary.Functions.asInt(ds[i]) - 48] + rm;
    }
    return rm;
  }
}

public static class Program {
  private static void Main(string[] args) {
    var d = 1789;
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.roman(d)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "MDCCLXXXIX\r\n");
    }

    #endregion

    #region Fails

    #endregion
}