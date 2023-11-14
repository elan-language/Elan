using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T60_CsharpKeywordAsIdentifier {
  #region Passes

  [TestMethod]
  public void Pass_CSkeywordAsIdentifier() {
    var code = @"
main
  var base = 1
  var break = 1
  var byte = 1
  var checked = 1
  var const = 1
  var continue = 1
  var delegate = 1
  var do = 1
  var double = 1
  var event = 1
  var explicit = 1
  var extern = 1
  var finally = 1
  var fixed = 1
  var goto = 1
  var implicit = 1
  var interface = 1
  var internal = 1
  var lock = 1
  var long = 1
  var namespace = 1
  var new = 1
  var null = 1
  var object = 1
  var operator = 1
  var out = 1
  var override = 1
  var params = 1
  var protected = 1
  var public = 1
  var readonly = 1
  var ref = 1
  var sbyte = 1
  var sealed = 1
  var short = 1
  var sizeof = 1
  var stackalloc = 1
  var static = 1
  var struct = 1
  var this = 1
  var typeof = 1
  var uint = 1
  var ulong = 1
  var unchecked = 1
  var unsafe = 1
  var ushort = 1
  var using = 1
  var virtual = 1
  var void = 1
  var volatile = 1
  var total = base+break+byte+checked+const+continue+delegate+do+double+event+explicit+extern+finally+fixed+goto+implicit+interface+internal+lock+long+namespace+new+null+object+operator+out+override+params+protected+public+readonly+ref+sbyte+sealed+short+sizeof+stackalloc+static+struct+this+typeof+uint+ulong+unchecked+unsafe+ushort+using+virtual+void+volatile
  print total
end main
";

   var parseTree = @"*";

    var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
    AssertParses(compileData);
    AssertParseTreeIs(compileData, parseTree);
    AssertCompiles(compileData);
    AssertObjectCodeCompiles(compileData);
    AssertObjectCodeExecutes(compileData, "50\r\n");
  }

  [TestMethod]
  public void Pass_CSKeywordAsType() {
    var code = @"
main
  var m = Base(3)
  print m.p1
end main

class Base
  constructor(p1 Int)
    set self.p1 to p1
  end constructor
  property p1 Int
  function asString() as String
    return """"
  end function
end class
";

 

    var parseTree = @"*";

    var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
    AssertParses(compileData);
    AssertParseTreeIs(compileData, parseTree);
    AssertCompiles(compileData);
    AssertObjectCodeCompiles(compileData);
    AssertObjectCodeExecutes(compileData, "3\r\n");
  }

  #endregion

  #region Fails

  [TestMethod]
  public void Fail_ElanKeywordAsIdentifier() {
    var code = @"
main
  var procedure = 1
end main
";
    var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
    AssertDoesNotParse(compileData);
  }
  #endregion
}
