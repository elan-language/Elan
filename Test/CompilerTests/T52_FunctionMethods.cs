using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T52_FunctionMethods {
    #region Passes

    [TestMethod]
    public void Pass_HappyCase() {
        var code = @"#
main
    var f = Foo()
    print f.times(2)
end main

class Foo
    constructor()
        set p1 to 5
    end constructor

    property p1 Int

    function times(value Int) -> Int
        return p1 * value
    end function

    function asString() -> String
         return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = 5;
    }
    public virtual int p1 { get; set; } = default;
    public virtual int times(int value) {

      return p1 * value;
    }
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    print(f.times(2));
  }
}";

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
    public void Pass_FunctionMethodMayCallOtherClassFunctionViaProperty() {
        var code = @"#
main
    var f = Foo()
    print f.length()
end main

class Foo
    constructor()
        set p1 to Bar()
    end constructor

    property p1 Bar

    function length() -> Int
        return p1.length() + 2
    end function

    function asString() -> String
         return """"
    end function

end class

class Bar
    constructor()
        set p1 to 5
    end constructor

    property p1 Int

    function length() -> Int
        return p1
    end function

    function asString() -> String
         return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = new Bar();
    }
    public virtual Bar p1 { get; set; } = Bar.DefaultInstance;
    public virtual int length() {

      return p1.length() + 2;
    }
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override Bar p1 => Bar.DefaultInstance;

      public override string asString() { return ""default Foo"";  }
    }
  }
  public record class Bar {
    public static Bar DefaultInstance { get; } = new Bar._DefaultBar();

    public Bar() {
      p1 = 5;
    }
    public virtual int p1 { get; set; } = default;
    public virtual int length() {

      return p1;
    }
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultBar : Bar {
      public _DefaultBar() { }
      public override int p1 => default;

      public override string asString() { return ""default Bar"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    print(f.length());
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "7\r\n");
    }

    [TestMethod]
    public void Pass_FunctionMethodMayCallOtherClassFunctionMethod() {
        var code = @"#
main
    var f = Foo()
    var b = Bar()
    print f.times(b)
end main

class Foo
    constructor()
        set p1 to 5
    end constructor

    property p1 Int

    function times(b Bar) -> Int
        return p1PlusOne() * b.p1PlusOne()
    end function

    function p1PlusOne() -> Int
        return p1 +1 
    end function

    function asString() -> String
         return """"
    end function

end class

class Bar
    constructor()
        set p1 to 1
    end constructor

    property p1 Int

    function p1PlusOne() -> Int
        return p1 +1 
    end function

    function asString() -> String
         return """"
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = 5;
    }
    public virtual int p1 { get; set; } = default;
    public virtual int times(Bar b) {

      return p1PlusOne() * b.p1PlusOne();
    }
    public virtual int p1PlusOne() {

      return p1 + 1;
    }
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;

      public override string asString() { return ""default Foo"";  }
    }
  }
  public record class Bar {
    public static Bar DefaultInstance { get; } = new Bar._DefaultBar();

    public Bar() {
      p1 = 1;
    }
    public virtual int p1 { get; set; } = default;
    public virtual int p1PlusOne() {

      return p1 + 1;
    }
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultBar : Bar {
      public _DefaultBar() { }
      public override int p1 => default;

      public override string asString() { return ""default Bar"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    var b = new Bar();
    print(f.times(b));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }

    [TestMethod]
    public void Pass_FunctionMethodNameHidesGlobalFunction() {
        var code = @"#
main
    var f = Foo()
    print f
end main

class Foo
    constructor()
        set p1 to 5
    end constructor

    property p1 Int

    function asString() -> String
         return p1.asString()
    end function

end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {
      p1 = 5;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string asString() {

      return StandardLibrary.Functions.asString(p1);
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public override int p1 => default;

      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var f = new Foo();
    print(f);
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

    #region Fails

    [TestMethod]
    public void Fail_FunctionCannotBeCalledDirectly() {
        var code = @"#
main
    var f = Foo()
    print times(f,2)
end main

class Foo
    constructor()
        set p1 to 5
    end constructor

    property p1 Int

    function times(value Int) -> Int
        return p1 * value
    end function

    function asString() -> String
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
    public void Fail_FunctionMethodCannotMutateProperty() {
        var code = @"#
class Foo
    constructor()
        set p1 to 5
    end constructor

    property p1 Int

    function times(value Int) -> Int
        set p1 to p1 * value
        return p1
    end function

    function asString() -> String
         return """"
    end function

end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot modify param in function");
    }

    [TestMethod, Ignore]
    public void Fail_FunctionMethodCannotCallProcedureMethod() {
        var code = @"#
class Foo
    constructor()
        set p1 to 5
    end constructor

    property p1 Int

    function times(value Int) -> Int
        call setP1(p1 * value)
        return p1
    end function

    procedure setP1(value Int) 
        set p1 to value
    end procedure

    function asString() -> String
         return """"
    end function

end class
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot have procedure call in function");
    }

    #endregion
}