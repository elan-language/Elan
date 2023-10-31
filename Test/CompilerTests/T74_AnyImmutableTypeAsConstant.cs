using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
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

        var objectCode = @"";

        var parseTree = @"*";

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

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "(3,Apple)\r\n");
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

        var objectCode = @"";

        var parseTree = @"*";

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

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Dictionary {'a':1,'b':3,'c':3}\r\n");
    }

    [TestMethod]
    public void Pass_ImmutableClass()
    {
        var code = @"
constant k = Foo(3)

main 
  printLine(k)
end main

immutable class Foo
    constructor(p1 3)
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

    [TestMethod]
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

    [TestMethod]
    public void Pass_MutableClass()
    {
        var code = @"
constant k = Foo(3)

class Foo
    constructor(p1 3)
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