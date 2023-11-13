﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T22_RandomNumbers {
    #region Passes

    [TestMethod]
    public void Pass_Raw() {
        var code = @"#
main
     call seedRandom(3)
     var x = random()
     print x
     set x to random()
     print x
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    seedRandom(3);
    var x = random();
    print(x);
    x = random();
    print(x);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "0.2935192125353586\r\n0.6975812123611482\r\n");
    }

    [TestMethod]
    public void Pass_MaxFloat() {
        var code = @"#
main
     call seedRandom(3)
     var x = random(1000.0)
     print x
     set x to random(1000.0)
     print x
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    seedRandom(3);
    var x = random(1000.0);
    print(x);
    x = random(1000.0);
    print(x);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "293.51921253535863\r\n697.5812123611482\r\n");
    }

    [TestMethod]
    public void Pass_RangeFloat() {
        var code = @"#
main
     call seedRandom(3)
     var x = random(100.0, 200.0)
     print x
     set x to random(100.0, 200.0)
     print x
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    seedRandom(3);
    var x = random(100.0, 200.0);
    print(x);
    x = random(100.0, 200.0);
    print(x);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "129.35192125353586\r\n169.75812123611482\r\n");
    }

    [TestMethod]
    public void Pass_MaxInt() {
        var code = @"#
main
     call seedRandom(3)
     var x = random(6)
     print x
     set x to random(6)
     print x
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    seedRandom(3);
    var x = random(6);
    print(x);
    x = random(6);
    print(x);
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
    public void Pass_RangeInt() {
        var code = @"#
main
     call seedRandom(3)
     var x = random(5,10)
     print x
     set x to random(5,10)
     print x
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    seedRandom(3);
    var x = random(5, 10);
    print(x);
    x = random(5, 10);
    print(x);
  }
}";

        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "7\r\n9\r\n");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_CalledWithinExpression()
    {
        var code = @"#
main
     call seedRandom(3)
     print random()
end main
";
        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot use a system call in an expression");
    }

    #endregion
}