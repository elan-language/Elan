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
        var t = new StandardLibrary.ElanList<int>(1, 2, 3, 4, 5);
        Assert.AreEqual("List {1,2,3,4,5}", asString(t));
    }

    [TestMethod]
    public void AsStringEmptyList() {
        var t = new StandardLibrary.ElanList<int>();
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

    #endregion
}