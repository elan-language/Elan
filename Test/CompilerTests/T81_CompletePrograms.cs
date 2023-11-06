using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass] 
public class T81_CompletePrograms
{
    #region Passes

    [TestMethod]
    public void Pass_BinarySearch() {
        var code = @"
function binarySearch(li List<String>, item String) ->  Bool 
  var mid = li.length() div 2
  var value = li[mid]
  var result = false
  if not li.isEmpty() then  
     if item == value then
        result = true
     else if item.isBefore(value) then
        result = binarySearch(li[0..mid], item) 
     else 
        result = binarySearch(li[mid+1..], item)
     end if
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
  public static bool binarySearch(StandardLibrary.ElanList<string> li, string item) {
    var mid = StandardLibrary.Functions.length(li) / 2;
    var value = li[mid];
    var result = false;
    if (!StandardLibrary.Functions.isEmpty(li)) {
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
    else if head(a).isBefore(head(b)) then 
      result = head(a) + merge(tail(a), b) 
    else 
      result = head(b) + merge(a, tail(b))
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
    else if (StandardLibrary.Functions.isBefore(StandardLibrary.Functions.head(a), StandardLibrary.Functions.head(b))) {
      result = StandardLibrary.Functions.head(a) + Globals.merge(StandardLibrary.Functions.tail(a), b);
    }
    else {
      result = StandardLibrary.Functions.head(b) + Globals.merge(a, StandardLibrary.Functions.tail(b));
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