using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T20_Functions_StatementBodied {
    #region Passes

    [TestMethod]
    public void Pass_SimpleCase() {
        var code = @"
main
    print foo(3,4)
end main

function foo(a Int, b Int) as Int
    return a * b
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int foo(int a, int b) {

    return a * b;
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.foo(3, 4)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "12\r\n");
    }

    [TestMethod]
    public void Pass_Recursive() {
        var code = @"
main
    print factorial(5)
end main

function factorial(a Int) as Int
    var result set to 0;
    if a > 2 then
        set result to a * factorial(a-1)
    else 
        set result to a
    end if
    return result
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static int factorial(int a) {
    var result = 0;
    if (a > 2) {
      result = a * Globals.factorial(a - 1);
    }
    else {
      result = a;
    }
    return result;
  }
}

public static class Program {
  private static void Main(string[] args) {
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.factorial(5)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "120\r\n");
    }

    [TestMethod]
    public void Pass_BinarySearch_Recursive() {
        var code = @"
main
    var li set to {""apple"",""apricot"",""banana"",""lemon"",""lime"",""melon"",""orange"",""pear"",""plum"",""strawberry""}
    print binarySearch(li, ""lemon"")
    print binarySearch(li, ""apple"")
    print binarySearch(li, ""strawberry"")
    print binarySearch(li, ""blueberry"")
end main

function binarySearch(li List<of String>, item String) as  Bool 
  var result set to false
  if li.length() is 0 then  
    set result to false
  else if li.length() is 1 then
    set result to li[0] is item
  else 
    var mid set to li.length() div 2
    var value set to li[mid]
    if item is value then
        set result to true
    else if item.isBefore(value) then
        set result to binarySearch(li[0..mid], item) 
    else 
        set result to binarySearch(li[mid+1..], item)
    end if
  end if
  return result
end function
";

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static bool binarySearch(StandardLibrary.ElanList<string> li, string item) {
    var result = false;
    if (StandardLibrary.Functions.length(li) == 0) {
      result = false;
    }
    else if (StandardLibrary.Functions.length(li) == 1) {
      result = li[0] == item;
    }
    else {
      var mid = StandardLibrary.Functions.length(li) / 2;
      var value = li[mid];
      if (item == value) {
        result = true;
      }
      else if (StandardLibrary.Functions.isBefore(item, value)) {
        result = Globals.binarySearch(li[(0)..(mid)], item);
      }
      else {
        result = Globals.binarySearch(li[(mid + 1)..], item);
      }
    }
    return result;
  }
}

public static class Program {
  private static void Main(string[] args) {
    var li = new StandardLibrary.ElanList<string>(@$""apple"", @$""apricot"", @$""banana"", @$""lemon"", @$""lime"", @$""melon"", @$""orange"", @$""pear"", @$""plum"", @$""strawberry"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.binarySearch(li, @$""lemon"")));
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.binarySearch(li, @$""apple"")));
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.binarySearch(li, @$""strawberry"")));
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.binarySearch(li, @$""blueberry"")));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "true\r\ntrue\r\ntrue\r\nfalse\r\n");
    }

    [TestMethod]
    public void Pass_MergeSort_Recursive() {
        var code = ReadElanSourceCodeFile("mergeSort.Elan");

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static StandardLibrary.ElanList<string> mergeSort(StandardLibrary.ElanList<string> list) {
    var result = list;
    if (StandardLibrary.Functions.length(list) > 1) {
      var mid = StandardLibrary.Functions.length(list) / 2;
      result = Globals.merge(Globals.mergeSort(list[..(mid)]), Globals.mergeSort(list[(mid)..]));
    }
    return result;
  }
  public static StandardLibrary.ElanList<string> merge(StandardLibrary.ElanList<string> a, StandardLibrary.ElanList<string> b) {
    var result = new StandardLibrary.ElanList<string>();
    if (StandardLibrary.Functions.isEmpty(a)) {
      result = b;
    }
    else if (StandardLibrary.Functions.isEmpty(b)) {
      result = a;
    }
    else if (StandardLibrary.Functions.isBefore(a[0], b[0])) {
      result = a[0] + Globals.merge(a[(1)..], b);
    }
    else {
      result = b[0] + Globals.merge(a, b[(1)..]);
    }
    return result;
  }
}

public static class Program {
  private static void Main(string[] args) {
    var li = new StandardLibrary.ElanList<string>(@$""plum"", @$""apricot"", @$""lime"", @$""lemon"", @$""melon"", @$""apple"", @$""orange"", @$""strawberry"", @$""pear"", @$""banana"");
    System.Console.WriteLine(StandardLibrary.Functions.asString(Globals.mergeSort(li)));
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
    }

    #endregion

    #region Fails

    [TestMethod]
    public void Fail_noEnd() {
        var code = @"
main
    call foo(3,4)
end main

function foo(a Int, b Int) as Int
    return a * b
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_noReturnType() {
        var code = @"
main
end main

function foo(a Int, b Int)
    return a * b
end function
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_noAs() {
        var code = @"
main
end main

function foo(a Int, b Int) Int
    return a * b
end function
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_noReturn() {
        var code = @"
main
end main

function foo(a Int, b Int) as Int
    var c set to a * b
end function
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_returnTypeIncompatible() {
        var code = @"
main
   var a set to """"
   set a to foo(3,4)
end main

function foo(a Int, b Int) as Int
    var c set to a * b
    return c
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_noReturn2() {
        var code = @"
main
end main

function foo(a Int, b Int) as Int
    var c set to a * b
    return
end function
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_embeddedReturns() {
        var code = @"
main
end main

function foo(a Int, b Int) as Bool
    if 2 > 1 then
        return true
    else
        return false
    end if
end function
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_nonMatchingReturn2() {
        var code = @"
main
end main

function foo(a Int, b Int) as Int
    return a / b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_statementAfterReturn() {
        var code = @"
main
end main

function foo(a Int, b Int) as Int
    return a * b
    var c set to a + b
end function
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_CanNotContainPrint() {
        var code = @"
main
end main

function foo(a Int, b Int) as Int
    print a
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot print in function");
    }

    [TestMethod]
    public void Fail_CanNotContainSystemAccessors() {
        var code = @"
main
end main

function foo(a Int, b Int) as Int
    var r set to system.random()
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot use system accessor in function");
    }

    [TestMethod]
    public void Fail_CanNotContainProcedureCall() {
        var code = @"
main
    var result set to foo(3,4)
    print result
end main

function foo(a Int, b Int) as Int
    call bar()
    return a * b
end function

procedure bar() 

end procedure
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot call a procedure within a function");
    }

    [TestMethod]
    public void Fail_CannotModifyParam() {
        var code = @"
main
    var result set to foo(3,4)
    print result
end main

function foo(a Int, b Int) as Int
    set a to a + 1
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertDoesNotCompile(compileData, "Cannot modify param in function");
    }

    [TestMethod]
    public void Fail_TooManyParams() {
        var code = @"
main
    var result set to foo(3,4,5)
    print result
end main

function foo(a Int, b Int) as Int
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_NotEnoughParams() {
        var code = @"
main
    var result set to foo(3)
    print result
end main

function foo(a Int, b Int) as Int
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_WrongParamType() {
        var code = @"
main
    var result set to foo(3, ""b"")
    print result
end main

function foo(a Int, b Int) as Int
    return a * b
end function
";
        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeDoesNotCompile(compileData);
    }

    [TestMethod]
    public void Fail_CannotSpecifyParamByRef() {
        var code = @"
main
end main

function foo(ref a Int, b Int) as Int
    return a * b
end function
";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}