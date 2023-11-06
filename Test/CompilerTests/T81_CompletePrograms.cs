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

    [TestMethod, Ignore]
    public void Pass_QueueOfTiles()
    {
        var code = ReadElanSourceCodeFile("words_QueueOfTiles.elan");

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals {
  public const int startHandSize = 15;
  public const int maxHandSize = 20;
  public const int maxTilesPlayed = 50;
  public const string letters = @$"" * ***ABCDEFGHIJKLMNOPQRSTUVWXYZ"";
  public const string mainMenu = @$""
========= 
MAIN MENU
=========

1. Play game with random start hand,
2. Play game with training start hand
9. Quit"";
  public static readonly StandardLibrary.ElanDictionary<char,int> tileDictionary = new StandardLibrary.ElanDictionary<char,int>(('A', 1), ('B', 2), ('C', 2), ('D', 2), ('E', 1), ('F', 3), ('G', 2), ('H', 3), ('I', 1), ('J', 5), ('K', 3), ('L', 2), ('M', 2), ('N', 1), ('O', 1), ('P', 2), ('Q', 5), ('R', 1), ('S', 1), ('T', 1), ('U', 2), ('V', 3), ('W', 3), ('X', 5), ('Y', 3), ('Z', 5));
  public record class QueueOfTiles {
    public static QueueOfTiles DefaultInstance { get; } = new QueueOfTiles._DefaultQueueOfTiles();
    private QueueOfTiles() {}
    public QueueOfTiles(int maxSize) {
      this.maxSize = maxSize;
      contents = new StandardLibrary.ElanArray<string>(maxSize);
      rear = -1;
    }
    protected virtual StandardLibrary.ElanArray<string> contents { get; set; } = StandardLibrary.ElanArray<string>.DefaultInstance;
    protected virtual int rear { get; set; } = default;
    protected virtual int maxSize { get; set; } = default;
    public virtual Boolean isNotEmpty() {

      return rear != -1;
    }
    public virtual string show() {
      var result = @$"""";
      foreach (var letter in contents) {
        result = result + letter;
      }
      return result + newline;
    }
    public virtual string asString() {

      return StandardLibrary.Functions.typeAndProperties(this);
    }
    public virtual void initialise() {
      rear = -1;
      for (var count = 0; count <= maxSize - 2; count = count + 1) {
        add();
      }
    }
    public virtual void withdrawNextLetter(ref string letter) {
      if (isNotEmpty()) {
        letter = contents[0];
        for (var count = 1; count <= rear; count = count + 1) {
          contents[count - 1] = contents[count];
          contents[rear] = @$"""";
        }
        rear = rear - 1;
      }
    }
    public virtual void add() {
      if (rear < maxSize - 1) {
        rear = rear + 1;
        var n = random(0, 30);
        contents[rear] = StandardLibrary.Functions.asString(letters[n]);
      }
    }
    private record class _DefaultQueueOfTiles : QueueOfTiles {
      public _DefaultQueueOfTiles() { }
      protected override StandardLibrary.ElanArray<string> contents => StandardLibrary.ElanArray<string>.DefaultInstance;
      protected override int rear => default;
      protected override int maxSize => default;
      public override void initialise() { }
      public override void withdrawNextLetter(ref string letter) { }
      public override void add() { }
      public override string asString() { return ""default QueueOfTiles"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var q = new QueueOfTiles(10);
    printLine(q.show());
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "xxx");

    }
    #endregion

    #region Fails

    #endregion
}