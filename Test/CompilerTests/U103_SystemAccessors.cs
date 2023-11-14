﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class U103_SystemAccessors
{
    #region Passes

    [TestMethod]
    public void Pass_accessorAsOrdinaryFunction() {
        var code = @"
main
 var d =  readFromNetwork(""www.foo.com"") 
 print d
end main

system readFromNetwork(url String) as String
  return ""data""
end system
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static string readFromNetwork(string url) {

    return @$""data"";
  }
}

public static class Program {
  private static void Main(string[] args) {
    var d = readFromNetwork(@$""www.foo.com"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(d));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "data\r\n");
    }

    [TestMethod]
    public void Pass_accessorCanBeCalledWithinAProcedure()
    {
        var code = @"

main
 call myProc()
end main

procedure myProc()
  var d = readFromNetwork(""www.foo.com"") 
  print d
end procedure

system readFromNetwork(url String) as String
  return ""data""
end system
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void myProc() {
    var d = readFromNetwork(@$""www.foo.com"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(d));
  }
  public static string readFromNetwork(string url) {

    return @$""data"";
  }
}

public static class Program {
  private static void Main(string[] args) {
    myProc();
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "data\r\n");
    }

    [TestMethod]
    public void Pass_accessorCanCallSystemCallsAndProcedures()
    {
        var code = @"
main
  var d = readFromNetwork(""www.foo.com"")
  print d
end main

system readFromNetwork(url String) as String
  var r = random()
  print ""checking""
  return ""data""
end system
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static string readFromNetwork(string url) {
    var r = random();
    System.Console.WriteLine(StandardLibrary.Functions.asString(@$""checking""));
    return @$""data"";
  }
}

public static class Program {
  private static void Main(string[] args) {
    var d = readFromNetwork(@$""www.foo.com"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(d));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "checking\r\ndata\r\n");
    }


    [TestMethod]
    public void Pass_accessorCanCallOtherAccessor()
    {
        var code = @"
main
  var d = readFromNetwork(""www.foo.com"") 
  print d
end main

system readFromNetwork(url String) as String
  var result = """"
  var b = check(url)
  if  b then
    set result to ""OK data""
  end if 
  return result
end system

system check(url String) as Bool
  return true
end system
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static string readFromNetwork(string url) {
    var result = @$"""";
    var b = check(url);
    if (b) {
      result = @$""OK data"";
    }
    return result;
  }
  public static bool check(string url) {

    return true;
  }
}

public static class Program {
  private static void Main(string[] args) {
    var d = readFromNetwork(@$""www.foo.com"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(d));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "OK data\r\n");
    }


    #endregion

    #region Fails
    [TestMethod]
    public void Fail_accessorCannotBeCalledWithinAFunction()
    {
        var code = @"
main
end main

function myFunc() as String
  return readFromNetwork(""url"")
end function

system readFromNetwork(url String) as String
  return ""data""
end system
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot have system call in function");
    }

    [TestMethod]
   public void Fail_accessorCannotBeCalledWithinAnExpression()
    {
        var code = @"
main
  var d =  readFromNetwork(""www.foo.com"") + ""."" 
  print d 
end main
system readFromNetwork(url String) as String
  return ""data""
end system
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot use a system call in an expression");
    }


    [TestMethod]
    public void Fail_accessorCannotBeCalledWithinAnExpression2()
    {
        var code = @"
main
 print readFromNetwork(""www.foo.com"") 
end main

system readFromNetwork(url String) as String
  return ""data""
end system
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot use a print in an expression");
    }


    #endregion
}