using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T78_identifiersMustBeUniqueIgnoringCase {
    #region Passes

    [TestMethod]
    public void Pass_SameVariableNameInDifferentScope() {
       var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
constant id set to 1

main
    var id set to 2
    print id
end main
";
        var parseTree = @"*";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public const int id = 1;
}

public static class Program {
  private static void Main(string[] args) {
    var id = 2;
    System.Console.WriteLine(StandardLibrary.Functions.asString(id));
  }
}";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\n");
    }

    [TestMethod]
    public void Pass_CanUseCSharpKeywordWithDifferentCase() {
       var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var bReak set to 1
    print bReak
end main
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var bReak = 1;
    System.Console.WriteLine(StandardLibrary.Functions.asString(bReak));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n");
    }

    [TestMethod]
    public void Pass_CanHaveIdentiferSameAsTypeExceptCase() {
       var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var foo set to new Foo()
    print foo
end main
class Foo
    constructor()
    end constructor
    function asString() as String
        return ""Hello World!""
    end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Foo {
    public static Foo DefaultInstance { get; } = new Foo._DefaultFoo();

    public Foo() {

    }

    public virtual string asString() {

      return @$""Hello World!"";
    }
    private record class _DefaultFoo : Foo {
      public _DefaultFoo() { }


      public override string asString() { return ""default Foo"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var foo = new Foo();
    System.Console.WriteLine(StandardLibrary.Functions.asString(foo));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Hello World!\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_DeclareSameVarNameWithDifferentCase() {
       var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var fOO set to 1
    var foo set to 1
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Duplicate id 'foo' in scope 'main'");
    }

    [TestMethod]
    public void Fail_ElanKeywordWithChangedCase() {
       var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var pRocedure set to 1
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Elan Keyword clash 'pRocedure' in scope 'main'");
    }

    [TestMethod]
    public void Fail_ElanKeywordTypeEvenWithChangedCase() {
       var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
class Main 
    constructor()
    end constructor

    function asString() as String
        return """"
    end function
end class
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Elan Keyword clash 'Main' in scope GlobalScope");
    }

    [TestMethod]
    public void Fail_CSharpKeywordWithCorrectCaseIfAlteredCaseAlreadyUsed() {
       var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var bReak set to 1
    var break set to 1
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Duplicate id 'break' in scope 'main'");
    }

    [TestMethod]
    public void Fail_SameVariableNameInScope() {
       var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var id set to 1
    var id set to 1
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Duplicate id 'id' in scope 'main'");
    }

    #endregion
}