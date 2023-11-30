using Compiler;

namespace Test.CompilerTests;

using static Helpers;

[TestClass]
public class T97_Wordle {
    #region Passes

    [TestMethod, Ignore]
    public void Pass_ConsoleUI() {
        var code = ReadElanSourceCodeFile("Wordle.elan");

        var objectCode = @"using System.Collections.Generic;
using StandardLibrary;
using static Globals;
using static StandardLibrary.Constants;

public static partial class Globals {
  public static readonly StandardLibrary.ElanList<string> allPossibleAnswers = new StandardLibrary.ElanList<string>(@$""ABACK"", @$""..."");
  public static readonly StandardLibrary.ElanList<string> validWords = new StandardLibrary.ElanList<string>(@$""ABACK"", @$""..."");
  public static bool isGreen(string attempt, string target, int n) {

    return target[n] == attempt[n];
  }
  public static string setChar(string word, int n, char newChar) {

    return word[..(n)] + newChar + word[(n + 1)..];
  }
  public static string setAttemptIfGreen(string attempt, string target, int n) {

    return Globals.isGreen(attempt, target, n) ? Globals.setChar(attempt, n, '*') : attempt;
  }
  public static string setTargetIfGreen(string attempt, string target, int n) {

    return Globals.isGreen(attempt, target, n) ? Globals.setChar(target, n, '.') : target;
  }
  public static bool isAlreadyMarkedGreen(string attempt, int n) {

    return attempt[n] == '*';
  }
  public static bool isYellow(string attempt, string target, int n) {

    return StandardLibrary.Functions.contains(target, attempt[n]);
  }
  public static string setAttemptIfYellow(string attempt, string target, int n) {

    return attempt[n] == '*' ? attempt : Globals.isYellow(attempt, target, n) ? Globals.setChar(attempt, n, '+') : Globals.setChar(attempt, n, '_');
  }
  public static string setTargetIfYellow(string attempt, string target, int n) {

    return Globals.isAlreadyMarkedGreen(attempt, n) ? target : Globals.isYellow(attempt, target, n) ? Globals.setChar(target, StandardLibrary.Functions.indexOf(target, attempt[n]), '.') : target;
  }
  public static (string, string) evaluateGreens(string attempt, string target) {

    return StandardLibrary.Functions.reduce(StandardLibrary.Functions.range(5), (attempt, target), (a, x) => (Globals.setAttemptIfGreen(a.attempt, a.target, x), Globals.setTargetIfGreen(a.attempt, a.target, x)));
  }
  public static (string, string) evaluateYellows(string attempt, string target) {

    return StandardLibrary.Functions.reduce(StandardLibrary.Functions.range(5), (attempt, target), (a, x) => (Globals.setAttemptIfYellow(a.attempt, a.target, x), Globals.setTargetIfYellow(a.attempt, a.target, x)));
  }
  public static string markAttempt(string attempt, string target) {
    var (attemptAfterGreens, targetAfterGreens) = Globals.evaluateGreens(attempt, target);
    return Globals.evaluateYellows(attemptAfterGreens, targetAfterGreens)[0];
  }
  public static System.Collections.Generic.IEnumerable<string> possibleAnswersAfterAttempt(System.Collections.Generic.IEnumerable<string> prior, string attempt, string mark) {

    return StandardLibrary.Functions.filter(prior, (w) => Globals.markAttempt(attempt, w) == mark);
  }
  public static int wordCountRemainingAfterAttempt(System.Collections.Generic.IEnumerable<string> possibleAnswers, string attempt) {
    var groups = StandardLibrary.Functions.groupBy(possibleAnswers, (w) => Globals.markAttempt(attempt, w));
    return StandardLibrary.Functions.max(groups, (g) => StandardLibrary.Functions.count(g));
  }
  public static System.Collections.Generic.IEnumerable<(string, int)> allRemainingWordCounts(StandardLibrary.ElanList<string> possAnswers, System.Collections.Generic.IEnumerable<string> possAttempts) {

    return StandardLibrary.Functions.map(StandardLibrary.Functions.asParallel(possAttempts), (w) => (w, Globals.wordCountRemainingAfterAttempt(possAnswers, w)));
  }
  public static (string, int) betterOf((string, int) word1, (string, int) word2, System.Collections.Generic.IEnumerable<string> possAnswers) {
    var (w1, w1Count) = word1;
    var (w2, w2Count) = word2;
    var isBetter = w2Count < w1Count;
    var isEqualAndPossAnswer = w2Count == w1Count && StandardLibrary.Functions.contains(possAnswers, w2);
    return isBetter || isEqualAndPossAnswer ? word2 : word1;
  }
  public static string bestAttempt(StandardLibrary.ElanList<string> possAnswers, StandardLibrary.ElanList<string> possAttempts) {
    var wordCounts = Globals.allRemainingWordCounts(possAnswers, possAttempts);
    return StandardLibrary.Functions.reduce(wordCounts, (bestSoFar, newWord) => Globals.betterOf(bestSoFar, newWord, possAnswers))[0];
  }
}

public static class Program {
  private static void Main(string[] args) {
    var possible = allPossibleAnswers;
    var marking = @$"""";
    var attempt = @$""RAISE"";
    while (marking != @$""*****"") {
      System.Console.WriteLine(StandardLibrary.Functions.asString(attempt));
      marking = ((Func<string>)(() => { System.Console.Write(""Please mark the attempt""); return Console.ReadLine() ?? """";}))();
      possible = StandardLibrary.Functions.asList(Globals.possibleAnswersAfterAttempt(possible, attempt, marking));
      attempt = Globals.bestAttempt(possible, validWords);
    }
  }
}";

        var compileData = Pipeline.Compile(new CompileData { ElanCode = code });
        AssertParses(compileData);
        AssertCompiles(compileData);
        AssertObjectCodeIs(compileData, objectCode);
        AssertObjectCodeCompiles(compileData);
    }

    #endregion

    #region Fails

    #endregion
}