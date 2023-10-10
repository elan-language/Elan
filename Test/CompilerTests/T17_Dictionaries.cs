using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T17_Dictionaries
{
    #region Passes

    [TestMethod]
    public void Pass_LiteralConstantAndAccessingElement() {
        var code = @"#
constant a = {'a':1, 'b':3, 'z':10}
main
  printLine(a)
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
        AssertObjectCodeExecutes(compileData, "{'a':1, 'b':3, 'z':10}\r\n");
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

        var objectCode = @"";

        var parseTree = @"";
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

        var objectCode = @"";

        var parseTree = @"";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "{'a', 'b', 'z'}\r\n");
    }

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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
        AssertObjectCodeExecutes(compileData, "{'a':1, 'b':3, 'z':10}\r\n{'a':1,'z':10}\r\n");
    }


    #endregion

    #region Fails
    [TestMethod]
    public void Fail_RepeatedKey()
    {
        var code = @"#
constant a = {'a':1, 'b':3, 'a':10}
main
  printLine(a)
end main
";

        var objectCode = @"";

        var parseTree = @"";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
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

        var objectCode = @"";

        var parseTree = @"";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
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

        var objectCode = @"";

        var parseTree = @"";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
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


    [TestMethod]
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


    [TestMethod]
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

    [TestMethod]
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