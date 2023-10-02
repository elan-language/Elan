namespace Test.ParserTests;

[TestClass]
public class OOP {
    private const string file = "file";

    [TestMethod]
    public void HappyCase1() {
        var code = @"
class Foo

    property name as String
    property p2 as Int
    property p3 as Foo

    constructor(p2 Int)
        name = ""anon""
        self.p2 = p2
    end constructor

    procedure setName(name String)
        self.name = name
    end procedure

    function getName() as String
        return name
    end function

end class
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void HappyCaseImmutable() {
        var code = @"
immutable class Foo

    property name as String
    property p2 as Int
    property p3 as Foo

    constructor (name String)
        self.name = name
    end constructor

    function getName() as String
        return name
    end function

end class
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void HappyCaseAbstract() {
        var code = @"
abstract class Foo

    property name as String

    function getName() as String

    procedure setName(name String)

end class
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void HappyCaseInheritance() {
        var code = @"
abstract class Bar

    property name as String

    procedure setName(name String)

    function getName() as String

end class

class Foo inherits Bar

    property name as String
    property p2 as Int
    property p3 as Foo

    procedure setName(name String)
        self.name = name
    end procedure

    function getName() as String
        return name
    end function

end class
";
        AssertParsesForRule(code, file);
    }

    #region Syntactic errors

    [TestMethod]
    public void Fail_ImmutableWithProcedure() {
        var code = @"
immutable class Foo

    property name as String

    procedure setName(name String)
        prop name = param name
    end procedure

end class
";
        AssertDoesNotParseForRule(code, file, "line 6:4 no viable alternative");
    }

    [TestMethod]
    public void AbstractWithConstructor() {
        var code = @"
abstract class Foo
    constructor
        name = ""anon""
    end constructor

end class
";
        AssertDoesNotParseForRule(code, file, "line 3:4 no viable alternative at input");
    }

    [TestMethod, Ignore]
    public void Fail_ConcreteWith2Constructors() {
        var code = @"
 class Foo

    property name as String

    constructor
        name = ""anon""
    end constructor

    constructor(name String)
        self.name = name
    end constructor

end class
";
        AssertDoesNotParseForRule(code, file, "");
    }

    [TestMethod]
    public void Fail_AbstractWithConcreteMethod() {
        var code = @"
abstract class Foo
    property name as String

    procedure setName(name String)
        self.name = name
    end procedure
end class
";
        AssertDoesNotParseForRule(code, file, "line 6:8 no viable alternative");
    }

    [TestMethod]
    public void Fail_AbstractWithAConstant() {
        var code = @"
abstract class Foo

    constant k = 5.995
    
end class
";
        AssertDoesNotParseForRule(code, file, "line 4:4 no viable alternative");
    }

    [TestMethod]
    public void InheritanceFromConcreteClass() {
        var code = @"
class Bar

    property name as String

end class

class Foo inherits Bar

    property name as String
    property p2 as Int
    property p3 as Foo

    procedure setName(name String)
        self.name = name
    end procedure

    function getName() as String
        return name
    end function

end class
";
        AssertDoesNotParseForRule(code, file, ""); //TODO
    }

    #endregion

    #region Non-syntactic errors - pending implementation further layers of compile

    #endregion
}