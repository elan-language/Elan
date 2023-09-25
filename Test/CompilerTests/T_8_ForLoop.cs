﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;


[TestClass, Ignore]
public class T_8_ForLoop
{
    [TestMethod]
    public void Pass_minimal()
    {
        var code = @"
main
  var tot = 0
  for i = 1 to 10
    tot = tot + i
  end for
  printLine(tot)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var tot = 0;
    for (var i = 1; i <= 10; i = i + 1) {
        tot = tot + i;
    }
    printLine(tot)
  }
}"; // could be n++ when there is no step specified, whichever is easier

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "55\r\n");
    }

    [TestMethod]
    public void Pass_withStep()
    {
        var code = @"
main
  var tot = 0
  for i = 1 to 10 step 2
    tot = tot + i
  end for
  printLine(tot)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var tot = 0;
    for (var i = 1; i <= 10; i = i + 2) {
        tot = tot + i;
    }
    printLine(tot)
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "25\r\n");
    }

    public void Pass_negativeStep()
    {
        var code = @"
main
  var tot = 0
  for i = 10 to 3 step -1
    tot = tot + i
  end for
  printLine(tot)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var tot = 0;
    for (var i = 10; i >= 3; i = i - 1) {
        tot = tot + i;
    }
    printLine(tot)
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "52\r\n");
    }

    public void Pass_innerLoop()
    {
        var code = @"
main
  var tot = 0
  for i = 1 to 3
    for j = 1 to 4
      tot = tot + 1
    end for
  end for
  printLine(tot)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var tot = 0;
    for (var i = 1; i <= 3; i = i + 1) {
        for (var j = 1; j <= 4; j = j + 1) {
            tot = tot + i;
        }
    }
    printLine(tot)
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
    public void Pass_canUseExistingVariablesOfRightType()
    {
        var code = @"
main
  var i = 4
  var lower = 1
  var upper = 10
  var inc = 2
  var tot = 0
  for i = lower to upper step inc 
    tot = tot + i
  end for
  printLine(tot)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var i = 4;
    var lower = 1;
    var upper = 10;
    var inc = 2;
    var tot = 0;
    for (var i = lower; i <= upper; i = i + inc) {
        tot = tot + i;
    }
    printLine(tot)
  }
}"; 

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "25\r\n");
    }

    [TestMethod]
    public void Fail_useOfFloat()
    {
        var code = @"
main
  var tot = 0
  for i = 1.5 to 10
    tot = tot + i
  end for
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_modifyingCounter()
    {
        var code = @"
main
  var tot = 0
  for i = 1 to 10
    i = 10
  end for
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    public void Fail_missingEnd()
    {
        var code = @"
main
  var tot = 0
  for i = 1 to 3
    for j = 1 to 4
      tot = tot + 1
    end for
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_useExistingVariableOfWrongType()
    {
        var code = @"
main
  var i = 4.1
  var tot = 0
  for i = 1 to 10
    tot = tot + i
  end for
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_next()
    {
        var code = @"
main
  var tot = 0
  for i = 1 to 10
    tot = tot + i
  next
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_nextVariable()
    {
        var code = @"
main
  var tot = 0
  for i = 1 to 10
    tot = tot + i
  next i
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_break()
    {
        var code = @"
main
  var tot = 0
  for i = 1 to 10
    tot = tot + i
    break
  next i
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_continue()
    {
        var code = @"
main
  var tot = 0
  for i = 1 to 10
    tot = tot + i
    continue
  next i
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }


}