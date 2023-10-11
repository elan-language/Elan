using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T17_Dictionaries
{
    #region Passes

    [TestMethod]
    public void Pass_LiteralConstantAndPrinting() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(KeyValuePair.Create('a', 1), KeyValuePair.Create('b', 3), KeyValuePair.Create('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    printLine(a);
  }
}";


        var parseTree = @"(file (constantDef constant a = (literal (literalDataStructure (literalDictionary { (literalKvp (literal (literalValue 'a')) : (literal (literalValue 1))) , (literalKvp (literal (literalValue 'b')) : (literal (literalValue 3))) , (literalKvp (literal (literalValue 'z')) : (literal (literalValue 10))) })))) (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value a))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Dictionary {a:1,b:3,z:10}\r\n");
    }

    [TestMethod]
    public void Pass_AccessByKey()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a['z'])
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(KeyValuePair.Create('a', 1), KeyValuePair.Create('b', 3), KeyValuePair.Create('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    printLine(a['z']);
  }
}";

        var parseTree = @"(file (constantDef constant a = (literal (literalDataStructure (literalDictionary { (literalKvp (literal (literalValue 'a')) : (literal (literalValue 1))) , (literalKvp (literal (literalValue 'b')) : (literal (literalValue 3))) , (literalKvp (literal (literalValue 'z')) : (literal (literalValue 10))) })))) (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) (index [ (expression (value (literal (literalValue 'z')))) ]))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod]
    public void Pass_keys()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a.keys())
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(KeyValuePair.Create('a', 1), KeyValuePair.Create('b', 3), KeyValuePair.Create('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    printLine(keys(a));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {a,b,z}\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_hasKey()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a.hasKey('b'))
  printLine(a.hasKey('d'))
end main
";

        var objectCode = @"";

        var parseTree = @"";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\nfalse\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_values()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a.values())
end main
";

        var objectCode = @"";

        var parseTree = @"";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "{1,3,10}\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_Set()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  var b = a.Set('b', 4)
  var c = b.Set('d', 2)
  printLine(a)
  printLine(c)
end mainLine
";

        var objectCode = @"";

        var parseTree = @"";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "{'a':1, 'b':3, 'z':10}\r\n{'a':1,'b':4,'d':2,'z':10}\r\n");
    }

    [TestMethod]
    public void Pass_RemoveEntry()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  var b = a.remove('b')
  printLine(a)
  printLine(b)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(KeyValuePair.Create('a', 1), KeyValuePair.Create('b', 3), KeyValuePair.Create('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    var b = remove(a, 'b');
    printLine(a);
    printLine(b);
  }
}";

        var parseTree = @"(file (constantDef constant a = (literal (literalDataStructure (literalDictionary { (literalKvp (literal (literalValue 'a')) : (literal (literalValue 1))) , (literalKvp (literal (literalValue 'b')) : (literal (literalValue 3))) , (literalKvp (literal (literalValue 'z')) : (literal (literalValue 10))) })))) (main main (statementBlock (varDef var (assignableValue b) = (expression (expression (value a)) . (methodCall remove ( (argumentList (expression (value (literal (literalValue 'b'))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value a))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value b))) ))))) end main) <EOF>)";
        
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Dictionary {a:1,b:3,z:10}\r\nDictionary {a:1,z:10}\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_CreateEmptyDictionary()
    {
        var code = @"#
main
  var a = new Dictionary<String, Int>()
  var b = a.set(""Foo"",1)
  b = set(""Bar"", 3)
  printLine(b)
end main
";

        var objectCode = @"";


        var parseTree = @"";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, @"Dictionary {Foo:1,Bar:3}\r\n"); 
    }
    #endregion

    #region Fails
    [TestMethod, Ignore]
    public void Fail_RepeatedKey()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'a':10}
main
  printLine(a)
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
    public void Fail_InconsistentTypes1()
    {
        var code = @"#
constant a = {'a':1, 'b':3.1, 'z':10}
main
  printLine(a)
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
    public void Fail_InconsistentTypes2()
    {
        var code = @"#
constant a = {'a':1, 'b':3.1, ""Z"":10}
main
  printLine(a)
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
    public void Fail_AccessByInvalidKey()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a['c'])
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanDictionary<char,int> a = new StandardLibrary.ElanDictionary<char,int>(KeyValuePair.Create('a', 1), KeyValuePair.Create('b', 3), KeyValuePair.Create('z', 10));
}

public static class Program {
  private static void Main(string[] args) {
    printLine(a['c']);
  }
}";

        var parseTree = @"(file (constantDef constant a = (literal (literalDataStructure (literalDictionary { (literalKvp (literal (literalValue 'a')) : (literal (literalValue 1))) , (literalKvp (literal (literalValue 'b')) : (literal (literalValue 3))) , (literalKvp (literal (literalValue 'z')) : (literal (literalValue 10))) })))) (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) (index [ (expression (value (literal (literalValue 'c')))) ]))) ))))) end main) <EOF>)";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Unhandled exception. System.Collections.Generic.KeyNotFoundException"); //Messaging saying key does not exist or similar
    }


    [TestMethod, Ignore]
    public void Fail_RemoveInvalidKey()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  var b = a.remove('c')
end mainLine
";

        var objectCode = @"";

        var parseTree = @"";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "!\r\n"); //Messaging saying key does not exist or similar
    }


    [TestMethod, Ignore]
    public void Fail_RemoveInvalidKeyType()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  var b = a.remove(""b"")
end mainLine
";

        var objectCode = @"";

        var parseTree = @"";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod, Ignore]
    public void Fail_SetInvalidKeyType()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  var b = a.Set(""b"", 4)
end mainLine
";

        var objectCode = @"";

        var parseTree = @"";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }
    #endregion
}