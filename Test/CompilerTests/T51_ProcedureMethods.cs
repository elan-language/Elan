using Compiler;
using CSharpLanguageModel;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T51_ProcedureMethods
{

    [TestInitialize]
    public void TestInit()
    {
        CodeHelpers.ResetUniqueId();
    }

    #region Fails

    [TestMethod]
    public void Fail_ProcedureMethodCannotBeCalledDirectly()
    {
        var code = @"#
main
    var f = Foo()
    call display(f)
end main

class Foo
    constructor()
        set p1 to 5
    end constructor

    property p1 Int

    procedure display()
        print p1
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
        AssertDoesNotCompile(compileData, "Calling unknown method");
    }

    [TestMethod]
    public void Fail_CallUnknownMethodOnInstance()
    {
        var code = @"#
main
    var x = Foo()
    call x.calculate()
end main

class Foo
    constructor()
        set p1 to 5
    end constructor
    property p1 Int

    property p2 String

    function asString() as String
         return """"
    end function

end class
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Calling unknown method");
    }
    #endregion

    #region Passes

    [TestMethod]
    public void Pass_HappyCase()
    {
        var code = @"#
main
    var f = Foo()
    print f.p1
    call f.setP1(7)
    print f.p1
end main

class Foo
    constructor()
        set p1 to 5
    end constructor
    property p1 Int
    procedure setP1(value Int)
        set p1 to value
    end procedure
    function asString() as String
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
    public virtual string asString() {

      return @$"""";
    }
    public virtual void setP1(ref int value) {
      p1 = value;
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override void setP1(ref int value) { }
      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    print(f.p1);
    var _setP1_1_0 = 7;
    f.setP1(ref _setP1_1_0);
    print(f.p1);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5\r\n7\r\n");
    }

    [TestMethod]
    public void Pass_ProcedureCanContainSystemCall()
    {
        var code = @"#
main
    var f = Foo()
    call f.display()
end main

class Foo
    constructor()
        set p1 to 5
    end constructor

    property p1 Int

    procedure display()
        print p1
    end procedure

    function asString() as String
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
    public virtual string asString() {

      return @$"""";
    }
    public virtual void display() {
      print(p1);
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;
      public override void display() { }
      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    f.display();
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "5\r\n");
    }

    #endregion
}