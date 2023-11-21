using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
public class T67_ListDeconstruction {
    #region Passes

    [TestMethod]
    public void Pass_IntoNewVars() {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
  var {x:xs} = source
  print x
  print xs
end main
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nList {2,3,5,7,11,13,17,19,23,27,31,37}\r\n");
    }

    [TestMethod]
    public void Pass_IntoExistingVars() {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
  var x = 0
  var y = default List<of String>
  set {x:xs} to source
  print x
  print xs
end main
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "2\r\nList {2,3,5,7,11,13,17,19,23,27,31,37}\r\n");
    }

    [TestMethod]
    public void Pass_InParam() {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
  var {x:xs} = source
  print x
  print xs
end main

function reverse({x:xs} as List<Int>) as List<Int> -> if xs.length() is 0 then {x} 
  else reverse(xs) + x
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "List {37,31,27,23,19,17,13,11,7,5,3,2}\r\n");
    }

    #endregion

    #region Fails

    #endregion
}