﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T52_FunctionMethods
{
    #region Passes

    [TestMethod]
    public void Pass_HappyCase()
    {
        var code = @"#
main
    var f = Foo()
    printLine(f.times(2))
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    function times(value Int) 
        return p1 * value
    end function

    function asString() as String
         return """"
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
        AssertObjectCodeExecutes(compileData, "10\r\n");
    }

    [TestMethod]
    public void Pass_FunctionMethodMayCallOtherFunctionMethod()
    {
        var code = @"#
main
    var f = Foo()
    printLine(f.times(2))
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    function times(value Int) 
        return p1PlusOne * value
    end function

    function p1PlusOne() 
        return p1 +1 
    end function

    function asString() as String
         return """"
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
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }


    #endregion

    #region Fails

    public void Fail_FunctionCannotBeCalledDirectly()
    {
        var code = @"#
main
    var f = Foo()
    printLine(times(f,2))
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    function times(value Int) 
        return p1 * value
    end function

    function asString() as String
         return """"
    end function

end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    public void Fail_FunctionMethodCannotMutateProperty()
    {
        var code = @"#
class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    function times(value Int) 
        p1 = p1 * value
        return p1
    end function

    function asString() as String
         return """"
    end function

end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    public void Fail_FunctionMethodCannotCallProcedureMethod()
    {
        var code = @"#
class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int

    function times(value Int) 
        setP1(p1 * value)
        return p1
    end function

    procedure setP1(value Int) 
        p1 = value
    end procedure

    function asString() as String
         return """"
    end function

end class
";

       var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }
    #endregion
}