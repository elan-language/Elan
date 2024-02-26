using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T15_eachLoop {
    #region Passes

    [TestMethod]
    public void Pass_List() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to {7,8,9}
    var n set to 0
    each x in a
        set n to n + x
    end each
    print n
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
    var a = new StandardLibrary.ElanList<int>(7, 8, 9);
    var n = 0;
    foreach (var x in a) {
      n = n + x;
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(n));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "24\r\n");
    }

    [TestMethod]
    public void Pass_Array() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to {7,8,9}.asArray()
    var n set to 0
    each x in a
        set n to n + x
    end each
    print n
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
    var a = StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<int>(7, 8, 9));
    var n = 0;
    foreach (var x in a) {
      n = n + x;
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(n));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "24\r\n");
    }

    [TestMethod]
    public void Pass_string() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to ""hello""
    each x in a
        print x
    end each
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
    var a = @$""hello"";
    foreach (var x in a) {
      System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    }
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "h\r\ne\r\nl\r\nl\r\no\r\n");
    }

    [TestMethod]
    public void Pass_doubleLoop() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    each x in ""12""
        each y in ""34""
            print ""{x}{y }""
        end each
    end each
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
    foreach (var x in @$""12"") {
      foreach (var y in @$""34"") {
        System.Console.WriteLine(StandardLibrary.Functions.asString(@$""{x}{y }""));
      }
    }
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "13\r\n14\r\n23\r\n24\r\n");
    }

    [TestMethod]
    public void Pass_functionProvidingList() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    each x in fruit()
        print x
    end each
end main

function fruit() as List<of String>
  return {""apple"",""orange"", ""pear""}
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static StandardLibrary.ElanList<string> fruit() {

    return new StandardLibrary.ElanList<string>(@$""apple"", @$""orange"", @$""pear"");
  }
}

public static class Program {
  private static void Main(string[] args) {
    foreach (var x in Globals.fruit()) {
      System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    }
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
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
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to {7,8,9}
    var x set to ""hello"";
    each x in a
       print x
    end each
    print x
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
    var a = new StandardLibrary.ElanList<int>(7, 8, 9);
    var x = @$""hello"";
    foreach (var x in a) {
      System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_NoEndeach() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  var a set to ""hello""
  each x in a
   print x
  end for
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_applyToANonIterable() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var y set to 10
    each x in y
       print x
    end each
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_CannotAlterTheIterableWithinLoop() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  var a set to {1,2,3,4,5}
  each x in a
    set a to a + x
  end each
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot modify control variable");
    }

    #endregion
}