using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T12_Arrays {
    #region passes

    [TestMethod]
    public void Pass_DeclareAnEmptyArrayBySizeAndCheckLength() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to Array<of String>(3)
    print a.length()
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
    var a = new StandardLibrary.ElanArray<string>(3);
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.length(a)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    [TestMethod]
    public void Pass_ConfirmStringElementsInitializedToEmptyStringNotNull() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to Array<of String>(3)
    print a[0].length()
    print a
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
    var a = new StandardLibrary.ElanArray<string>(3);
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.length(a[0])));
    System.Console.WriteLine(StandardLibrary.Functions.asString(a));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0\r\nArray {,,}\r\n");
    }

    [TestMethod]
    public void Pass_SetAndReadElements() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to Array<of String>(3)
    set a[0] to ""foo""
    set a[2] to ""yon""
    print a[0]
    print a[2]
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
    var a = new StandardLibrary.ElanArray<string>(3);
    a[0] = @$""foo"";
    a[2] = @$""yon"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[0]));
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[2]));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "foo\r\nyon\r\n");
    }

    [TestMethod]
    public void Pass_InitializeAnArrayFromAList() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to {""foo"",""bar"",""yon""}.asArray()
    print a.length()
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
    var a = StandardLibrary.Functions.asArray(new StandardLibrary.ElanList<string>(@$""foo"", @$""bar"", @$""yon""));
    System.Console.WriteLine(StandardLibrary.Functions.asString(StandardLibrary.Functions.length(a)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    [TestMethod]
    public void Pass_2DArray() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to new Array<of String>(3,4)
    set a[0,0] to ""foo""
    set a[2,3] to ""yon""
    print a[0,0]
    print a[2,3]
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
    var a = new StandardLibrary.ElanArray<string>(3, 4);
    a[0,0] = @$""foo"";
    a[2,3] = @$""yon"";
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[0,0]));
    System.Console.WriteLine(StandardLibrary.Functions.asString(a[2,3]));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "foo\r\nyon\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_UseRoundBracketsForIndex() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to Array<of String>(3)
    var b set to a(0)
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_ApplyIndexToANonIndexable() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to 3
    var b set to a[0]
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_2DArrayCreatedByDoubleIndex() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to Array<of String>[3][4]
    print a[0,0]
    print a[2,3]
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_1DArrayAccessedAs2D() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to Array<of String>(3)
    set a[0,0] to ""foo""
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Index was outside the bounds of the array.");
    }

    [TestMethod]
    public void Fail_OutOfRange() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to Array<of String>(3)
    var b set to a[3]
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
    var a = new StandardLibrary.ElanArray<string>(3);
    var b = a[3];
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Index was outside the bounds of the array."); //or something to that effect
    }

    [TestMethod]
    public void Fail_TypeIncompatibility() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to Array<of String>(3)
    set a[0] to true
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_SizeNotSpecified() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to Array<of String>()
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Array must have size specified in round brackets");
    }

    [TestMethod]
    public void Fail_SizeSpecifiedInSquareBrackets() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to Array<of String>[3]
end main
";
        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_SpecifySizeAndInitializer() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to Array<of String>(3) {""foo"",""bar"",""yon""}
end main
";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_SpecifyWithoutSizeOrInitializer() {
        var code = @"# FFFFFFFFFFFFFFFF Elan v0.1 valid
main
    var a set to Array<of String>()
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(CompileData(code));
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Array must have size");
    }

    #endregion
}