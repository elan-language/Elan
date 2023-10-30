using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T43_Inheritance
{
    #region Passes

    [TestMethod]
    public void Pass_DefineAbstractClassAndInheritFromIt() {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
    printLine(x.p1)
    printLine(x.p2)
    printLine(x.product())
    x.setP1(4)
    printLine(x.product())
end main

abstract class Foo
    property p1 Int
    property p2 Int

    procedure setP1(v Int)

    function product() -> Int
end class

class Bar inherits Foo
    constructor()
        p1 = 3
        p2 = 4
    end constructor
    property p1 Int
    property p2 Int

    procedure setP1(p1 Int)
        self.p1 = p1
    end procedure

    function product() -> Int
        return p1 * p2
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
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public interface Foo {
    public int p1 { get; }
    public int p2 { get; }
    public int product();
    public void setP1(ref int v);
  }
  public class Bar : Foo {
    public static Bar DefaultInstance { get; } = new Bar._DefaultBar();

    public Bar() {
      p1 = 3;
      p2 = 4;
    }
    public int p1 { get; private set; } = default;
    public int p2 { get; private set; } = default;
    public virtual int product() {

      return p1 * p2;
    }
    public virtual string asString() {

      return @$"""";
    }
    public virtual void setP1(ref int p1) {
      this.p1 = p1;
    }
    private class _DefaultBar : Bar {
      public _DefaultBar() { }
      public override void setP1(ref int p1) { }
      public override string asString() { return ""Default Bar"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Bar();
    var l = new StandardLibrary.ElanList<Foo>() + x;
    printLine(x.p1);
    printLine(x.p2);
    printLine(x.product());
    var _setP1_0 = 4;
    x.setP1(ref _setP1_0);
    printLine(x.product());
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Bar) ( )))) (varDef var (assignableValue l) = (expression (expression (newInstance (type (dataStructureType List (genericSpecifier < (type Foo) >))) ( ))) (binaryOp (arithmeticOp +)) (expression (value x)))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p2)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . (methodCall product ( )))) )))) (callStatement (expression (expression (value x)) . (methodCall setP1 ( (argumentList (expression (value (literal (literalValue 4))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . (methodCall product ( )))) ))))) end main) (classDef (abstractClass abstract class Foo (property property p1 (type Int)) (property property p2 (type Int)) procedure (procedureSignature setP1 ( (parameterList (parameter v (type Int))) )) function (functionSignature product ( ) -> (type Int)) end class)) (classDef (mutableClass class Bar (inherits inherits (type Foo)) (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 3))))) (assignment (assignableValue p2) = (expression (value (literal (literalValue 4)))))) end constructor) (property property p1 (type Int)) (property property p2 (type Int)) (procedureDef procedure (procedureSignature setP1 ( (parameterList (parameter p1 (type Int))) )) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (value p1)))) end procedure) (functionDef (functionWithBody function (functionSignature product ( ) -> (type Int)) statementBlock return (expression (expression (value p1)) (binaryOp (arithmeticOp *)) (expression (value p2))) end function)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n4\r\n12\r\n16\r\n"); //N.B. String prop should be auto-initialised to "" not null
    }

    [TestMethod]
    public void Pass_InheritFromMoreThanOneAbstractClass()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
    printLine(x.p1)
    printLine(x.p2)
    printLine(x.product())
    x.setP1(4)
    printLine(x.product())
end main

abstract class Foo
    property p1 Int
    property p2 Int
end class

abstract class Yon
    procedure setP1(v Int)
    function product() -> Int
end class

class Bar inherits Foo, Yon
    constructor()
        p1 = 3
        p2 = 4
    end constructor
    property p1 Int
    property p2 Int

    procedure setP1(p1 Int)
        self.p1 = p1
    end procedure

    function product() -> Int
        return p1 * p2
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
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public interface Foo {
    public int p1 { get; }
    public int p2 { get; }

  }
  public interface Yon {

    public int product();
    public void setP1(ref int v);
  }
  public class Bar : Foo, Yon {
    public static Bar DefaultInstance { get; } = new Bar._DefaultBar();

    public Bar() {
      p1 = 3;
      p2 = 4;
    }
    public int p1 { get; private set; } = default;
    public int p2 { get; private set; } = default;
    public virtual int product() {

      return p1 * p2;
    }
    public virtual string asString() {

      return @$"""";
    }
    public virtual void setP1(ref int p1) {
      this.p1 = p1;
    }
    private class _DefaultBar : Bar {
      public _DefaultBar() { }
      public override void setP1(ref int p1) { }
      public override string asString() { return ""Default Bar"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Bar();
    var l = new StandardLibrary.ElanList<Foo>() + x;
    printLine(x.p1);
    printLine(x.p2);
    printLine(x.product());
    var _setP1_0 = 4;
    x.setP1(ref _setP1_0);
    printLine(x.product());
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Bar) ( )))) (varDef var (assignableValue l) = (expression (expression (newInstance (type (dataStructureType List (genericSpecifier < (type Foo) >))) ( ))) (binaryOp (arithmeticOp +)) (expression (value x)))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p2)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . (methodCall product ( )))) )))) (callStatement (expression (expression (value x)) . (methodCall setP1 ( (argumentList (expression (value (literal (literalValue 4))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . (methodCall product ( )))) ))))) end main) (classDef (abstractClass abstract class Foo (property property p1 (type Int)) (property property p2 (type Int)) end class)) (classDef (abstractClass abstract class Yon procedure (procedureSignature setP1 ( (parameterList (parameter v (type Int))) )) function (functionSignature product ( ) -> (type Int)) end class)) (classDef (mutableClass class Bar (inherits inherits (type Foo) , (type Yon)) (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 3))))) (assignment (assignableValue p2) = (expression (value (literal (literalValue 4)))))) end constructor) (property property p1 (type Int)) (property property p2 (type Int)) (procedureDef procedure (procedureSignature setP1 ( (parameterList (parameter p1 (type Int))) )) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (value p1)))) end procedure) (functionDef (functionWithBody function (functionSignature product ( ) -> (type Int)) statementBlock return (expression (expression (value p1)) (binaryOp (arithmeticOp *)) (expression (value p2))) end function)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n4\r\n12\r\n16\r\n"); //N.B. String prop should be auto-initialised to "" not null
    }

    [TestMethod]
    public void Pass_SuperclassesCanDefineSameMember()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
    printLine(x.p1)
    printLine(x.p2)
    printLine(x.product())
    x.setP1(4)
    printLine(x.product())
