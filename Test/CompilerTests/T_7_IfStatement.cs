using Compiler;

namespace Test.CompilerTests;

using static Helpers;

// Ticket #2
[TestClass]
public class T_7_IfStatement {
    [TestMethod]
    public void Pass1() {
        var code = @"#
main
  var a = true
  if a then
    print ""yes""
  else
    print ""no""
  end if
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
    var a = true;
    if (a) {
      print(@$""yes"");
    }
    else {
      print(@$""no"");
    }
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "yes\r\n");
    }

    [TestMethod]
    public void Pass2() {
        var code = @"#
main
  var a = false
  if a then
    print ""yes""
  else
    print ""no""
  end if
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
    var a = false;
    if (a) {
      print(@$""yes"");
    }
    else {
      print(@$""no"");
    }
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "no\r\n");
    }

    [TestMethod]
    public void Pass3() {
        var code = @"#
main
  var a = 2
  if a is 1 then
    print ""one""
  else if a is 2 then
    print ""two""
  else
    print ""neither""
  end if
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
    var a = 2;
    if (a == 1) {
      print(@$""one"");
    }
    else if (a == 2) {
      print(@$""two"");
    }
    else {
      print(@$""neither"");
    }
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "two\r\n");
    }

    [TestMethod]
    public void Pass4() {
        var code = @"#
main
  var a = 3
  if a is 1 then
    print ""one""
  else if a is 2 then
    print ""two""
  else
    print ""neither""
  end if
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
    var a = 3;
    if (a == 1) {
      print(@$""one"");
    }
    else if (a == 2) {
      print(@$""two"");
    }
    else {
      print(@$""neither"");
    }
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "neither\r\n");
    }

    [TestMethod]
    public void Pass5() {
        var code = @"#
main
  var a = true
  if a then
    print ""yes""
  end if
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
    var a = true;
    if (a) {
      print(@$""yes"");
    }
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "yes\r\n");
    }

    [TestMethod]
    public void Pass6() {
        var code = @"#
main
  var a = 3
  if a is 1 then
    print ""one""
  else if a is 2 then
    print ""two""
  else if a is 3 then
    print ""three""
  else
    print ""neither""
  end if
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
    var a = 3;
    if (a == 1) {
      print(@$""one"");
    }
    else if (a == 2) {
      print(@$""two"");
    }
    else if (a == 3) {
      print(@$""three"");
    }
    else {
      print(@$""neither"");
    }
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "three\r\n");
    }

    [TestMethod]
    public void Fail_NoEndIf() {
        var code = @"#
main
  var a = 3
  if a is 1 then
    print ""one""
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_NoThen() {
        var code = @"#
main
  var a = true
  if a 
    print @$""yes""
  end if
end
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_ElseIfAfterElse() {
        var code = @"#
main
  var a = 3
  if a is 1 then
    print ""one""
  else
    print ""not one""
  else if a is 2 then
    print ""two""
  end if
end main
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }
}