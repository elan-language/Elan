using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T34_ConcreteClasses {
    #region Passes

    [TestMethod]
    public void Pass_Class_SimpleInstantiation_PropertyAccess_Methods() {
        var code = @"#
main
    var x = Foo()
    print x.p1
    print x.p2
    print x.asString()
end main

class Foo
    constructor()
        set p1 to 5
    end constructor
    property p1 Int

    property p2 String

    function asString() -> String
         return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = 5;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo();
    print(x.p1);
    print(x.p2);
    print(x.asString());
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5\r\n\r\n\r\n"); //N.B. Important that String prop should be auto-initialised to "" not null
    }

    [TestMethod]
    public void Pass_ConstructorWithParm() {
        var code = @"#
main
    var x = Foo(7, ""Apple"")
    print x.p1
    print x.p2
end main

class Foo
    constructor(p_1 Int, p_2 String)
        set p1 to p_1
        set p2 to p_2
    end constructor

    property p1 Int
    property p2 String
    function asString() -> String
        return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    private Foo() {}
    public Foo(int p_1, string p_2) {
      p1 = p_1;
      p2 = p_2;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string p2 { get; set; } = """";
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override string p2 => """";

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Foo(7, @$""Apple"");
    print(x.p1);
    print(x.p2);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "7\r\nApple\r\n"); //N.B. String prop should be auto-initialised to "" not null
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_NoConstructor() {
        var code = @"#
class Foo

    property p1 Int
    property p2 String
   
    function asString() -> String
        return """"
    end function

end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_InitialisePropertyInLine() {
        var code = @"#
class Foo

    property p1 Int = 3
    property p2 String
   
    function asString() -> String
        return """"
    end function

end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_AttemptToModifyAPropertyDirectly() {
        var code = @"#
main
    var x = Foo()
    set x.p1 to 3
end main

class Foo
    constructor()
    end constructor

    property p1 Int

    function asString() -> String
        return """"
    end function
end class
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_OverloadedConstructor() {
        var code = @"#

class Foo
    constructor()
    end constructor

    constructor(val Int)
        set p1 to val
    end constructor

    property p1 Int

    function asString() -> String
        return """"
    end function

end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_InstantiateWithoutRequiredArgs() {
        var code = @"#
main
    var x = Foo()
end main

class Foo
    constructor(val Int)
        set p1 to val
    end constructor

    property p1 Int

    function asString() -> String
        return """"
    end function

end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_InstantiateWithWrongArgType() {
        var code = @"#
main
    var x = Foo(7.1)
end main

class Foo
    constructor(val Int)
        set p1 to val
    end constructor

    property p1 Int

    function asString() -> String
        return """"
    end function

end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_SupplyingArgumentNotSpecified() {
        var code = @"#
main
    var x = Foo(7)
end main

class Foo
    constructor()
    end constructor

    property p1 Int

    function asString() -> String
        return """"
    end function

end class
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    #endregion
}