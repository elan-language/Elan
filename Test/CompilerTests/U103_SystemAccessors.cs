using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] [Ignore]
public class TU103_SystemAccessors
{
    #region Passes

    [TestMethod]
    public void Pass_accessorAsOrdinaryFunction() {
        var code = @"
main
 var d =  readFromNetwork(""www.foo.com"") 
 print d
end main

system readFromNetwork(url String) -> String
  return ""data""
end system
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "data\r\n");
    }

    [TestMethod]
    public void Pass_accessorCanBeCalledWithinAProcedure()
    {
        var code = @"

main
 call myProc
end main

procedure myProc()
  var d = readFromNetwork(""www.foo.com"") 
  print d
end procedure

system readFromNetwork(url String) -> String
  return ""data""
end system
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "data\r\n");
    }

    [TestMethod]
    public void Pass_accessorCanCallSystemCallsAndProcedures()
    {
        var code = @"
main
  var d = readFromNetwork(""www.foo.com"")
  print d
end main

system readFromNetwork(url String) -> String
  var r = random()
  print ""checking""
  return ""data""
end system
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "checking\r\ndata\r\n");
    }


    [TestMethod]
    public void Pass_accessorCanCallOtherAccessor()
    {
        var code = @"
main
  var d = readFromNetwork(""www.foo.com"") 
  print d
end main

system readFromNetwork(url String) -> String
  var result = """"
  if check(url)
    result = ""OK data""
  end if
  return result
end system

system check(url String) -> Bool
  return true
end system
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "OK data\r\n");
    }


    #endregion

    #region Fails
    [TestMethod]
    public void Pass_accessorCannotBeCalledWithinAFunction()
    {
        var code = @"
main
end main

function myFunc()
  return readFromNetWork(""url"")
end function

system readFromNetwork(url String) -> String
  return ""data""
end system
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "?");
    }

    [TestMethod]
   public void Fail_accessorCannotBeCalledWithinAnExpression()
    {
        var code = @"
main
 var d =  readFromNetwork(""www.foo.com"") 
 print d + "".""
end main

system readFromNetwork(url String) -> String
  return ""data""
end system
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "?");
    }


    [TestMethod]
    public void Fail_accessorCannotBeCalledWithinAnExpression2()
    {
        var code = @"
main
 print readFromNetwork(""www.foo.com"") 
end main

system readFromNetwork(url String) -> String
  return ""data""
end system
";

        var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "?");
    }


    #endregion
}