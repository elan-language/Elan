using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T35_enums {
    #region Passes

    [TestMethod]
    public void Pass_PrintValues() {
        var code = @"
main
 print Fruit.apple
 print Fruit.orange
 print Fruit.pear
end main

enum Fruit
    apple, orange, pear
end enum
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(Fruit.apple));
    System.Console.WriteLine(StandardLibrary.Functions.asString(Fruit.orange));
    System.Console.WriteLine(StandardLibrary.Functions.asString(Fruit.pear));
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
 set x to Fruit.pear
 print x
end main

enum Fruit
    apple, orange, pear
end enum
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
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
    print x is Fruit.apple
    print x is Fruit.pear
end main

enum Fruit
    apple, orange, pear
end enum
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == Fruit.apple));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x == Fruit.pear));
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
    public void Pass_SwitchCaseOnEnum() {
        var code = @"
main
      var f = Fruit.orange
      switch f
        case Fruit.apple
            print 'a'
        case Fruit.orange
            print 'o'
        case Fruit.pear
            print 'p'
        default
      end switch
end main

enum Fruit
    apple, orange, pear
end enum
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
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
        System.Console.WriteLine(StandardLibrary.Functions.asString('a'));
        break;
      case Fruit.orange:
        System.Console.WriteLine(StandardLibrary.Functions.asString('o'));
        break;
      case Fruit.pear:
        System.Console.WriteLine(StandardLibrary.Functions.asString('p'));
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
  print a
end main

enum Fruit
    apple, orange, pear
end enum
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
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
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
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

enum fruit
    apple, orange, pear
end enum
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_InvalidValueName() {
        var code = @"
main
end main

enum Fruit
    apple, Orange, pear
end enum
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_AssigningIntsToValues() {
        var code = @"
main
end main

enum Fruit
    apple = 1, orange = 2, pear = 3
end enum
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_coercionToInt() {
        var code = @"
main
 var a = 1
 set a to Fruit.apple
end main

enum Fruit
    apple, orange, pear
end enum
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