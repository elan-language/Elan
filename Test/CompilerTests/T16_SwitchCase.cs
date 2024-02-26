using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T16_SwitchCase {
    #region Passes

    [TestMethod]
    public void Pass_Minimal() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 3
      switch i
        case 1
            print 'a'
        case 2
            print 'b'
        case 3
            print 'c'
        default
      end switch
  end for
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
    for (var i = 1; i <= 3; i = i + 1) {
      switch (i) {
        case 1:
          System.Console.WriteLine(StandardLibrary.Functions.asString('a'));
          break;
        case 2:
          System.Console.WriteLine(StandardLibrary.Functions.asString('b'));
          break;
        case 3:
          System.Console.WriteLine(StandardLibrary.Functions.asString('c'));
          break;
        default:
          
          break;
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
        AssertObjectCodeExecutes(compileData, "a\r\nb\r\nc\r\n");
    }

    [TestMethod]
    public void Pass_BracketsIgnored() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 3
      switch (i)
        case 1
            print 'a'
        case 2
            print 'b'
        case 3
            print 'c'
        default
      end switch
  end for
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
    for (var i = 1; i <= 3; i = i + 1) {
      switch ((i)) {
        case 1:
          System.Console.WriteLine(StandardLibrary.Functions.asString('a'));
          break;
        case 2:
          System.Console.WriteLine(StandardLibrary.Functions.asString('b'));
          break;
        case 3:
          System.Console.WriteLine(StandardLibrary.Functions.asString('c'));
          break;
        default:
          
          break;
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
        AssertObjectCodeExecutes(compileData, "a\r\nb\r\nc\r\n");
    }

    [TestMethod]
    public void Pass_DefaultIsUsed() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 3
      switch i
        case 1
            print 'a'
        default
            print 'b'      
      end switch
  end for
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
    for (var i = 1; i <= 3; i = i + 1) {
      switch (i) {
        case 1:
          System.Console.WriteLine(StandardLibrary.Functions.asString('a'));
          break;
        default:
          System.Console.WriteLine(StandardLibrary.Functions.asString('b'));
          break;
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
        AssertObjectCodeExecutes(compileData, "a\r\nb\r\nb\r\n");
    }

    [TestMethod]
    public void Pass_SwitchOnExpression() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 3
      switch i + 1
        case 1
            print 'a'
        case 2
            print 'b'
        default
            print 'c'        
      end switch
  end for
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
    for (var i = 1; i <= 3; i = i + 1) {
      switch (i + 1) {
        case 1:
          System.Console.WriteLine(StandardLibrary.Functions.asString('a'));
          break;
        case 2:
          System.Console.WriteLine(StandardLibrary.Functions.asString('b'));
          break;
        default:
          System.Console.WriteLine(StandardLibrary.Functions.asString('c'));
          break;
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
        AssertObjectCodeExecutes(compileData, "b\r\nc\r\nc\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_NoDefault() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 4
      switch i
        case 1
            print 'a'
        case 2
            print 'b'
        case 3
            print 'c'   
      end switch
  end for
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_NoCase() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 4
      switch i
        default
            print 'a' 
      end switch
  end for
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_IncompatibleCaseType() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 3
      switch i
        case 1
            print 'a'
        case 2
            print 'b'
        case 3.1
            print 'c' 
        default
      end switch
  end for
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
    public void Fail_UseOfVariableForCase() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  var a set to 2
  for i from 1 to 3
      switch i
        case 1
            print 'a'
        case a
            print 'b'
        case 3
            print 'c'        
      end switch
  end for
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_UseOfExpression() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 3
      switch i
        case 1
            print 'a'
        case 1 + 1
            print 'b'
        case 3
            print 'c'        
      end switch
  end for
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_CaseAfterDefault() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 3
      switch i
        case 1
            print 'a'
        default
            print 'b'
        case 3
            print 'c'        
      end switch
  end for
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_WithColons() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 4
      switch i
        case 1:
            print 'a'
        case 2:
            print 'b'
        case 3:
            print 'c'        
  end for
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_actionOnSameLineAsCase() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 3
      switch i
        case 1 System.Console.WriteLine(StandardLibrary.Functions.asString('a'))
        case 2 System.Console.WriteLine(StandardLibrary.Functions.asString('b'))       
      end switch
  end for
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_missingExpression() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 3
      switch
        case 1 
            print 'a'
        case 2 
            print 'b'       
      end switch
  end for
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_caseValueMissing() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
  for i from 1 to 3
      switch
        case
            print 'a'
        case 2 
            print 'b'       
      end switch
  end for
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    #endregion
}