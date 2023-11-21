using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
public class T26_Iter {
    #region Passes

    [TestMethod]
    public void Pass_List() {
        var code = @"
main
  var it = { 1,5,6}
  printEach(it)
end main

procedure printEach(target Iter<of Int>)
  foreach x in target
    print x
  end foreach
end procedure
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n5\r\n6\r\n");
    }

    [TestMethod]
    public void Pass_Array() {
        var code = @"
main
  var it = { 1,3,6}.asArray()
  printEach(it)
end main

procedure printEach(target Iter<of Int>)
  foreach x in target
    print x
  end foreach
end procedure
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1\r\n3\r\n6\r\n");
    }

    [TestMethod]
    public void Pass_String() {
        var code = @"
main
  var s = ""Foo""
  printEach(s)
end main

procedure printEach(target Iter<of Char>)
  foreach x in target
    print x
  end foreach
end procedure
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "F\r\no\r\no\r\n");
    }

    public void Pass_Indexing() {
        var code = @"
main
  var it = { 1,2,3,4,5,6,7}
  printEach(it[2..4])
end main

procedure printEach(target Iter<of Int>)
  foreach x in target
    print x
  end foreach
  print target[0]
end procedure
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "6\r\n3\r\n4\r\n5\r\n1\r\n");
    }

    public void Pass_Printing() {
        var code = @"
main
  var it = { 1,2,3,4,5,6,7}
  printAsIter(it)
  printAsList(it)
end main

procedure printAsIter(target Iter<of Int>)
  var some = target[1..3]
  print some
end procedure

procedure printAsList(target Iter<of Int>)
  var some = target[3..].asList()
  print some
end procedure
";
        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "Iter\r\nList {1,2}");
    }

    [TestMethod]
    public void Pass_Default() {
        var code = @"
main
  var f = Foo()
  print f.it
end main

class Foo
  constructor()
  end constructor

  property it Iter<of Int>

  function asString() as String
    return ""A Foo""
  end function
end class
";

        var objectCode = @"";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "default Iter<Int>");
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_NoGenericTypeSpecified() {
        var code = @"
main
end main

procedure printEach(target Iter)
  foreach x in target
    print x
  end foreach
end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "?");
    }

    [TestMethod]
    public void Fail_PassArgumentWithWrongGenericType() {
        var code = @"
main
  var s = ""Hello""
  printEach(it)
end main

procedure printEach(target Iter<of Int>)
  foreach x in target
    print x
  end foreach
end procedure
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "?");
    }

    #endregion
}