using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T_8_ForLoop {
    [TestMethod]
    public void Pass_minimal() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var tot set to 0
  for i from 1 to 10
    set tot to tot + i
  end for
  print tot
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
    var tot = 0;
    for (var i = 1; i <= 10; i = i + 1) {
      tot = tot + i;
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(tot));
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
    public void Pass_withStep() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var tot set to 0
  for i from 1 to 10 step 2
    set tot to tot + i
  end for
  print tot
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
    var tot = 0;
    for (var i = 1; i <= 10; i = i + 2) {
      tot = tot + i;
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(tot));
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
    public void Pass_negativeStep() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var tot set to 0
  for i from 10 to 3 step -1
    set tot to tot + i
  end for
  print tot
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
    var tot = 0;
    for (var i = 10; i >= 3; i = i - 1) {
      tot = tot + i;
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(tot));
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

    [TestMethod]
    public void Pass_innerLoop() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var tot set to 0
  for i from 1 to 3
    for j from 1 to 4
      set tot to tot + 1
    end for
  end for
  print tot
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
    var tot = 0;
    for (var i = 1; i <= 3; i = i + 1) {
      for (var j = 1; j <= 4; j = j + 1) {
        tot = tot + 1;
      }
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(tot));
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
    public void Pass_canUseExistingVariablesOfRightType() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var lower set to 1
  var upper set to 10
  var tot set to 0
  for i from lower to upper step 2 
    set tot to tot + i
  end for
  print tot
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
    var lower = 1;
    var upper = 10;
    var tot = 0;
    for (var i = lower; i <= upper; i = i + 2) {
      tot = tot + i;
    }
    System.Console.WriteLine(StandardLibrary.Functions.asString(tot));
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
    public void Fail_useOfFloat() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var tot set to 0
  for i from 1.5 to 10
    set tot to tot + i
  end for
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
    public void Fail_modifyingCounter() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var tot set to 0
  for i from 1 to 10
    set i to 10
  end for
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot modify control variable");
    }

    public void Fail_missingEnd() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var tot set to 0
  for i from 1 to 3
    for j from 1 to 4
      set tot to tot + 1
    end for
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_nextVariable() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var tot set to 0
  for i from 1 to 10
    set tot to tot + i
  next i
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_break() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var tot set to 0
  for i from 1 to 10
    set tot to tot + i
    break
  next i
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_continue() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var tot set to 0
  for i from 1 to 10
    set tot to tot + i
    continue
  next i
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
}