end main

abstract class Foo
    property p1 Int
    property p2 Int
end class

abstract class Yon
    property p1 Int
    procedure setP1(v Int)
    function product() -> Int
end class

class Bar inherits Foo, Yon
    constructor()
        p1 = 3
        p2 = 4
    end constructor
    property p1 Int
    property p2 Int

    procedure setP1(p1 Int)
        self.p1 = p1
    end procedure

    function product() -> Int
        return p1 * p2
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
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public interface Foo {
    public int p1 { get; }
    public int p2 { get; }

  }
  public interface Yon {
    public int p1 { get; }
    public int product();
    public void setP1(ref int v);
  }
  public class Bar : Foo, Yon {
    public static Bar DefaultInstance { get; } = new Bar._DefaultBar();

    public Bar() {
      p1 = 3;
      p2 = 4;
    }
    public int p1 { get; private set; } = default;
    public int p2 { get; private set; } = default;
    public virtual int product() {

      return p1 * p2;
    }
    public virtual string asString() {

      return @$"""";
    }
    public virtual void setP1(ref int p1) {
      this.p1 = p1;
    }
    private class _DefaultBar : Bar {
      public _DefaultBar() { }
      public override void setP1(ref int p1) { }
      public override string asString() { return ""Default Bar"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var x = new Bar();
    var l = new StandardLibrary.ElanList<Foo>() + x;
    printLine(x.p1);
    printLine(x.p2);
    printLine(x.product());
    var _setP1_0 = 4;
    x.setP1(ref _setP1_0);
    printLine(x.product());
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue x) = (expression (newInstance (type Bar) ( )))) (varDef var (assignableValue l) = (expression (expression (newInstance (type (dataStructureType List (genericSpecifier < (type Foo) >))) ( ))) (binaryOp (arithmeticOp +)) (expression (value x)))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p1)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . p2)) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . (methodCall product ( )))) )))) (callStatement (expression (expression (value x)) . (methodCall setP1 ( (argumentList (expression (value (literal (literalValue 4))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value x)) . (methodCall product ( )))) ))))) end main) (classDef (abstractClass abstract class Foo (property property p1 (type Int)) (property property p2 (type Int)) end class)) (classDef (abstractClass abstract class Yon (property property p1 (type Int)) procedure (procedureSignature setP1 ( (parameterList (parameter v (type Int))) )) function (functionSignature product ( ) -> (type Int)) end class)) (classDef (mutableClass class Bar (inherits inherits (type Foo) , (type Yon)) (constructor constructor ( ) (statementBlock (assignment (assignableValue p1) = (expression (value (literal (literalValue 3))))) (assignment (assignableValue p2) = (expression (value (literal (literalValue 4)))))) end constructor) (property property p1 (type Int)) (property property p2 (type Int)) (procedureDef procedure (procedureSignature setP1 ( (parameterList (parameter p1 (type Int))) )) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (value p1)))) end procedure) (functionDef (functionWithBody function (functionSignature product ( ) -> (type Int)) statementBlock return (expression (expression (value p1)) (binaryOp (arithmeticOp *)) (expression (value p2))) end function)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

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
    public void Fail_CannotInheritFromConcreteClass()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
end main

class Foo
    constructor()
    end constructor
    property p1 Int
    property p2 Int
    function asString() -> String 
        return """"
    end function
end class

class Bar inherits Foo
    constructor()
        p1 = 3
        p2 = 4
    end constructor
    property p1 Int
    property p2 Int

    procedure setP1(p1 Int)
        self.p1 = p1
    end procedure

    function product() -> Int
        return p1 * p2
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
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_AbstractClassCannotInheritFromConcreteClass()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
end main

class Foo
    constructor()
    end constructor
    property p1 Int
    property p2 Int
    function asString() -> String 
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
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_MustImplementAllInheritedMethods()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
end main

abstract class Foo
    property p1 Int
    property p2 Int

    procedure setP1(v Int)

    function product() -> Int
end class

class Bar inherits Foo
    constructor()
        p1 = 3
        p2 = 4
    end constructor
    property p1 Int
    property p2 Int

    procedure setP1(p1 Int)
        self.p1 = p1
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
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_ImplementedMethodMustHaveSameSignature()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
end main

abstract class Foo
    property p1 Int
    property p2 Int

    procedure setP1(v Int)

    function product() -> Int
end class

class Bar inherits Foo
    constructor()
        p1 = 3
        p2 = 4
    end constructor
    property p1 Int
    property p2 Int

    procedure setP1(p1 Int)
        self.p1 = p1
    end procedure

    function product() -> Float
        return p1 * p2
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
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_AbstractClassDefinesMethodBody()
    {
        var code = @"#
main
    var x = Bar()
    var l = List<Foo>() + x
end main

abstract class Foo
    property p1 Int
    property p2 Int

    procedure setP1(v Int)
        self.p1 = p1
    end procedure

    function product() -> Int
end class
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}