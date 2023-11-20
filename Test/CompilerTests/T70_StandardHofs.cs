﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
public class T70_StandardHofs {
    #region Passes

    [TestMethod]
    public void Pass_Filter() {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.filter(lambda x -> x > 20))
 print source.filter(lambda x -> x > 20)).asList()
 print source.filter(lambda x -> x < 3 or x > 35.asList()
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
        AssertObjectCodeExecutes(compileData, "Iter\r\nList {23,27,31,37}\r\nList {2,37}");
    }

    [TestMethod]
    public void Pass_Map()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.map(lambda x -> x + 1)).asList()
 print source.map(lambda x -> x.asString()+'*')).asList()
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
        AssertObjectCodeExecutes(compileData, "List {3,4,6,8,12,14,18,20,24,28,32,38}\r\nList {2*,3*,5*,7*,11*,13*,17*,19*,23*,27*,31*,37*}");
    }

    [TestMethod]
    public void Pass_Reduce()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.reduce(lambda s,x -> s+x))
 print source.reduce(100, lambda s,x -> s+x))
 print source.reduce(""Concat:"",lambda s,x -> s+x.asString()))
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
        AssertObjectCodeExecutes(compileData, "195\r\n295\r\nresult:23571113171923273137");
    }
    public void Pass_Max()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.max()
 print source.max(lambda x -> x mod 5)).asList()
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
        AssertObjectCodeExecutes(compileData, "37\r\n19\r\n");
    }
    public void Pass_Min()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
 print source.min()
 print source.min(lambda x -> x mod 5)).asList()
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
        AssertObjectCodeExecutes(compileData, "2\r\n13\r\n");
    }

    [TestMethod]
    public void Pass_Any()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
  print source.any(lambda x -> x > 20))
  print source.any(lambda x -> x mod 2 is 0))
  print source.any(lambda x -> x > 40))

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
        AssertObjectCodeExecutes(compileData, "true\r\ntrye\r\nfalse");
    }

    [TestMethod]
    public void Pass_GroupBy()
    {
        var code = @"
constant source = {2,3,5,7,11,13,17,19,23,27,31,37}
main
  var gs = source.groupBy(lambda x -> x mod 5)) # {5} {11,31} {2,7,17,27,37} {13,23,19}
  print gs
  print gs[2]
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
        AssertObjectCodeExecutes(compileData, "Iter<Group<Int,Int>>\r\nGroup 1 {11,31}"); //Not sure quite what output is reasonable?
    }
    #endregion

    #region Fails

    #endregion
}