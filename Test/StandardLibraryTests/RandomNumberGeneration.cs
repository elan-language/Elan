using static StandardLibrary.SystemCalls;

namespace Test.StandardLibraryTests;

[TestClass]
public class RandomNumberGeneration {
    [TestMethod]
    public void SeededGenerator() {
        seedRandom(3);
        Assert.AreEqual(0.2935192125353586, random());
        Assert.AreNotEqual(0.2935192125353586, random());
        Assert.AreEqual(0.8649866608274107, random());
    }

    [TestMethod]
    public void MaxFloat() {
        seedRandom(3);
        Assert.AreEqual(293.51921253535863, random(1000.0));
        Assert.AreEqual(697.5812123611482, random(1000.0));
    }

    [TestMethod]
    public void RangeFloat() {
        seedRandom(3);
        Assert.AreEqual(129.351921253535863, random(100.0, 200.0));
        Assert.AreEqual(169.75812123611482, random(100.0, 200.0));
    }

    [TestMethod]
    public void MaxInt() {
        seedRandom(3);
        Assert.AreEqual(2, random(6));
        Assert.AreEqual(5, random(6));
    }

    [TestMethod]
    public void RangeInt() {
        seedRandom(3);
        Assert.AreEqual(7, random(5, 10));
        Assert.AreEqual(9, random(5, 10));
    }

    [TestMethod]
    public void NonSeededRandom() //This test could in theory fail, but only rarely!
    {
        seedRandom(3);
        Assert.AreEqual(293.51921253535863, random(1000.0));
        resetRandom();
        Assert.AreNotEqual(697.5812123611482, random(1000.0));
    }
}