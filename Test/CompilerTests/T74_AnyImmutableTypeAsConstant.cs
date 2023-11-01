using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T74_AnyImmutableTypeAsConstant
{
    #region Passes

    [TestMethod]
    public void Pass_String() {
        var code = @"
constant k = ""Apple""

main 
  printLine(k)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public const string k = @$""Apple"";
}

public static class Program {
  private static void Main(string[] args) {
    printLine(k);
  }
}";

        var parseTree = @"(file (constantDef constant k = (literal (literalDataStructure ""Apple""))) (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value k))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Apple\r\n");
    }

    [TestMethod]
    public void Pass_Tuple()
    {
        var code = @"
constant k = (3, ""Apple"")

main 
  printLine(k)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly (int, string) k = (3, @$""Apple"");
}

public static class Program {
  private static void Main(string[] args) {
    printLine(k);
  }
}";

        var parseTree = @"(file (constantDef constant k = (literal (literalDataStructure (literalTuple ( (literal (literalValue 3)) , (literal (literalDataStructure ""Apple"")) ))))) (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value k))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "(3, Apple)\r\n");
    }

    [TestMethod]
    public void Pass_List()
    {
        var code = @"
constant k = {1,2,3}

main 
  printLine(k)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<int> k = new StandardLibrary.ElanList<int>(1, 2, 3);
}

public static class Program {
  private static void Main(string[] args) {
    printLine(k);
  }
}";

        var parseTree = @"(file (constantDef constant k = (literal (literalDataStructure (literalList { (literal (literalValue 1)) , (literal (literalValue 2)) , (literal (literalValue 3)) })))) (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value k))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {1,2,3}\r\n");
    }

    [TestMethod]
    public void Pass_Dictionary()
    {
        var code = @"
constant k = {'a':1, 'b':3, 'c':3}

main 
  printLine(k)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> k = new StandardLibrary.ElanDictionary<char,int>(('a', 1), ('b', 3), ('c', 3));
}

public static class Program {
  private static void Main(string[] args) {
    printLine(k);
  }
}";

        var parseTree = @"(file (constantDef constant k = (literal (literalDataStructure (literalDictionary { (literalKvp (literal (literalValue 'a')) : (literal (literalValue 1))) , (literalKvp (literal (literalValue 'b')) : (literal (literalValue 3))) , (literalKvp (literal (literalValue 'c')) : (literal (literalValue 3))) })))) (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value k))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Dictionary {a:1,b:3,c:3}\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_ImmutableClass()
    {
        var code = @"
constant k = Foo(3)

main 
  printLine(k)
end main

immutable class Foo
    constructor(p1 Int)
        self.p1 = p1 * 2
    end constructor

    property p1 Int

    function asString() -> String
        return ""{p1}""
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
        AssertObjectCodeExecutes(compileData, "6\r\n");
    }


    #endregion

    #region Fails

    [TestMethod, Ignore]
    public void Fail_Array()
    {
        var code = @"
constant k = Array<Int>(3)
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod, Ignore]
    public void Pass_MutableClass()
    {
        var code = @"
constant k = Foo(3)

class Foo
    constructor(p1 Int)
        self.p1 = p1 * 2
    end constructor

    property p1 Int

    function asString() -> String
        return ""{p1}""
    end function
end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertDoesNotCompile(compileData);
    }
    #endregion
}