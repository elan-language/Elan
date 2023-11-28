using StandardLibrary;
using static StandardLibrary.Functions;

namespace Test.CompilerTests;

[TestClass]
public class FunctionsTests {
    [TestMethod]
    public void newLines() {
        Assert.AreEqual(@"


", newlines(3));
    }

    [TestMethod]
    public void testAsUpperCase() {
        Assert.AreEqual("HELLO WORLD!", asUpperCase("hello World!"));
    }

    [TestMethod]
    public void testAsLowerCase() {
        Assert.AreEqual("hello world!", asLowerCase("hello World!"));
    }

    [TestMethod]
    public void testIsBefore() {
        Assert.IsTrue(isBefore("a", "b"));
        Assert.IsTrue(isBefore("a", "aa"));
        Assert.IsTrue(isBefore("a", "a_"));
        Assert.IsFalse(isBefore("b", "a_"));
        Assert.IsFalse(isBefore("a", "a"));
    }

    [TestMethod]
    public void testIsBeforeOrSameAs() {
        Assert.IsTrue(isBeforeOrSameAs("a", "b"));
        Assert.IsTrue(isBeforeOrSameAs("a", "aa"));
        Assert.IsTrue(isBeforeOrSameAs("a", "a"));
        Assert.IsFalse(isBeforeOrSameAs("b", "a_"));
        Assert.IsTrue(isBeforeOrSameAs("a", "a"));
    }

    [TestMethod]
    public void testIsAfter() {
        Assert.IsTrue(isAfter("b", "a"));
        Assert.IsTrue(isAfter("bb", "b"));
        Assert.IsTrue(isAfter("b_", "b"));
        Assert.IsFalse(isAfter("b", "c"));
        Assert.IsFalse(isAfter("b", "b"));
    }

    [TestMethod]
    public void testIsAfterOrSameAs() {
        Assert.IsTrue(isAfterOrSameAs("b", "a"));
        Assert.IsTrue(isAfterOrSameAs("bb", "b"));
        Assert.IsTrue(isAfterOrSameAs("b_", "b"));
        Assert.IsFalse(isAfterOrSameAs("b", "c"));
        Assert.IsTrue(isAfterOrSameAs("b", "b"));
    }

    #region asString

    [TestMethod]
    public void AsStringBoolean() {
        var x = true;
        Assert.AreEqual("true", asString(x));
    }

    [TestMethod]
    public void AsStringChar() {
        var x = 'x';
        Assert.AreEqual("x", asString(x));
    }

    [TestMethod]
    public void AsStringString() {
        var x = "Hello";
        Assert.AreEqual("Hello", asString(x));
    }

    [TestMethod]
    public void AsStringList() {
        var t = new ElanList<int>(1, 2, 3, 4, 5);
        Assert.AreEqual("List {1,2,3,4,5}", asString(t));
    }

    [TestMethod]
    public void AsStringEmptyList() {
        var t = new ElanList<int>();
        Assert.AreEqual("empty list", asString(t));
    }

    [TestMethod]
    public void AsStringArray() {
        var a = new ElanArray<bool> { true, false, false };
        Assert.AreEqual("Array {true,false,false}", asString(a));
    }

    [TestMethod]
    public void AsStringArray2() {
        var a = new ElanArray<bool>(4);
        Assert.AreEqual("Array {false,false,false,false}", asString(a));
    }

    [TestMethod]
    public void AsStringArray3() {
        var t = new ElanArray<string> { "", "", "" };
        Assert.AreEqual("Array {,,}", asString(t));
    }

    [TestMethod]
    public void AsStringEmptyArray() {
        var t = new ElanArray<string>();
        Assert.AreEqual("empty array", asString(t));
    }

    [TestMethod]
    public void AsStringDictionary() {
        var t = new ElanDictionary<char, int>(('a', 1), ('b', 2));
        Assert.AreEqual("Dictionary {a:1,b:2}", asString(t));
    }

    [TestMethod]
    public void AsStringTuple() {
        var t = (123, "Richard", 4.3, false, 'x');
        Assert.AreEqual("(123, Richard, 4.3, false, x)", asString(t));
    }

    [TestMethod]
    public void AsStringTuple2() {
        var t = (true, false);
        Assert.AreEqual("(true, false)", asString(t));
    }

    [TestMethod]
    public void AsStringFunc() {
        Func<int, string, int> t = (i, s) => 0; 
        Assert.AreEqual("(Int,String -> Int)", asString(t));
    }

    [TestMethod]
    public void AsStringFunc1() {
        Func<int, Foo> t = i => Foo.DefaultInstance; 
        Assert.AreEqual("(Int -> Foo)", asString(t));
    }

    public record class Foo {
        public Foo() {
            p1 = 5;
            p2 = @"Apple";
        }

        public static Foo DefaultInstance { get; } = new _DefaultFoo();

        public virtual int p1 { get; set; }
        protected virtual string p2 { get; set; } = "";
        public virtual string asString() => typeAndProperties(this);

        private record class _DefaultFoo : Foo {
            public override int p1 => default;
            protected override string p2 => "";

            public override string asString() => "default Foo";
        }
    }

    [TestMethod]
    public void TypeAndProperties() {
        Assert.AreEqual("Foo {p1:5, p2:Apple}", typeAndProperties(new Foo()));
    }

    [TestMethod]
    public void Iter() {
        IEnumerable<int> a = new [] { 1, 2, 3, 5, 6, 7, 8, 9, 10 };

        Assert.AreEqual("6", Functions.asString(Functions.element(a, 5)));
        Assert.AreEqual("1", Functions.asString(Functions.head(a)));
        Assert.AreEqual("Iter {2,3,5,6,7,8,9,10}", Functions.asString(Functions.tail(a)));
        Assert.AreEqual("Iter {5,6,7,8}", Functions.asString(Functions.range(a, 3, 7)));
        Assert.AreEqual("Iter {6,7,8,9,10}", Functions.asString(Functions.rangeFrom(a, 4)));
        Assert.AreEqual("Iter {1,2,3,5,6,7,8}", Functions.asString(Functions.rangeTo(a, 7)));
    }



    #endregion
}