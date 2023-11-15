﻿using Compiler;
using CSharpLanguageModel;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T95_RepeatedCallToProcedureWithLiteralArgument
{
    [TestInitialize]
    public void TestInit() {
        CodeHelpers.ResetUniqueId();
    }


    #region Passes

    [TestMethod]
    public void Pass_Template() {
        var code = @"
main
  call square(3)
  call square(5)
end main

procedure square(x Int)
  print x * x
end procedure
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.SystemAccessors;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static void square(int x) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(x * x));
  }
}

public static class Program {
  private static void Main(string[] args) {
    square(3);
    square(5);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "9\r\n25\r\n");
    }

    #endregion

    #region Fails

    #endregion
}