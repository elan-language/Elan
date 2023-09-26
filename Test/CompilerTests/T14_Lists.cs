using Compiler;
using System.Collections.Immutable;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T14_Lists {

    #region Passes
    [TestMethod]
    public void Pass_literalList() {
        var code = @"
main
    var a = {4,5,6,7,8}
    printLine(a)
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
    var a = ImmutableList.Create<int>(4, 5, 6, 7, 8);
    printLine(a);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {4,5,6,7,8}\r\n");
    }

    [TestMethod]
    public void Pass_literalListWithCoercion()
    {
        var code = @"
main
    var a = {4.1,5,6,7,8}
    printLine(a)
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
    var a = ImmutableList.Create<double>(4.1, 5, 6, 7, 8);
    printLine(a);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {4.1,5,6,7,8}\r\n");
    }

    [TestMethod]
    public void Pass_length()
    {
        var code = @"
main
    var a = {4,5,6,7,8}
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
    var a = ImmutableList.Create<int>(4, 5, 6, 7, 8);
    printLine(length(a));
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

    [TestMethod, Ignore]
    public void Pass_emptyList()
    {
        var code = @"
main
    var a = new List<Int>()
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
    var a = new ImmutableList<int>();
    printLine(a.length);
    printLine(a);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""Hello World!""))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0\r\n{}\r\n");
    }

    [TestMethod]
    public void Pass_index()
    {
        var code = @"
main
    var a = {4,5,6,7,8}
    printLine(a[2])
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
    var a = ImmutableList.Create<int>(4, 5, 6, 7, 8);
    printLine(a.ToImmutableArray()[2]);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "6\r\n");
    }

    [TestMethod]
    public void Pass_range()
    {
        var code = @"
main
    var a = {4,5,6,7,8}
    printLine(a[2..])
    printLine(a[1..3])
    printLine(a[..2])
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
    var a = ImmutableList.Create<int>(4, 5, 6, 7, 8);
    printLine(a.ToImmutableArray()[(2)..]);
    printLine(a.ToImmutableArray()[(1)..(3)]);
    printLine(a.ToImmutableArray()[..(2)]);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {6,7,8}\r\nList {5,6}\r\nList {4,5}\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_addElementToList()
    {
        var code = @"
main
    var a = {4,5,6,7,8}
    var b = a + 9
    printLine(a)
    printLine(b)
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
    var a = new ImmutableList<int> {4,5,6,7,8};
    var b = a.Add(9);
    printLine(a);
    printLine(b);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""Hello World!""))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "{4,5,6,7,8,9}\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_addListToElement()
    {
        var code = @"
main
    var a = {4,5,6,7,8}
    var b = 9 + a
    printLine(a)
    printLine(b)
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
    var a = new ImmutableList<int> {4,5,6,7,8};
    var b = a.Add(9);
    printLine(a);
    printLine(b);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""Hello World!""))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "{9,4,5,6,7,8}\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_addListToListUsingPlus()
    {
        var code = @"
main
    var a = {4,5,6,7,8}
    var b = {1,2,3}
    var c = a + b
    printLine(c)
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
    var a = new ImmutableList<int> {4,5,6,7,8};
    var b = new ImmutableList<int> {1,2,3};
    var c = a.AddRange(b);
    printLine(c);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""Hello World!""))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "{4,5,6,7,8,1,2,3}\r\n");
    }

    [TestMethod]
    public void Pass_constantLists()
    {
        var code = @"
constant a = {4,5,6,7,8}
main
    printLine(a)
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static GlobalConstants;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;

public static partial class GlobalConstants {
  public static readonly ImmutableList<int> a = ImmutableList.Create<int>(4, 5, 6, 7, 8);
}

public static class Program {
  private static void Main(string[] args) {
    printLine(a);
  }
}";

        var parseTree = @"(file (constantDef constant a = (literal (literalDataStructure (literalList { (literal (literalValue 4)) , (literal (literalValue 5)) , (literal (literalValue 6)) , (literal (literalValue 7)) , (literal (literalValue 8)) })))) (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value a))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {4,5,6,7,8}\r\n");
    }

    [TestMethod, Ignore]
    public void Pass_createEmptyListUsingConstructor()
    {
        var code = @"
main
    var a = new List<Int>()
    printLine(a)
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
    var a = new ImmutableList<int>()};
    printLine(a);
  }
}";

        var parseTree = @"(file (main main (statementBlock (callStatement (expression (methodCall printLine ( (argumentList (expression (value (literal (literalDataStructure ""Hello World!""))))) ))))) end main) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "empty List\r\n");
    }
    #endregion

    #region Fails
    [TestMethod]
    public void Fail_emptyLiteralList()
    {
        var code = @"
main
    var a = {}
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_literalListInconsistentTypes1()
    {
        var code = @"
main
    var a = {3, ""apples""}
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_literalListInconsistentTypes2()
    {
        var code = @"
main
    var a = {3, 3.1}
end main
";  //Because list type is decided by FIRST element

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_OutOfRange()
    {
        var code = @"
main
    var a = {4,5,6,7,8}
    var b = a[5];
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
    var a = ImmutableList.Create<int>(4, 5, 6, 7, 8);
    var b = a.ToImmutableArray()[5];
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeFails(compileData, "Index was outside the bounds of the array");
    }
    #endregion
}