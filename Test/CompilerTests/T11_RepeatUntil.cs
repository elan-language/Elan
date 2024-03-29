﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T11_RepeatUntil {
    [TestMethod]
    public void Pass_minimal() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
   var x set to 0
   repeat
     set x to x + 1
   end repeat when x >= 10
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
    var x = 0;
    do {
      x = x + 1;
    } while (!(x >= 10));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod]
    public void Pass_innerLoop() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
   var t set to 0
   var x set to 0
   repeat
    var y set to 0
       repeat
         set y to y + 1
         set t to t + 1
       end repeat when y > 4
     set x to x + 1
   end repeat when x > 3
   print t
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
    var t = 0;
    var x = 0;
    do {
      var y = 0;
      do {
        y = y + 1;
        t = t + 1;
      } while (!(y > 4));
      x = x + 1;
    } while (!(x > 3));
    System.Console.WriteLine(StandardLibrary.Functions.asString(t));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "20\r\n");
    }

    [TestMethod]
    public void Fail_variableRedeclaredInTest() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var x set to 0
    repeat
      set x to x + 1
   end repeat when var x >= 10
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_variableDefinedInLoop() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
   repeat
     var x set to x + 1
   end repeat when  x >= 10
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertDoesNotCompile(compileData, "Unresolved Symbol Variable x PendingResolveSymbolType { Name = x } 'main': x");
    }

    [TestMethod]
    public void Fail_testPutOnRepeat() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var x set to 0
    repeat x >= 10
      set x to x + 1
    end repeat 
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_noCondition() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var x set to 0
    repeat
      set x to x + 1
    end repeat when 
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_invalidCondition() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var x set to 0
    repeat
      set x to x + 1
    end repeat when >= 10
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }
}