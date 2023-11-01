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
    var enum = 1
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
    var throw = 1
    var typeof = 1
    var uint = 1
    var ulong = 1
    var unchecked = 1
    var unsafe = 1
    var ushort = 1
    var using = 1
    var virtual = 1
    var void = 1
    var volatile  = 1
    
    print(base);
    print(break)
    print(byte)
    print(checked)
    print(const)
    print(continue)
    print(delegate)
    print(do)
    print(double)
    print(enum)
    print(event)
    print(explicit)
    print(extern)
    print(finally)
    print(fixed)
    print(goto)
    print(implicit)
    print(interface)
    print(internal)
    print(lock)
    print(long)
    print(namespace)
    print(new)
    print(null)
    print(object)
    print(operator)
    print(out)
    print(override)
    print(params)
    print(protected)
    print(public)
    print(readonly)
    print(ref)
    print(sbyte)
    print(sealed)
    print(short)
    print(sizeof)
    print(stackalloc)
    print(static)
    print(struct)
    print(this)
    print(throw)
    print(typeof)
    print(uint)
    print(ulong)
    print(unchecked)
    print(unsafe)
    print(ushort)
    print(using)
    print(virtual)
    print(void)
    print(volatile )


end main
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {

}

public static class Program {
  private static void Main(string[] args) {
    var @base = 1;
    var @break = 1;
    var @byte = 1;
    var @checked = 1;
    var @const = 1;
    var @continue = 1;
    var @delegate = 1;
    var @do = 1;
    var @double = 1;
    var @enum = 1;
    var @event = 1;
    var @explicit = 1;
    var @extern = 1;
    var @finally = 1;
    var @fixed = 1;
    var @goto = 1;
    var @implicit = 1;
    var @interface = 1;
    var @internal = 1;
    var @lock = 1;
    var @long = 1;
    var @namespace = 1;
    var @new = 1;
    var @null = 1;
    var @object = 1;
    var @operator = 1;
    var @out = 1;
    var @override = 1;
    var @params = 1;
    var @protected = 1;
    var @public = 1;
    var @readonly = 1;
    var @ref = 1;
    var @sbyte = 1;
    var @sealed = 1;
    var @short = 1;
    var @sizeof = 1;
    var @stackalloc = 1;
    var @static = 1;
    var @struct = 1;
    var @this = 1;
    var @throw = 1;
    var @typeof = 1;
    var @uint = 1;
    var @ulong = 1;
    var @unchecked = 1;
    var @unsafe = 1;
    var @ushort = 1;
    var @using = 1;
    var @virtual = 1;
    var @void = 1;
    var @volatile = 1;
    print(@base);
    print(@break);
    print(@byte);
    print(@checked);
    print(@const);
    print(@continue);
    print(@delegate);
    print(@do);
    print(@double);
    print(@enum);
    print(@event);
    print(@explicit);
    print(@extern);
    print(@finally);
    print(@fixed);
    print(@goto);
    print(@implicit);
    print(@interface);
    print(@internal);
    print(@lock);
    print(@long);
    print(@namespace);
    print(@new);
    print(@null);
    print(@object);
    print(@operator);
    print(@out);
    print(@override);
    print(@params);
    print(@protected);
    print(@public);
    print(@readonly);
    print(@ref);
    print(@sbyte);
    print(@sealed);
    print(@short);
    print(@sizeof);
    print(@stackalloc);
    print(@static);
    print(@struct);
    print(@this);
    print(@throw);
    print(@typeof);
    print(@uint);
    print(@ulong);
    print(@unchecked);
    print(@unsafe);
    print(@ushort);
    print(@using);
    print(@virtual);
    print(@void);
    print(@volatile);
  }
}";

        var parseTree = @"*";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "1111111111111111111111111111111111111111111111111111");
    }

    [TestMethod]
    public void Pass_CSKeywordAsType() {
        var code = @"
main
    var m = Base(3)
    printLine(m.p1)
end main

class Base
    constructor(p1 Int)
        self.p1 = p1
    end constructor
    property p1 Int
    function asString() -> String
        return """"
    end function
end class
";

        var objectCode = @"using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Functions;
using static StandardLibrary.Constants;

public static partial class Globals {
  public record class Base {
    public static Base DefaultInstance { get; } = new Base._DefaultBase();
    private Base() {}
    public Base(int p1) {
      this.p1 = p1;
    }
    public virtual int p1 { get; set; } = default;
    public virtual string asString() {

      return @$"""";
    }
    private record class _DefaultBase : Base {
      public _DefaultBase() { }
      public override int p1 => default;

      public override string asString() { return ""default Base"";  }
    }
  }
}

public static class Program {
  private static void Main(string[] args) {
    var m = new Base(3);
    printLine(m.p1);
  }
}";

        var parseTree = @"(file (main main (statementBlock (varDef var (assignableValue m) = (expression (newInstance (type Base) ( (argumentList (expression (value (literal (literalValue 3))))) )))) (callStatement (expression (methodCall printLine ( (argumentList (expression (expression (value m)) . p1)) ))))) end main) (classDef (mutableClass class Base (constructor constructor ( (parameterList (parameter p1 (type Int))) ) (statementBlock (assignment (assignableValue (nameQualifier self .) p1) = (expression (value p1)))) end constructor) (property property p1 (type Int)) (functionDef (functionWithBody function (functionSignature asString ( ) -> (type String)) statementBlock return (expression (value (literal (literalDataStructure """")))) end function)) end class)) <EOF>)";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertParseTreeIs(compileData, parseTree);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
        AssertObjectCodeExecutes(compileData, "3\r\n");
    }

    #endregion

    #region Fails

    [TestMethod, Ignore]
    public void Fail_ElanKeywordAsIdentier() {
        var code = @"
main
    var procedure = 1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    public void Fail_ElanKeywordWithChangedCase() {
        var code = @"
main
    var pRocedure = 1
end main
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    [TestMethod, Ignore]
    public void Fail_ElanKeywordTypeEvenWithChangedCase() {
        var code = @"
class Main 
    constructor()
    end constructor

    function asString() -> String
        return """"
    end function
end class
";
        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertDoesNotParse(compileData);
    }

    #endregion
}




public static partial class Globals {

    public static string? asString(int i) => "";

    public record class Base {
        public static Base DefaultInstance { get; } = new Base._DefaultBase();
        private Base() {}
        public Base(int p1) {
            this.p1 = p1;
        }
        public virtual int p1 { get; set; } = default;
        public virtual string asString() {

            return Globals.asString(p1);
        }
        private record class _DefaultBase : Base {
            public _DefaultBase() { }
            public override int p1 => default;

            public override string asString() { return "default Base";  }
        }
    }
}