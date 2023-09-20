using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using Compiler;
using CSharpLanguageModel.Models;
using StandardLibrary;

namespace CSharpLanguageModel;

public static class CodeHelpers {
    private const int IndentSpaces = 2;

    public static string Indent1 => Indent(1);

    public static string Indent(ICodeModel model, int indent) => model.ToString(indent);

    public static string Indent(int level) => new(' ', level * IndentSpaces);

    public static string AsLineSeparatedString(this IEnumerable<ICodeModel> mm, int indent = 0) {
        return string.Join("\r\n", mm.Select(cm => $"{cm.ToString(indent)}"));
    }

    public static string AsCommaSeparatedString(this IEnumerable<ICodeModel> mm) => string.Join(", ", mm.Select(v => v.ToString(0)));

    public static string NodeToCSharpType(IAstNode node) {
        return node switch {
            IntegerValueNode => "int",
            StringValueNode => "string",
            FloatValueNode => "double",
            CharValueNode => "char",
            BoolValueNode => "bool",
            _ => throw new NotImplementedException(node?.GetType().ToString() ?? "null")
        };
    }

    public static string OperatorToCSharpOperator(Operator op) {
        return op switch {
            Operator.Plus => "+",
            Operator.Minus => "-",
            Operator.Multiply => "*",
            Operator.IntDivide => "/",
            Operator.Modulus => "%",
            Operator.Equal => "==",
            Operator.Power => $"{typeof(Math).FullName}.{nameof(Math.Pow)}",
            Operator.Divide => $"{typeof(WrapperFunctions).FullName}.{nameof(WrapperFunctions.FloatDiv)}",
            Operator.And => "&&",
            Operator.Or => "||",
            Operator.Not => "!",
            _ => throw new NotImplementedException(op.ToString())
        };
    }
}