using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass, Ignore]
public class T12_Arrays
{
    [TestMethod]
    public void Pass_DeclareAnEmptyArrayBySizeAndCheckLength()
    {
        var code = @"
main
    var a = new Array<String>(3)
    printLine(a.length())
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new string[3];
    printLine(a.length());
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    [TestMethod]
    public void Pass_ConfirmStringElementsInitializedToEmptyStringNotNull()
    {
        var code = @"
main
    var a = new Array<String>(3)
    printLine(a[0].length())
    printLine(a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new string[3];
    printLine(a.length());
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0\r\n{,,}\r\n");
    }

    [TestMethod]
    public void Pass_SetAndReadElements()
    {
        var code = @"
main
    var a = new Array<String>(3)
    a[0] = ""foo""
    a[2] = ""yon""
    printLine(a[0])
    printLine(a[2])
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new string[3];
    a[0] = ""foo"";
    a[2] = ""yon"";
    printLine(a[0]);
    printLine(a[2]);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "foo\r\nyon\r\n");
    }

    [TestMethod]
    public void Pass_InitializeAnArrayFromAList()
    {
        var code = @"
main
    var a = new Array<String> {""foo"",""bar"",""yon""}
    printLine(a.length())
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new string[] {""foo"",""bar"",""yon""};
    printLine(a.length());
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }


    [TestMethod]
    public void Pass_2DArray()
    {
        var code = @"
main
    var a = new Array<String>(3,4)
    a[0,0] = ""foo""
    a[2,3] = ""yon""
    printLine(a[0,0])
    printLine(a[2,3])
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new string[3,4];
    a[0,0] = ""foo"";
    a[2,3] = ""yon"";
    printLine(a[0,0]);
    printLine(a[2,3]);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "foo\r\nyon\r\n");
    }

    [TestMethod]
    public void Fail_UseRoundBracketsForIndex()
    { 
        var code = @"
main
    var a = new Array<String>(3)
    var b = a(0)
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_ApplyIndexToANonIndexable()
    {
        var code = @"
main
    var a = 3
    var b = a[0]
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }


    [TestMethod]
    public void Fail_2DArrayCreatedByDoubleIndex()
    {
        var code = @"
main
    var a = new Array<String>[3][4]
    printLine(a[0,0])
    printLine(a[2,3])
end main
";
        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_1DArrayAccessedAs2D()
    {
        var code = @"
main
    var a = new Array<String>[3]
    a[0,0] = ""foo""
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_OutOfRange()
    {
        var code = @"
main
    var a = new Array<String>(3)
    var b = a[3]
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;using static StandardLibrary.Functions;

public static partial class GlobalConstants {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new string[3];
    var b = a[3];
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Out Of Range"); //or something to that effect
    }


    [TestMethod]
    public void Fail_TypeIncompatibility()
    {
        var code = @"
main
    var a = new Array<String>(3)
    a[0] = true
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_SizeNotSpecified()
    {
        var code = @"
main
    var a = new Array<String>()
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_SizeSpecifiedInSquareBrackets()
    {
        var code = @"
main
    var a = new Array<String>[3]
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_InitializeAnArrayFromListAndSpecifySizeEvenIfCorrect()
    {
        var code = @"
main
    var a = new Array<String>(3) {""foo"",""bar"",""yon""}
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
}