using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T73_Lambdas {
    #region Passes

    [TestMethod]
    public void Pass_PassAsParam() {
       var code = @"# Elanv0.1 Parsed FFFF
main
  call printModified(4, lambda x -> x * 3)
end main

procedure printModified(i Int, f (Int -> Int))
  print f(i)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printModified(int i, Func<int, int> f) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(f(i)));
  }
}

public static class Program {
  private static void Main(string[] args) {
    Globals.printModified(4, (x) => x * 3);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }

    [TestMethod]
    public void Pass_TupleArg() {
       var code = @"# Elanv0.1 Parsed FFFF
main
  call printModified((4, 5), lambda t -> t.first())
end main

procedure printModified(i (Int, Int), f ((Int, Int) -> Int))
  print f(i)
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printModified((int, int) i, Func<(int, int), int> f) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(f(i)));
  }
}

public static class Program {
  private static void Main(string[] args) {
    Globals.printModified((4, 5), (t) => StandardLibrary.Functions.first(t));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "4\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_PassLambdaWithWrongTypes() {
       var code = @"# Elanv0.1 Parsed FFFF
main
  call printModified(4, lambda x -> x.asString())
end main

procedure printModified(i Int, f (Int -> Int))
  print f(i)
end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_InvokeLambdaWithWrongType() {
       var code = @"# Elanv0.1 Parsed FFFF
main
  call printModified(""4"", lambda x -> x * 3)
end main

procedure printModified(s String, f (Int -> Int))
  print f(i)
end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_AssignALambdaToAVariable() {
       var code = @"# Elanv0.1 Parsed FFFF
main
  var l = lambda x -> x * 5
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_ReturnALambda() {
       var code = @"# Elanv0.1 Parsed FFFF
main
end main

function getFunc() as (Int -> Int)
  return lambda x -> x * 5
end function

";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}