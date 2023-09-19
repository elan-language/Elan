namespace Test.ParserTests;

[TestClass]
public class OOP {
    private const string file = "file";

    [TestMethod]
    public void HappyCase1() {
        var code = @"
class Foo

    property name = """"
    property p2 Int
    property p3 Foo

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

    property name = """"
    property p2 Int
    property p3 Foo

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

    property name String

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

    property name String

    procedure setName(name String)

    function getName() as String

end class

class Foo inherits Bar

    property name = """"
    property p2 Int
    property p3 Foo

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
    public void ImmutableWithProcedure() {
        var code = @"
immutable class Foo

    property name = """"

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

    [TestMethod]
    public void ConcreteWith2Constructors() {
        var code = @"
 class Foo

    property name = ""

    constructor
        name = ""anon""
    end constructor

    constructor(name String)
        self.name = name
    end constructor

end class
";
        AssertDoesNotParseForRule(code, file, "line 7:16 extraneous input 'anon' expecting NL");
    }

    [TestMethod]
    public void AbstractWithConcreteMethod() {
        var code = @"
abstract class Foo
    property name = """"

    procedure setName(name String)
        self.name = name
    end procedure
end class
";
        AssertDoesNotParseForRule(code, file, "line 6:8 no viable alternative");
    }

    [TestMethod]
    public void AbstractWithAConstant() {
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

    property name String

end class

class Foo inherits Bar

    property name = """"
    property p2 Int
    property p3 Foo

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