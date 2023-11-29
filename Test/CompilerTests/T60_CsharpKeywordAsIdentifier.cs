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
  var base set to 1
  var break set to 1
  var byte set to 1
  var checked set to 1
  var const set to 1
  var continue set to 1
  var delegate set to 1
  var do set to 1
  var double set to 1
  var event set to 1
  var explicit set to 1
  var extern set to 1
  var finally set to 1
  var fixed set to 1
  var goto set to 1
  var implicit set to 1
  var interface set to 1
  var internal set to 1
  var lock set to 1
  var long set to 1
  var namespace set to 1
  var null set to 1
  var object set to 1
  var operator set to 1
  var override set to 1
  var params set to 1
  var protected set to 1
  var public set to 1
  var readonly set to 1
  var sbyte set to 1
  var sealed set to 1
  var short set to 1
  var sizeof set to 1
  var stackalloc set to 1
  var static set to 1
  var struct set to 1
  var this set to 1
  var typeof set to 1
  var uint set to 1
  var ulong set to 1
  var unchecked set to 1
  var unsafe set to 1
  var ushort set to 1
  var using set to 1
  var virtual set to 1
  var void set to 1
  var volatile set to 1
  var total set to base+break+byte+checked+const+continue+delegate+do+double+event+explicit+extern+finally+fixed+goto+implicit+interface+internal+lock+long+namespace+null+object+operator+override+params+protected+public+readonly+sbyte+sealed+short+sizeof+stackalloc+static+struct+this+typeof+uint+ulong+unchecked+unsafe+ushort+using+virtual+void+volatile
  print total
end main
";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "47\r\n");
    }

    [TestMethod]
    public void Pass_CSKeywordAsType() {
        var code = @"
main
  var m set to new Base(3)
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
  var procedure set to 1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod]
    public void Fail_OutAsIdentifier() {
        var code = @"
main
  var out set to 1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}