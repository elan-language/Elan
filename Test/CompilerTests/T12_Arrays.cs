using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T12_Arrays {
    #region passes

    [TestMethod, Ignore]
    public void Pass_DeclareAnEmptyArrayBySizeAndCheckLength() {
        var code = @"
main
    var a = Array<String>(3)
    printLine(a.length())
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new StandardLibrary.Array<string>(3);
    printLine(length(a));
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (dataStructureDefinition (arrayDefinition new Array (genericSpecifier < (type String) >) ( 3 )))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) . (methodCall length ( )))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_ConfirmStringElementsInitializedToEmptyStringNotNull() {
        var code = @"
main
    var a = Array<String>(3)
    printLine(a[0].length())
    printLine(a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new StandardLibrary.Array<string>(3);
    printLine(length(a[0]));
    printLine(a);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (dataStructureDefinition (arrayDefinition new Array (genericSpecifier < (type String) >) ( 3 )))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (expression (value a)) (index [ (expression (value (literal (literalValue 0)))) ])) . (methodCall length ( )))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (value a))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0\r\nArray {,,}\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_SetAndReadElements() {
        var code = @"
main
    var a = Array<String>(3)
    a[0] = ""foo""
    a[2] = ""yon""
    printLine(a[0])
    printLine(a[2])
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new StandardLibrary.Array<string>(3);
    a[0] = @$""foo"";
    a[2] = @$""yon"";
    printLine(a[0]);
    printLine(a[2]);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (dataStructureDefinition (arrayDefinition new Array (genericSpecifier < (type String) >) ( 3 )))))) (assignment (assignableValue a (index [ (expression (value (literal (literalValue 0)))) ])) = (expression (value (literal (literalDataStructure ""foo""))))) (assignment (assignableValue a (index [ (expression (value (literal (literalValue 2)))) ])) = (expression (value (literal (literalDataStructure ""yon""))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) (index [ (expression (value (literal (literalValue 0)))) ]))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) (index [ (expression (value (literal (literalValue 2)))) ]))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "foo\r\nyon\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_InitializeAnArrayFromAList() {
        var code = @"
main
    var a = {""foo"",""bar"",""yon""}.asArray()
    printLine(a.length())
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new StandardLibrary.Array<string>() {@$""foo"", @$""bar"", @$""yon""};
    printLine(length(a));
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (value (dataStructureDefinition (arrayDefinition new Array (genericSpecifier < (type String) >) ( ) (listDefinition { (expression (value (literal (literalDataStructure ""foo"")))) , (expression (value (literal (literalDataStructure ""bar"")))) , (expression (value (literal (literalDataStructure ""yon"")))) })))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) . (methodCall length ( )))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_2DArray() {
        var code = @"
main
    var a = Array<String>(3,4)
    a[0,0] = ""foo""
    a[2,3] = ""yon""
    printLine(a[0,0])
    printLine(a[2,3])
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new StandardLibrary.Array<string>(3, 4);
    a[0,0] = @$""foo"";
    a[2,3] = @$""yon"";
    printLine(a[0,0]);
    printLine(a[2,3]);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue a) = (expression (newInstance new (type (dataStructureType Array (genericSpecifier < (type String) >))) ( (argumentList (expression (value (literal (literalValue 3)))) , (expression (value (literal (literalValue 4))))) )))) (assignment (assignableValue a (index [ (expression (value (literal (literalValue 0)))) , (expression (value (literal (literalValue 0)))) ])) = (expression (value (literal (literalDataStructure ""foo""))))) (assignment (assignableValue a (index [ (expression (value (literal (literalValue 2)))) , (expression (value (literal (literalValue 3)))) ])) = (expression (value (literal (literalDataStructure ""yon""))))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) (index [ (expression (value (literal (literalValue 0)))) , (expression (value (literal (literalValue 0)))) ]))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value a)) (index [ (expression (value (literal (literalValue 2)))) , (expression (value (literal (literalValue 3)))) ]))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
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
        var code = @"
main
    var a = Array<String>(3)
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
    public void Fail_ApplyIndexToANonIndexable() {
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
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_2DArrayCreatedByDoubleIndex() {
        var code = @"
main
    var a = Array<String>[3][4]
    printLine(a[0,0])
    printLine(a[2,3])
end main
";
       
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_1DArrayAccessedAs2D() {
        var code = @"
main
    var a = Array<String>(3)
    a[0,0] = ""foo""
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Index was outside the bounds of the array.");
    }

    [TestMethod]
    public void Fail_OutOfRange() {
        var code = @"
main
    var a = Array<String>(3)
    var b = a[3]
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var a = new StandardLibrary.Array<string>(3);
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
        AssertObjectCodeFails(compileData, "Index was outside the bounds of the array."); //or something to that effect
    }

    [TestMethod]
    public void Fail_TypeIncompatibility() {
        var code = @"
main
    var a = Array<String>(3)
    a[0] = true
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_SizeNotSpecified() {
        var code = @"
main
    var a = Array<String>()
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_SizeSpecifiedInSquareBrackets() {
        var code = @"
main
    var a = Array<String>[3]
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod, Ignore]
    public void Fail_SpecifySizeAndInitializer() {
        var code = @"
main
    var a = Array<String>(3) {""foo"",""bar"",""yon""}
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_SpecifyWithoutSizeOrInitializer() {
        var code = @"
main
    var a = Array<String>()
end main
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData);
    }

    #endregion
}