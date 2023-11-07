using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T16_SwitchCase {
    #region Passes

    [TestMethod]
    public void Pass_Minimal() {
        var code = @"
main
  for i = 1 to 3
      switch i
        case 1
            printLine('a')
        case 2
            printLine('b')
        case 3
            printLine('c')
        default
      end switch
  end for
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    for (var i = 1; i <= 3; i = i + 1) {
      switch (i) {
        case 1:
          printLine('a');
          break;
        case 2:
          printLine('b');
          break;
        case 3:
          printLine('c');
          break;
        default:
          
          break;
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
        AssertObjectCodeExecutes(compileData, "a\r\nb\r\nc\r\n");
    }

    [TestMethod]
    public void Pass_BracketsIgnored() {
        var code = @"
main
  for i = 1 to 3
      switch (i)
        case 1
            printLine('a')
        case 2
            printLine('b')
        case 3
            printLine('c')
        default
      end switch
  end for
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    for (var i = 1; i <= 3; i = i + 1) {
      switch ((i)) {
        case 1:
          printLine('a');
          break;
        case 2:
          printLine('b');
          break;
        case 3:
          printLine('c');
          break;
        default:
          
          break;
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
        AssertObjectCodeExecutes(compileData, "a\r\nb\r\nc\r\n");
    }

    [TestMethod]
    public void Pass_DefaultIsUsed() {
        var code = @"
main
  for i = 1 to 3
      switch i
        case 1
            printLine('a')
        default
            printLine('b')      
      end switch
  end for
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    for (var i = 1; i <= 3; i = i + 1) {
      switch (i) {
        case 1:
          printLine('a');
          break;
        default:
          printLine('b');
          break;
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
        AssertObjectCodeExecutes(compileData, "a\r\nb\r\nb\r\n");
    }

    [TestMethod]
    public void Pass_SwitchOnExpression() {
        var code = @"
main
  for i = 1 to 3
      switch i + 1
        case 1
            printLine('a')
        case 2
            printLine('b')
        default
            printLine('c')        
      end switch
  end for
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    for (var i = 1; i <= 3; i = i + 1) {
      switch (i + 1) {
        case 1:
          printLine('a');
          break;
        case 2:
          printLine('b');
          break;
        default:
          printLine('c');
          break;
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
        AssertObjectCodeExecutes(compileData, "b\r\nc\r\nc\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_NoDefault() {
        var code = @"
main
  for i = 1 to 4
      switch i
        case 1
            printLine('a')
        case 2
            printLine('b')
        case 3
            printLine('c')   
      end switch
  end for
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_NoCase() {
        var code = @"
main
  for i = 1 to 4
      switch i
        default
            printLine('a') 
      end switch
  end for
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_IncompatibleCaseType() {
        var code = @"
main
  for i = 1 to 3
      switch i
        case 1
            printLine('a')
        case 2
            printLine('b')
        case 3.1
            printLine('c') 
        default
      end switch
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
    public void Fail_UseOfVariableForCase() {
        var code = @"
main
  var a = 2
  for i = 1 to 3
      switch i
        case 1
            printLine('a')
        case a
            printLine('b')
        case 3
            printLine('c')        
      end switch
  end for
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_UseOfExpression() {
        var code = @"
main
  for i = 1 to 3
      switch i
        case 1
            printLine('a')
        case 1 + 1
            printLine('b')
        case 3
            printLine('c')        
      end switch
  end for
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_CaseAfterDefault() {
        var code = @"
main
  for i = 1 to 3
      switch i
        case 1
            printLine('a')
        default
            printLine('b')
        case 3
            printLine('c')        
      end switch
  end for
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_WithColons() {
        var code = @"
main
  for i = 1 to 4
      switch i
        case 1:
            printLine('a')
        case 2:
            printLine('b')
        case 3:
            printLine('c')        
  end for
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_actionOnSameLineAsCase() {
        var code = @"
main
  for i = 1 to 3
      switch i
        case 1 printLine('a')
        case 2 printLine('b')       
      end switch
  end for
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_missingExpression() {
        var code = @"
main
  for i = 1 to 3
      switch
        case 1 
            printLine('a')
        case 2 
            printLine('b')       
      end switch
  end for
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_caseValueMissing() {
        var code = @"
main
  for i = 1 to 3
      switch
        case
            printLine('a')
        case 2 
            printLine('b')       
      end switch
  end for
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}