using Compiler;
using CSharpLanguageModel;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] 
public class T81_CompletePrograms
{
    [TestInitialize]
    public void TestInit() {
        CodeHelpers.ResetUniqueId();
    }

    #region Passes

    [TestMethod]
    public void Pass_BinarySearch() {
        var code = @"
main
    var li = {""apple"",""apricot"",""banana"",""lemon"",""lime"",""melon"",""orange"",""pear"",""plum"",""strawberry""}
    printLine(binarySearch(li, ""lemon""))
    printLine(binarySearch(li, ""apple""))
    printLine(binarySearch(li, ""strawberry""))
    printLine(binarySearch(li, ""blueberry""))
end main

function binarySearch(li List<String>, item String) ->  Bool 
  var result = false
  if li.length() is 0 then  
    result = false
  else if li.length() is 1 then
    result = li[0] is item
  else 
    var mid = li.length() div 2
    var value = li[mid]
    if item is value then
        result = true
    else if item.isBefore(value) then
        result = binarySearch(li[0..mid], item) 
    else 
        result = binarySearch(li[mid+1..], item)
    end if
  end if
  return result
end function
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
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
    printLine(Globals.binarySearch(li, @$""lemon""));
    printLine(Globals.binarySearch(li, @$""apple""));
    printLine(Globals.binarySearch(li, @$""strawberry""));
    printLine(Globals.binarySearch(li, @$""blueberry""));
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
    public void Pass_MergeSort() {
        var code = ReadElanSourceCodeFile("mergeSort.Elan");

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
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
    printLine(Globals.mergeSort(li));
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

    [TestMethod]
    public void Pass_QueueOfTiles()
    {
        var code = ReadElanSourceCodeFile("words_QueueOfTiles.elan");

        //var objectCode = @"";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        //AssertObjectCodeIs(compileData, objectCode);
      }

    [TestMethod, Ignore]
    public void Pass_Player()
    {
        var code = ReadElanSourceCodeFile("words_Player.elan");
        //var objectCode = @"";
        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        //AssertObjectCodeIs(compileData, objectCode);
    }

    [TestMethod, Ignore]
    public void Pass_Game()
    {
        var code = ReadElanSourceCodeFile("words_Game.elan");
        var objectCode = @"";
        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
    }


    [TestMethod, Ignore]
    public void Pass_Constants()
    {
        var code = ReadElanSourceCodeFile("words_Constants.elan");
        //var objectCode = @"";
        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        //AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "****ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\n");
    }


    [TestMethod, Ignore]
    public void Pass_Complete()
    {
        var code = ReadElanSourceCodeFile("words_complete.elan");
        //var objectCode = @"";
        var parseTree = @"*";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        //AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "");
    }
    #endregion
}