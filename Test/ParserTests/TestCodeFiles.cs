namespace Test.ParserTests;

[TestClass, Ignore]
public class TestCodeFiles {
    [TestMethod]
    public void Wordle() {
        var code = ReadInCodeFile("Wordle.elan");
        AssertParsesForRule(code, "file");
    }

    [TestMethod]
    public void MergeSort() {
        var code = ReadInCodeFile("mergeSort.elan");
        AssertParsesForRule(code, "file");
    }

    [TestMethod]
    public void BinarySearch() {
        var code = ReadInCodeFile("binarySearch.elan");
        AssertParsesForRule(code, "file");
    }

    [TestMethod]
    public void TempTestOfGenericType() {
        var code = @"
function binarySearch(list List<String>, item String) as Bool 
    -> let
            mid = list.len() div 2,
            value = list[mid]
        in
            if list.isEmpty() then false
            else if item == value then true
            else if item < value then binarySearch(list[0..mid], item)
            else binarySearch(list[mid + 1..], item)";
        AssertParsesForRule(code, "expressionFunction");
    }

    [TestMethod]
    public void Words_Game() {
        var code = ReadInCodeFile("words_Game.elan");
        AssertParsesForRule(code, "file");
    }

    [TestMethod]
    public void Words_main() {
        var code = ReadInCodeFile("words_main.elan");
        AssertParsesForRule(code, "file");
    }

    [TestMethod, Ignore]
    public void Words_procedures() {
        var code = ReadInCodeFile("words_procedures.elan");
        AssertParsesForRule(code, "file");
    }

    [TestMethod]
    public void Words_QueueOfTiles() {
        var code = ReadInCodeFile("words_QueueOfTiles.elan");
        AssertParsesForRule(code, "file");
    }

    [TestMethod]
    public void Words_Player() {
        var code = ReadInCodeFile("words_Player.elan");
        AssertParsesForRule(code, "file");
    }

    [TestMethod]
    public void Harshad() {
        var code = ReadInCodeFile("Harshad.elan");
        AssertParsesForRule(code, "file");
    }

    [TestMethod]
    public void DigitFrequencies() {
        var code = ReadInCodeFile("DigitFrequencies.elan");
        AssertParsesForRule(code, "file");
    }
}