using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T28_Expressions2_Brackets {
    #region Passes

    [TestMethod]
    public void Pass_BracketsChangeOperatorEvaluation() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var x set to 2 + 3 * 5 + 1
  var y set to (2 + 3) * 5 + 1
  var z set to (2 + 3) * (5 + 1)
  print x
  print y
  print z
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
    var x = 2 + 3 * 5 + 1;
    var y = (2 + 3) * 5 + 1;
    var z = (2 + 3) * (5 + 1);
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
    System.Console.WriteLine(StandardLibrary.Functions.asString(z));
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
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var x set to 2 + (3 * 5) + 1
  var y set to ((2 + 3)) * 5 + (1)
  var z set to ((2 + 3) * (5 + 1))
  print x
  print y
  print z
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
    var x = 2 + (3 * 5) + 1;
    var y = ((2 + 3)) * 5 + (1);
    var z = ((2 + 3) * (5 + 1));
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
    System.Console.WriteLine(StandardLibrary.Functions.asString(z));
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
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var x set to 2 + 3 ^ 2
  var y set to (2 + 3) ^ 2
  print x
  print y
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
    var x = 2 + System.Math.Pow(3, 2);
    var y = System.Math.Pow((2 + 3), 2);
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
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
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var x set to 16.0 / 2 ^ 3
  var y set to (16.0/2) ^ 3
  print x
  print y
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
    var x = Compiler.WrapperFunctions.FloatDiv(16.0, System.Math.Pow(2, 3));
    var y = System.Math.Pow((Compiler.WrapperFunctions.FloatDiv(16.0, 2)), 3);
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
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
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var x set to 16 / 2 ^ 3
  var y set to (16/2) ^ 3
  print x
  print y
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
    var x = Compiler.WrapperFunctions.FloatDiv(16, System.Math.Pow(2, 3));
    var y = System.Math.Pow((Compiler.WrapperFunctions.FloatDiv(16, 2)), 3);
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
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
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
  var x set to - 4.7
  var y set to 5 * -3
  print x
  print y
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
    var x = -4.7;
    var y = 5 * -3;
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
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
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var x set to 11 mod 3
    var y set to 5 + 6 mod 3
    print x
    print y
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
    var x = 11 % 3;
    var y = 5 + 6 % 3;
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
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
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
main
    var x set to 11 div 3
    var y set to 5 + 6 div 3
    print x
    print y
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
    var x = 11 / 3;
    var y = 5 + 6 / 3;
    System.Console.WriteLine(StandardLibrary.Functions.asString(x));
    System.Console.WriteLine(StandardLibrary.Functions.asString(y));
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
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
    main
      var a set to 3 * + 4
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_MultiplyAfterMinus() {
        var code = @"# Elan v0.1 valid FFFFFFFFFFFFFFFF
    main
      var a set to 3 - * 4
    end main
    ";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}