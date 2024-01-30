using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T43_Inheritance {
    [TestInitialize]
    public void TestInit() { }

    #region Passes

    [TestMethod]
    public void Pass_DefineAbstractClassAndInheritFromIt() {
        var code = @"# Elanv0.1 Parsed FFFF
main
    var x set to new Bar()
    var l set to new List<of Foo>() + x
    print x.p1
    print x.p2
    print x.product()
    call x.setP1(4)
    print x.product()
end main

abstract class Foo
    property p1 Int
    property p2 Int

    procedure setP1(v Int)

    function product() as Int
end class

class Bar inherits Foo
    constructor()
        set p1 to 3
        set p2 to 4
    end constructor
    property p1 Int
    property p2 Int

    procedure setP1(p1 Int)
        set property.p1 to p1
    end procedure

    function product() as Int
        return p1 * p2
    end function

    function asString() as String 
        return """"
    end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public interface Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    public int p1 { get; }
    public int p2 { get; }
    public int product();
    public void setP1(int v);
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public int p1 => default;
      public int p2 => default;
      public void setP1(int v) { }      public int product() => default;
      public string asString() { return ""default Foo"";  }
    }
  }
  public record class Bar : Foo {
    public static Bar DefaultInstance { get; } = new Bar._DefaultBar();

    public Bar() {
      p1 = 3;
      p2 = 4;
    }
    public virtual int p1 { get; set; } = default;
    public virtual int p2 { get; set; } = default;
    public virtual int product() {

      return p1 * p2;
    }
    public virtual string asString() {

      return @$"""";
    }
    public virtual void setP1(int p1) {
      this.p1 = p1;
    }
    private record class _DefaultBar : Bar {
      public _DefaultBar() { }
      public override int p1 => default;
      public override int p2 => default;
      public override void setP1(int p1) { }
      public override string asString() { return ""default Bar"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Bar();
    var l = new StandardLibrary.ElanList<Foo>() + x;
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.p1));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.p2));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.product()));
    x.setP1(4);
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.product()));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n4\r\n12\r\n16\r\n"); //N.B. String prop should be auto-initialised to "" not null
    }

    [TestMethod]
    public void Pass_InheritFromMoreThanOneAbstractClass() {
        var code = @"# Elanv0.1 Parsed FFFF
main
    var x set to new Bar()
    var l set to new List<of Foo>() + x
    print x.p1
    print x.p2
    print x.product()
    call x.setP1(4)
    print x.product()
end main

abstract class Foo
    property p1 Int
    property p2 Int
end class

abstract class Yon
    procedure setP1(v Int)
    function product() as Int
end class

class Bar inherits Foo, Yon
    constructor()
        set p1 to 3
        set p2 to 4
    end constructor
    property p1 Int
    property p2 Int

    procedure setP1(p1 Int)
        set property.p1 to p1
    end procedure

    function product() as Int
        return p1 * p2
    end function

    function asString() as String 
        return """"
    end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public interface Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    public int p1 { get; }
    public int p2 { get; }

    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public int p1 => default;
      public int p2 => default;

      public string asString() { return ""default Foo"";  }
    }
  }
  public interface Yon {
    public static Yon DefaultInstance { get; } = new Yon._DefaultYon();

    public int product();
    public void setP1(int v);
    private record class _DefaultYon : Yon {
      public _DefaultYon() { }

      public void setP1(int v) { }      public int product() => default;
      public string asString() { return ""default Yon"";  }
    }
  }
  public record class Bar : Foo, Yon {
    public static Bar DefaultInstance { get; } = new Bar._DefaultBar();

    public Bar() {
      p1 = 3;
      p2 = 4;
    }
    public virtual int p1 { get; set; } = default;
    public virtual int p2 { get; set; } = default;
    public virtual int product() {

      return p1 * p2;
    }
    public virtual string asString() {

      return @$"""";
    }
    public virtual void setP1(int p1) {
      this.p1 = p1;
    }
    private record class _DefaultBar : Bar {
      public _DefaultBar() { }
      public override int p1 => default;
      public override int p2 => default;
      public override void setP1(int p1) { }
      public override string asString() { return ""default Bar"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Bar();
    var l = new StandardLibrary.ElanList<Foo>() + x;
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.p1));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.p2));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.product()));
    x.setP1(4);
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.product()));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n4\r\n12\r\n16\r\n"); //N.B. String prop should be auto-initialised to "" not null
    }

    [TestMethod]
    public void Pass_SuperclassesCanDefineSameMember() {
        var code = @"# Elanv0.1 Parsed FFFF
main
    var x set to new Bar()
    var l set to new List<of Foo>() + x
    print x.p1
    print x.p2
    print x.product()
    call x.setP1(4)
    print x.product()
end main

abstract class Foo
    property p1 Int
    property p2 Int
end class

abstract class Yon
    property p1 Int
    procedure setP1(v Int)
    function product() as Int
end class

class Bar inherits Foo, Yon
    constructor()
        set p1 to 3
        set p2 to 4
    end constructor
    property p1 Int
    property p2 Int

    procedure setP1(p1 Int)
        set property.p1 to p1
    end procedure

    function product() as Int
        return p1 * p2
    end function

    function asString() as String 
        return """"
    end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public interface Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();
    public int p1 { get; }
    public int p2 { get; }

    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }
      public int p1 => default;
      public int p2 => default;

      public string asString() { return ""default Foo"";  }
    }
  }
  public interface Yon {
    public static Yon DefaultInstance { get; } = new Yon._DefaultYon();
    public int p1 { get; }
    public int product();
    public void setP1(int v);
    private record class _DefaultYon : Yon {
      public _DefaultYon() { }
      public int p1 => default;
      public void setP1(int v) { }      public int product() => default;
      public string asString() { return ""default Yon"";  }
    }
  }
  public record class Bar : Foo, Yon {
    public static Bar DefaultInstance { get; } = new Bar._DefaultBar();

    public Bar() {
      p1 = 3;
      p2 = 4;
    }
    public virtual int p1 { get; set; } = default;
    public virtual int p2 { get; set; } = default;
    public virtual int product() {

      return p1 * p2;
    }
    public virtual string asString() {

      return @$"""";
    }
    public virtual void setP1(int p1) {
      this.p1 = p1;
    }
    private record class _DefaultBar : Bar {
      public _DefaultBar() { }
      public override int p1 => default;
      public override int p2 => default;
      public override void setP1(int p1) { }
      public override string asString() { return ""default Bar"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Bar();
    var l = new StandardLibrary.ElanList<Foo>() + x;
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.p1));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.p2));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.product()));
    x.setP1(4);
    System.Console.WriteLine(StandardLibrary.Functions.asString(x.product()));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n4\r\n12\r\n16\r\n"); //N.B. String prop should be auto-initialised to "" not null
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_CannotInheritFromConcreteClass() {
        var code = @"# Elanv0.1 Parsed FFFF
main
    var x set to new Bar()
    var l set to new List<of Foo>() + x
end main

class Foo
    constructor()
    end constructor
    property p1 Int
    property p2 Int
    function asString() as String 
        return """"
    end function
end class

class Bar inherits Foo
    constructor()
        set p1 to 3
        set p2 to 4
    end constructor
    property p1 Int
    property p2 Int

    procedure setP1(p1 Int)
        set property.p1 to p1
    end procedure

    function product() as Int
        return p1 * p2
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
        AssertDoesNotCompile(compileData, "Cannot inherit from concrete class");
    }

    [TestMethod]
    public void Fail_AbstractClassCannotInheritFromConcreteClass() {
        var code = @"# Elanv0.1 Parsed FFFF
main
    var x set to new Bar()
    var l set to new List<of Foo>() + x
end main

class Foo
    constructor()
    end constructor
    property p1 Int
    property p2 Int
    function asString() as String 
        return """"
    end function
end class

abstract class Bar inherits Foo
    property p1 Int
    property p2 Int
end class
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot inherit from concrete class");
    }

    [TestMethod]
    public void Fail_MustImplementAllInheritedMethods() {
        var code = @"# Elanv0.1 Parsed FFFF
main
    var x set to new Bar()
    var l set to new List<of Foo>() + x
end main

abstract class Foo
    property p1 Int
    property p2 Int

    procedure setP1(v Int)

    function product() as Int
end class

class Bar inherits Foo
    constructor()
        set p1 to 3
        set p2 to 4
    end constructor
    property p1 Int
    property p2 Int

    procedure setP1(p1 Int)
        set property.p1 to p1
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
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_ImplementedMethodMustHaveSameSignature() {
        var code = @"# Elanv0.1 Parsed FFFF
main
    var x set to new Bar()
    var l set to new List<of Foo>() + x
end main

abstract class Foo
    property p1 Int
    property p2 Int

    procedure setP1(v Int)

    function product() as Int
end class

class Bar inherits Foo
    constructor()
        set p1 to 3
        set p2 to 4
    end constructor
    property p1 Int
    property p2 Int

    procedure setP1(p1 Int)
        set property.p1 to p1
    end procedure

    function product() as Float
        return p1 * p2
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
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_AbstractClassDefinesMethodBody() {
        var code = @"# Elanv0.1 Parsed FFFF
main
    var x set to new Bar()
    var l set to new List<of Foo>() + x
end main

abstract class Foo
    property p1 Int
    property p2 Int

    procedure setP1(v Int)
        set property.p1 to p1
    end procedure

    function product() as Int
end class
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}