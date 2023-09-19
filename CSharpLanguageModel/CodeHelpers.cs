using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using CSharpLanguageModel.Models;

namespace CSharpLanguageModel;

public static class CodeHelpers {
    private const int IndentSpaces = 2;

    public static string Indent1 => Indent(1);

    public static string Indent(int level) => new(' ', level * IndentSpaces);

    public static string AsLineSeparatedString(this IEnumerable<ICodeModel> mm, int level = 0, string prefix = "") {
        var indentation = Indent(level);
        prefix = string.IsNullOrWhiteSpace(prefix) ? "" : $"{prefix} ";
        return string.Join("\r\n", mm.Select(cm => $"{indentation}{prefix}{cm.AsCode()}")).TrimEnd();
    }

    public static string AsCommaSeparatedString(this IEnumerable<ICodeModel> mm) => string.Join(", ", mm.Select(v => v.ToString())).Trim();

    public static string AsCode(this ICodeModel? cm) => (cm?.ToString() ?? "").Trim();

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
            Operator.Divide => "/",
            Operator.Mod => "%",
            Operator.Power => "System.Math.Pow",
            _ => throw new NotImplementedException(op.ToString())
        };
    }
}