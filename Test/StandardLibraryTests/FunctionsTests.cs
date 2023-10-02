using System.Collections.Immutable;
using StandardLibrary;
using System.Linq.Expressions;
using static StandardLibrary.Functions;

namespace Test.CompilerTests;

[TestClass]
public class FunctionsTests
{
    #region asString
    [TestMethod]
    public void AsStringBoolean()
    {
        var x = true;
        Assert.AreEqual("true", asString(x));
    }

    [TestMethod]
    public void AsStringChar()
    {
        var x = 'x';
        Assert.AreEqual("x", asString(x));
    }

    [TestMethod]
    public void AsStringString()
    {
        var x = "Hello";
        Assert.AreEqual("Hello", asString(x));
    }

    [TestMethod]
    public void AsStringList()
    {
        var t = new StandardLibrary. List<int>(1, 2, 3, 4, 5 );
        Assert.AreEqual("List {1,2,3,4,5}", asString(t));
    }

    [TestMethod]
    public void AsStringEmptyList()
    {
        var t = new StandardLibrary.List<int>();
        Assert.AreEqual("empty list", asString(t));
    }

    //[TestMethod]
    //public void AsStringArray()
    //{
    //    var a = new bool[] { true, false, false };
    //    Assert.AreEqual("Array {true,false,false}", asString(a));
    //}

    //[TestMethod]
    //public void AsStringArray2()
    //{
    //    var a = new bool[4];
    //    Assert.AreEqual("Array {false,false,false,false}", asString(a));
    //}
    //[TestMethod]
    //public void AsStringArray3()
    //{
    //    var t = new string[] {"","",""};
    //    Assert.AreEqual("Array {,,}", asString(t));
    //}

    //[TestMethod]
    //public void AsStringEmptyArray()
    //{
    //    var t = new string[] {};
    //    Assert.AreEqual("empty array", asString(t));
    //}

    [TestMethod]
    public void AsStringTuple()
    {
        var t = (123, "Richard", 4.3, false, 'x');
        Assert.AreEqual("(123, Richard, 4.3, false, x)", asString(t));
    }

    [TestMethod]
    public void AsStringTuple2()
    {
        var t = (true, false);
        Assert.AreEqual("(true, false)", asString(t));
    }

    #endregion
}

