namespace Test.ParserTests;

[TestClass, Ignore]
public class Functions {
    private const string file = "file";

    [TestMethod]
    public void HappyCase1() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String 
    return word[..n] + newChar + word[n+1..]
end function
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void HappyCase2NoParams() {
        var code = @"
function setChar() as String 
    return word[..n] + newChar + word[n+1..]
end function
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void HappyCase3Overloaded() {
        var code = @"
function setChar() as String 
    return word[..n] + newChar + word[n+1..]
end function

function setChar(n Int) as String 
    return word[..n] + newChar + word[n+1..]
end function
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void HappyCase4WithControlFlow() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String 
    var a = 0
    repeat
      a = a + 1
    until a >= 5
    return word[..n] + newChar + word[n+1..]
end function
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void MisspelledFunction() {
        var code = @"
funciton setChar(word String, n Int, newChar Char) as String 
   return word[..n] + newChar + word[n+1..]
end function
";
        AssertDoesNotParseForRule(code, file, @"line 2:0 no viable alternative");
    }

    [TestMethod]
    public void InvalidName() {
        var code = @"
function SetChar(word String, n Int, newChar Char) as String 
   return word[..n] + newChar + word[n+1..]
end function
";
        AssertDoesNotParseForRule(code, file); //TODO: currently parses OK because casing rules relaxed at Parser pass, to be enforced in later pass.
    }

    [TestMethod]
    public void InvalidReturnTypeCasing() {
        var code = @"
function setChar(word String, n Int, newChar Char) as string
   return word[..n] + newChar + word[n+1..]
end function
";
        AssertDoesNotParseForRule(code, file); //See above
    }

    [TestMethod]
    public void ParameterNameInvalid() {
        var code = @"
function setChar(word, n Int, new-Char Char) as String 
    return word[..n] + newChar + word[n+1..]
end function
";
        AssertDoesNotParseForRule(code, file, @"line 2:21 no viable alternative at input");
    }

    [TestMethod]
    public void ParameterTypeMissing() {
        var code = @"
function setChar(word, n Int, newChar Char) as String 
    return word[..n] + newChar + word[n+1..]
end function
";
        AssertDoesNotParseForRule(code, file, @"line 2:21 no viable alternative at input");
    }

    [TestMethod]
    public void ParameterCommaMissing() {
        var code = @"
function setChar(word String n Int, newChar Char) as String 
    return word[..n] + newChar + word[n+1..]
end function
";
        AssertDoesNotParseForRule(code, file, @"line 2:29 no viable alternative");
    }

    [TestMethod]
    public void NoAsClause() {
        var code = @"
function setChar(word String, n Int, newChar Char)
   return word[..n] + newChar + word[n+1..]
end function
";
        AssertDoesNotParseForRule(code, file, @"line 3:3 no viable alternative");
    }

    [TestMethod]
    public void NoEndFunction1() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String 
   return word[..n] + newChar + word[n+1..]

";
        AssertDoesNotParseForRule(code, file, @"line 5:0 mismatched input '<EOF>' expecting 'end'");
    }

    [TestMethod] [Ignore("Failing on appveyor")]
    public void NoEndFunction2() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String 
   return word[..n] + newChar + word[n+1..]
end
";
        AssertDoesNotParseForRule(code, file, @"line 4:3 missing 'function' at '\r\n'");
    }

    [TestMethod]
    public void NoEndFunction3() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String 
   return word[..n] + newChar + word[n+1..]
end while
";
        AssertDoesNotParseForRule(code, file, @"line 4:4 mismatched input 'while' expecting 'function'");
    }

    [TestMethod]
    public void NoReturn1() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String 
   var a = word[..n] + newChar + word[n+1..]
end function
";
        AssertDoesNotParseForRule(code, file, @"line 4:0 mismatched input 'end' expecting 'return'");
    }

    [TestMethod]
    public void NoReturn2() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String 
end function
";
        AssertDoesNotParseForRule(code, file, @"line 3:0 no viable alternative at input");
    }

    [TestMethod]
    public void ReturnIsNotLastStatement() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String
    var a = """"
    return a
    var b = 3
end function
";
        AssertDoesNotParseForRule(code, file, @"line 5:4 mismatched input 'var'");
    }

    #region Expression functions

    [TestMethod]
    public void ExpressionFunctionHappyCase() {
        var code = @"
function foo(word String, n Int, newChar Char) as String -> 
    word[..n] + newChar + word[n+1..]
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void ExpressionFunctionWithLet1() {
        var code = @"
function foo(a Float, b Float) as Float -> let c = a + b in c*c
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void ExpressionFunctionWithLet2() {
        var code = @"
function foo(a Float, b Float) as Float -> let c = a + b, d = a - b in c*c + d*d
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void PreviousTesAsStatementFunction() {
        var code = @"
function setChar(a Float, b Float) as Float
  var c = a + b
  var d = a - b
  return c*c + d*d
end function
";
        AssertParsesForRule(code, file);
    }

    [TestMethod]
    public void ExpressionFunctionNoArrow() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String 
    word[..n] + newChar + word[n+1..]
";
        AssertDoesNotParseForRule(code, file, @"line 4:0 mismatched input '<EOF");
    }

    [TestMethod]
    public void ExpressionFunctionUnnecessaryReturn() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String ->
    return word[..n] + newChar + word[n+1..]
";
        AssertDoesNotParseForRule(code, file, @"line 3:4 no viable alternative");
    }

    #endregion

    #region Non-syntactical compile errors (pending implementation of other layers)

    [TestMethod]
    public void AttemptingToAssignParamater() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String 
   n = n + 1
   return word
end function
";
        AssertDoesNotParseForRule(code, file, @""); //TODO: should give compile error
    }

    [TestMethod]
    public void UsingAVariableThatIsNotInScope() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String 
   x = 5
   return word
end function
";
        AssertDoesNotParseForRule(code, file, @""); //TODO: should give compile error
    }

    [TestMethod]
    public void ReturnIncompatibleType() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String 
   return n
end function
";
        AssertDoesNotParseForRule(code, file, @""); //TODO: should give compile error
    }

    [TestMethod]
    public void InnerFunction() {
        var code = @"
function setChar(word String, n Int, newChar Char) as String 
    function changeChar(word String, n Int, newChar Char) as String 
        return word
    end function
   return word
end function
";
        AssertDoesNotParseForRule(code, file, @"line 3:4 no viable alternative");
    }

    [TestMethod]
    public void DuplicateSignature() {
        var code = @"
function foo(int n) as Int 
    return n
end function

function foo(int n) as String 
    return n.asString()
end function
";
        AssertDoesNotParseForRule(code, file); //Should give compile error
    }

    #endregion
}