﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T18_ThrowAndCatchException {
    #region Passes

    [TestMethod]
    public void Pass_ThrowExceptionInMain() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    throw ""Foo""
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
    throw new StandardLibrary.ElanException(@$""Foo"");
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Unhandled exception. StandardLibrary.ElanException: Foo");
    }

    [TestMethod]
    public void Pass_ThrowExceptionInMainUsingVariableForMessage() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var msg set to ""Foo""
    throw msg
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
    var msg = @$""Foo"";
    throw new StandardLibrary.ElanException(msg);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Unhandled exception. StandardLibrary.ElanException: Foo");
    }

    [TestMethod]
    public void Pass_ThrowExceptionUsingInterpolatedStringForMessage() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var bar set to 1
    throw ""{bar}""
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
    var bar = 1;
    throw new StandardLibrary.ElanException(@$""{bar}"");
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Unhandled exception. StandardLibrary.ElanException: 1");
    }

    [TestMethod]
    public void Pass_ThrowExceptionInProcedure() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
   call foo()
end main

procedure foo()
  throw ""Foo""
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo() {
    throw new StandardLibrary.ElanException(@$""Foo"");
  }
}

public static class Program {
  private static void Main(string[] args) {
    Globals.foo();
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Unhandled exception. StandardLibrary.ElanException: Foo");
    }

    [TestMethod]
    public void Pass_CatchException() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  try
     call foo()
     print ""not caught""
  catch e
    print e
  end try
end main

procedure foo()
  throw ""Foo""
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo() {
    throw new StandardLibrary.ElanException(@$""Foo"");
  }
}

public static class Program {
  private static void Main(string[] args) {
    try {
      Globals.foo();
      System.Console.WriteLine(StandardLibrary.Functions.asString(@$""not caught""));
    }
    catch (Exception _e) {
      var e = new StandardLibrary.ElanException(_e);
      System.Console.WriteLine(StandardLibrary.Functions.asString(e));
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
        AssertObjectCodeExecutes(compileData, "Foo\r\n");
    }

    [TestMethod]
    public void Pass_CatchSystemGeneratedException() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  try
    var x set to 1
    var y set to 0
    var z set to x div y
    print @$""not caught"";
  catch e
    print e
  end try
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
    try {
      var x = 1;
      var y = 0;
      var z = x / y;
      System.Console.WriteLine(StandardLibrary.Functions.asString(@$""not caught""));
    }
    catch (Exception _e) {
      var e = new StandardLibrary.ElanException(_e);
      System.Console.WriteLine(StandardLibrary.Functions.asString(e));
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
        AssertObjectCodeExecutes(compileData, "Attempted to divide by zero.\r\n");
    }

    [TestMethod]
    public void Pass_UseException() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  try
     call foo()
     print ""not caught""
  catch e
    print e.message
  end try
end main

procedure foo()
  throw ""Foo""
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void foo() {
    throw new StandardLibrary.ElanException(@$""Foo"");
  }
}

public static class Program {
  private static void Main(string[] args) {
    try {
      Globals.foo();
      System.Console.WriteLine(StandardLibrary.Functions.asString(@$""not caught""));
    }
    catch (Exception _e) {
      var e = new StandardLibrary.ElanException(_e);
      System.Console.WriteLine(StandardLibrary.Functions.asString(e.message));
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
        AssertObjectCodeExecutes(compileData, "Foo\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_ThrowExceptionInFunction() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
   var s set to foo()
end main

function foo(x String) as String
  throw x
  return x
end function
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertDoesNotCompile(compileData, "Cannot throw exception in function");
    }

    [TestMethod]
    public void Fail_catchMissingVariable() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  try
     call foo()
     print ""not caught""
  catch
    print ""caught""
  end try
end main

procedure foo()
  throw ""Foo""
end procedure
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_UseExpressionForMessage() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var msg set to ""Foo""
    throw msg + bar
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    #endregion
}