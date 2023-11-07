﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T35_Enumerations {
    #region Passes

    [TestMethod]
    public void Pass_PrintValues() {
        var code = @"
main
 printLine(Fruit.apple)
 printLine(Fruit.orange)
 printLine(Fruit.pear)
end main

enumeration Fruit
    apple, orange, pear
end enumeration
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public enum Fruit {
    apple,
    orange,
    pear,
  }
}

public static class Program {
  private static void Main(string[] args) {
    printLine(Fruit.apple);
    printLine(Fruit.orange);
    printLine(Fruit.pear);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "apple\r\norange\r\npear\r\n");
    }

    [TestMethod]
    public void Pass_useInVariable() {
        var code = @"
main
 var x = Fruit.apple
 x = Fruit.pear
 printLine(x)
end main

enumeration Fruit
    apple, orange, pear
end enumeration
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public enum Fruit {
    apple,
    orange,
    pear,
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = Fruit.apple;
    x = Fruit.pear;
    printLine(x);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "pear\r\n");
    }

    [TestMethod]
    public void Pass_equality() {
        var code = @"
main
    var x = Fruit.apple
    printLine(x is Fruit.apple)
    printLine(x is Fruit.pear)
end main

enumeration Fruit
    apple, orange, pear
end enumeration
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public enum Fruit {
    apple,
    orange,
    pear,
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = Fruit.apple;
    printLine(x == Fruit.apple);
    printLine(x == Fruit.pear);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_SwitchCaseOnEnumeration() {
        var code = @"
main
      var f = Fruit.orange
      switch f
        case Fruit.apple
            printLine('a')
        case Fruit.orange
            printLine('o')
        case Fruit.pear
            printLine('p')
        default
      end switch
end main

enumeration Fruit
    apple, orange, pear
end enumeration
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public enum Fruit {
    apple,
    orange,
    pear,
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = Fruit.orange;
    switch (f) {
      case Fruit.apple:
        printLine('a');
        break;
      case Fruit.orange:
        printLine('o');
        break;
      case Fruit.pear:
        printLine('p');
        break;
      default:
        
        break;
    }
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "o\r\n");
    }

    [TestMethod]
    public void Pass_coercionToString() {
        var code = @"
main
  var a = ""Eat more "" + Fruit.apple + ""s!""
  printLine(a)
end main

enumeration Fruit
    apple, orange, pear
end enumeration
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public enum Fruit {
    apple,
    orange,
    pear,
  }
}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""Eat more "" + Fruit.apple + @$""s!"";
    printLine(a);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Eat more apples!\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_InvalidTypeName() {
        var code = @"
main
end main

enumeration fruit
    apple, orange, pear
end enumeration
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_InvalidValueName() {
        var code = @"
main
end main

enumeration Fruit
    apple, Orange, pear
end enumeration
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_AssigningIntsToValues() {
        var code = @"
main
end main

enumeration Fruit
    apple = 1, orange = 2, pear = 3
end enumeration
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_coercionToInt() {
        var code = @"
main
 var a = 1
 a = Fruit.apple
end main

enumeration Fruit
    apple, orange, pear
end enumeration
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    #endregion
}