using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T34_ConcreteClasses
{
    #region Passes

    [TestMethod]
    public void Pass_Class_SimpleInstantiation_PropertyAccess_Methods() {
        var code = @"#
main
    var x = Foo()
    printLine(x.p1)
    printLine(x.p2)
    printLine(x.p3)
    x.setP2(3)
    printLine(x.p2)
    printLine(x.product())
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int
    property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    function product() as Int
        return p1 * p2
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
        AssertObjectCodeExecutes(compileData, "5\r\n0\r\n\r\n3\r\n15\r\n"); //N.B. String prop should be auto-initialised to "" not null
    }

    [TestMethod]
    public void Pass_ConstructorWithParmAndSelf()
    {
        var code = @"#
main
    var x = Foo(7)
    printLine(x.p1)
end main

class Foo
    constructor(p1 Int)
        self.p1 = p1
    end constructor

    property p1 as Int
    property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    function product() as Int
        return p1 * p2
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
        AssertObjectCodeExecutes(compileData, "7\r\n"); //N.B. String prop should be auto-initialised to "" not null
    }

    [TestMethod]
    public void Pass_PrivatePropertyAndMethod()
    {
        var code = @"#
class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int
    private property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    private function product() as Int
        return p1 * p2
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
    }




        #endregion

        #region Fails
        public void Fail_NoConstructor()
    {
        var code = @"#
class Foo
    property p1 as Int
    property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    function product() as Int
        return p1 * p2
    end function

end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    public void Fail_InitialisePropertyInLine()
    {
        var code = @"#
class Foo
    constructor()
    end constructor

    property p1 as Int
    property p2 as Int = 3
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    function product() as Int
        return p1 * p2
    end function

end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }


    [TestMethod]
    public void Fail_AttemptToModifyAPropertyDirectly()
    {
        var code = @"#
main
    var x = Foo()
    x.p2 = 3
end main

class Foo
    constructor()
    end constructor


    property p1 as Int
    property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    function product() as Int
        return p1 * p2
    end function

end class
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_AttemptToModifyAPropertyInAFunctionMethod()
    {
        var code = @"#
main
    var x = Foo()
    x.p2 = 3
end main

class Foo
    constructor()
    end constructor

    property p1 as Int
    property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    function product() as Int
        p2 = p1 * 4
        return p2
    end function

end class
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_OverloadedConstructor()
    {
        var code = @"#

class Foo
    constructor()
    end constructor

    constructor(val Int)
        p1 = val
    end constructor

    property p1 as Int
    property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    function product() as Int
        p2 = p1 * 4
        return p2
    end function

end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }


    [TestMethod]
    public void Fail_InstantiateWithoutRequiredArgs()
    {
        var code = @"#
main
    var x = Foo()
end main

class Foo
    constructor(p1 Int)
        self.p1 = p1
    end constructor

    property p1 as Int
    property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    function product() as Int
        return p1 * p2
    end function

end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_InstantiateWithWrongArgType()
    {
        var code = @"#
main
    var x = Foo(7.1)
end main

class Foo
    constructor(p1 Int)
        self.p1 = p1
    end constructor

    property p1 as Int
    property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    function product() as Int
        return p1 * p2
    end function

end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_SupplyingArgumentNotSpecified()
    {
        var code = @"#
main
    var x = Foo(7)
end main

class Foo
    constructor()
    end constructor

    property p1 as Int
    property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    function product() as Int
        p2 = p1 * 4
        return p2
    end function

end class
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Pass_InstantiationMissingWith()
    {
        var code = @"#
main
    var x = Foo() {p1 = 9, p2 = 11, p3 = ""Hello""}
    printLine(x.p1)
    printLine(x.p2)
    printLine(x.p3)
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int
    property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    function product() as Int
        return p1 * p2
    end function

end class
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_AttemptToModifyConstructorParam()
    {
        var code = @"#
class Foo
    constructor(a Int)
        a = 3
    end constructor
end class
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Pass_CannotAccessPrivateProperty()
    {
        var code = @"#
main
    var x = Foo()
    var y = x.p2
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int
    private property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    private function product() as Int
        return p1 * p2
    end function

end class
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Pass_CannotAccessPrivateMethod()
    {
        var code = @"#
main
    var x = Foo()
    var y = x.product()
end main

class Foo
    constructor()
        p1 = 5
    end constructor

    property p1 as Int
    private property p2 as Int
    property p3 as String

    procedure setP1(value Int)
        p1 = value
    end procedure
    
    private function product() as Int
        return p1 * p2
    end function

end class
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    #endregion
}