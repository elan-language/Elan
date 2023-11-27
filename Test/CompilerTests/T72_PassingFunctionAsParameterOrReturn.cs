﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T72_PassingFunctionAsParameterOrReturn {
    #region Passes

    [TestMethod]
    public void Pass_PassAsParam() {
        var code = @"
main
  call printModified(3, twice)
end main

procedure printModified(i Int, f (Int -> Int))
  print f(i)
end procedure

function twice(x Int) as Int
  return x * 2
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void printModified(int i, Func<int, int> f) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(f(i)));
  }
  public static int twice(int x) {

    return x * 2;
  }
}

public static class Program {
  private static void Main(string[] args) {
    Globals.printModified(3, twice);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "6\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_PassAsParam2() {
        var code = @"
main
  call printIt(""Hello"", 'e', find)
end main

procedure printIt(s String, c Char, f (String, Char -> Int))
  print f(s,c)
end procedure

function find(x String, y Char ) as Int
  return x.indexOf(y)
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
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_ReturnAFunction() {
        var code = @"
main
  print getFunc()(5)
end main

function getFunc() as (Int -> Int)
  return twice
end function

function twice(x Int) as Int
  return x * 2
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
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_FuncAsProperty() {
        var code = @"
main
  var foo = Foo(twice)
  print foo.f(7)
end main

class Foo
  constructor(f (Int -> Int))
    self.f = f
  end constructor

  property f (Int -> Int)

  function asString() as String
    return ""a Foo""
  end function
end class

function twice(x Int) as Int
  return x * 2
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
        AssertObjectCodeExecutes(compileData, "14\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_DefaultValue() {
        var code = @"
main
  var foo = Foo(twice)
  print foo.f
  print foo.f(7)
end main

class Foo
  constructor()
  end constructor

  property f (Int -> Int)

  function asString() as String
    return ""a Foo""
  end function
end class
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Default (Int -> Int)\r\n0\r\n");
    }

    #endregion

    #region Fails

    [TestMethod, Ignore]
    public void Fail_FunctionSignatureDoesntMatch() {
        var code = @"
main
  call printModified(3, power)
end main

procedure printModified(i Int, f (Int -> Int))
  print f(i)
end procedure

function power(x Int, y Int) as Int
  return x ^ y
end function
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "?");
    }

    [TestMethod, Ignore]
    public void Fail_UsingReturnedFuncWithoutArgs() {
        var code = @"
main
  print getFunc()()
end main

function getFunc() as (Int -> Int)
  return twice
end function

function twice(x Int) as Int
  return x * 2
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "?");
    }

    [TestMethod, Ignore]
    public void Fail_PrintingAFunction() {
        var code = @"
main
  print twice
end main

function twice(x Int) as Int
  return x * 2
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "?");
    }

    #endregion
}