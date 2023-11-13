using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T28_Expressions2_Brackets {
    #region Passes

    [TestMethod]
    public void Pass_BracketsChangeOperatorEvaluation() {
        var code = @"#
main
  var x = 2 + 3 * 5 + 1
  var y = (2 + 3) * 5 + 1
  var z = (2 + 3) * (5 + 1)
  print x
  print y
  print z
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 2 + 3 * 5 + 1;
    var y = (2 + 3) * 5 + 1;
    var z = (2 + 3) * (5 + 1);
    print(x);
    print(y);
    print(z);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "18\r\n26\r\n30\r\n");
    }

    [TestMethod]
    public void Pass_RedundantBracketsIgnored() {
        var code = @"#
main
  var x = 2 + (3 * 5) + 1
  var y = ((2 + 3)) * 5 + (1)
  var z = ((2 + 3) * (5 + 1))
  print x
  print y
  print z
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 2 + (3 * 5) + 1;
    var y = ((2 + 3)) * 5 + (1);
    var z = ((2 + 3) * (5 + 1));
    print(x);
    print(y);
    print(z);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "18\r\n26\r\n30\r\n");
    }

    [TestMethod]
    public void Pass_PowerHasHigherPrecedenceThatMultiply() {
        var code = @"#
main
  var x = 2 + 3 ^ 2
  var y = (2 + 3) ^ 2
  print x
  print y
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 2 + System.Math.Pow(3, 2);
    var y = System.Math.Pow((2 + 3), 2);
    print(x);
    print(y);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "11\r\n25\r\n");
    }

    [TestMethod]
    public void Pass_PowerHasHigherPrecedenceThanFloatDivision() {
        var code = @"#
main
  var x = 16.0 / 2 ^ 3
  var y = (16.0/2) ^ 3
  print x
  print y
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = Compiler.WrapperFunctions.FloatDiv(16.0, System.Math.Pow(2, 3));
    var y = System.Math.Pow((Compiler.WrapperFunctions.FloatDiv(16.0, 2)), 3);
    print(x);
    print(y);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\n512\r\n");
    }

    [TestMethod]
    public void Pass_PowerHasHigherPrecedenceThanIntDivision() {
        //Point of this test is that dividing an integer by an integer is implemented a funciton, not an operator, in C#
        var code = @"#
main
  var x = 16 / 2 ^ 3
  var y = (16/2) ^ 3
  print x
  print y
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = Compiler.WrapperFunctions.FloatDiv(16, System.Math.Pow(2, 3));
    var y = System.Math.Pow((Compiler.WrapperFunctions.FloatDiv(16, 2)), 3);
    print(x);
    print(y);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\n512\r\n");
    }

    [TestMethod]
    public void Pass_MinusAsAUnaryOperator() {
        var code = @"#
main
  var x = - 4.7
  var y = 5 * -3
  print x
  print y
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = -4.7;
    var y = 5 * -3;
    print(x);
    print(y);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "-4.7\r\n-15\r\n");
    }

    [TestMethod]
    public void Pass_OperatorPrecedenceForMod() {
        var code = @"#
main
    var x = 11 mod 3
    var y = 5 + 6 mod 3
    print x
    print y
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 11 % 3;
    var y = 5 + 6 % 3;
    print(x);
    print(y);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\n5\r\n");
    }

    [TestMethod]
    public void Pass_OperatorPrecedenceForDiv() {
        var code = @"#
main
    var x = 11 div 3
    var y = 5 + 6 div 3
    print x
    print y
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Procedures;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var x = 11 / 3;
    var y = 5 + 6 / 3;
    print(x);
    print(y);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n7\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_PlusIsNotUnary() {
        var code = @"#
    main
      var a = 3 * + 4
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_MultiplyAfterMinus() {
        var code = @"#
    main
      var a = 3 - * 4
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}