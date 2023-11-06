﻿using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] 
public class T81_CompletePrograms
{
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
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static bool binarySearch(StandardLibrary.ElanList<string> li, string item) {
    var result = false;
    if (length(li) == 0) {
      result = false;
    }
    else if (length(li) == 1) {
      result = li[0] == item;
    }
    else {
      var mid = length(li) / 2;
      var value = li[mid];
      if (item == value) {
        result = true;
      }
      else if (isBefore(item, value)) {
        result = binarySearch(li[(0)..(mid)], item);
      }
      else {
        result = binarySearch(li[(mid + 1)..], item);
      }
    }
    return result;
  }
}

public static class Program {
  private static void Main(string[] args) {
    var li = new StandardLibrary.ElanList<string>(@$""apple"", @$""apricot"", @$""banana"", @$""lemon"", @$""lime"", @$""melon"", @$""orange"", @$""pear"", @$""plum"", @$""strawberry"");
    printLine(binarySearch(li, @$""lemon""));
    printLine(binarySearch(li, @$""apple""));
    printLine(binarySearch(li, @$""strawberry""));
    printLine(binarySearch(li, @$""blueberry""));
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
        var code = @"
function mergeSort(list List<String>) -> List<String> 
    var result = list
    if list.length() > 1 then
      var mid = list.length() div 2 
      result = merge(mergeSort(list[..mid]), mergeSort(list[mid..]))
    end if
    return result
end function

function merge(a List<String>, b List<String>) -> List<String>
    var result = List<String>()
    if a.isEmpty() then 
      result = b 
    else if b.isEmpty() then
      result = a
    else if a[0].isBefore(b[0]) then 
      result = a[0] + merge(a[1..], b) 
    else 
      result = b[0] + merge(a, b[1..])
    end if
    return result
end function
main
end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static StandardLibrary.ElanList<string> mergeSort(StandardLibrary.ElanList<string> list) {
    var result = list;
    if (length(list) > 1) {
      var mid = length(list) / 2;
      result = merge(mergeSort(list[..(mid)]), mergeSort(list[(mid)..]));
    }
    return result;
  }
  public static StandardLibrary.ElanList<string> merge(StandardLibrary.ElanList<string> a, StandardLibrary.ElanList<string> b) {
    var result = new StandardLibrary.ElanList<string>();
    if (isEmpty(a)) {
      result = b;
    }
    else if (isEmpty(b)) {
      result = a;
    }
    else if (isBefore(a[0], b[0])) {
      result = a[0] + merge(a[(1)..], b);
    }
    else {
      result = b[0] + merge(a, b[(1)..]);
    }
    return result;
  }
}

public static class Program {
  private static void Main(string[] args) {

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

    #endregion
}