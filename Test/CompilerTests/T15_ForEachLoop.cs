using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T15_ForEachLoop {
    #region Passes

    [TestMethod]
    public void Pass_List() {
        var code = @"
main
    var a = {7,8,9}
    var n = 0
    foreach x in a
        set n to n + x
    end foreach
    print n
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new StandardLibrary.ElanList<int>(7, 8, 9);
    var n = 0;
    foreach (var x in a) {
      n = n + x;
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(n));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "24\r\n");
    }

    [TestMethod]
    public void Pass_Array() {
        var code = @"
main
    var a = {7,8,9}.asArray()
    var n = 0
    foreach x in a
        set n to n + x
    end foreach
    print n
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<int>(7, 8, 9));
    var n = 0;
    foreach (var x in a) {
      n = n + x;
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(n));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "24\r\n");
    }

    [TestMethod]
    public void Pass_string() {
        var code = @"
main
    var a = ""hello""
    foreach x in a
        print x
    end foreach
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = @$""hello"";
    foreach (var x in a) {
      System.Console.WriteLine(StandardLibrary.Functions.asString(x));
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
        AssertObjectCodeExecutes(compileData, "h\r\ne\r\nl\r\nl\r\no\r\n");
    }

    [TestMethod]
    public void Pass_doubleLoop() {
        var code = @"
main
    foreach x in ""12""
        foreach y in ""34""
            print ""{x}{y }""
        end foreach
    end foreach
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    foreach (var x in @$""12"") {
      foreach (var y in @$""34"") {
        System.Console.WriteLine(StandardLibrary.Functions.asString(@$""{x}{y }""));
      }
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
        AssertObjectCodeExecutes(compileData, "13\r\n14\r\n23\r\n24\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_functionProvidingList()
    {
        var code = @"
main
    foreach x in fruit()
        print x
    end foreach
end main

function fruit() as List<String>
  return {""apple"",""orange"", ""pear""}
end function
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "apple\r\norange\r\npear\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_variableIsScoped() {
        var code = @"
main
    var a = {7,8,9}
    var x = ""hello"";
    foreach x in a
       print x
    end foreach
    print x
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new StandardLibrary.ElanList<int>(7, 8, 9);
    var x = @$""hello"";
    foreach (var x in a) {
      System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_NoEndForeach() {
        var code = @"
main
  var a = ""hello""
  foreach x in a
   print x
  end for
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_applyToANonIterable() {
        var code = @"
main
    var y = 10
    foreach x in y
       print x
    end foreach
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_CannotAlterTheIterableWithinLoop() {
        var code = @"
main
  var a ={1,2,3,4,5}
  foreach x in a
    set a to a + x
  end foreach
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot modify control variable");
    }

    #endregion
}