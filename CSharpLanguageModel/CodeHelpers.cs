using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using Compiler;
using CSharpLanguageModel.Models;
using StandardLibrary;
using ValueType = AbstractSyntaxTree.ValueType;

namespace CSharpLanguageModel;

public static class CodeHelpers {
    public enum MethodType {
        SystemCall, 
        Procedure, 
        Function
    }



    private const int IndentSpaces = 2;

    public static string Indent1 => Indent(1);

    public static string Indent(ICodeModel model, int indent) => model.ToString(indent);

    public static string Indent(int level) => new(' ', level * IndentSpaces);

    public static string AsLineSeparatedString(this IEnumerable<ICodeModel> mm, int indent = 0) {
        return string.Join("\r\n", mm.Select(cm => $"{cm.ToString(indent)}"));
    }

    public static string AsCommaSeparatedString(this IEnumerable<ICodeModel> mm) => string.Join(", ", mm.Select(v => v.ToString(0)));

    public static string AsCommaSeparatedString(this IEnumerable<ICodeModel> mm, string prefix) => string.Join(", ", mm.Select(v => $"{prefix}{v}"));

    public static string ValueTypeToCSharpType(ValueType type) =>
        type switch {
            ValueType.Int => "int",
            ValueType.String => "string",
            ValueType.Float => "double",
            ValueType.Char => "char",
            ValueType.Bool => "bool",
            _ => throw new NotImplementedException(type.ToString() ?? "null")
        };

    public static string DataStructureTypeToCSharpType(DataStructure type) =>
        type switch {
            DataStructure.List => "List",
            DataStructure.Array => "Array",
            _ => throw new NotImplementedException(type.ToString() ?? "null")
        };

    // todo fix cast
    public static string ValueNodeToCSharpType(ValueNode node) => ValueTypeToCSharpType(((ValueTypeNode)node.TypeNode).Type);

    public static string NodeToCSharpType(IAstNode node) {
        return node switch {
            ValueNode vn => ValueNodeToCSharpType(vn),
            LiteralListNode lln => $"StandardLibrary.List<{NodeToCSharpType(lln.ItemNodes.First())}>",
            _ => throw new NotImplementedException(node?.GetType().ToString() ?? "null")
        };
    }

    public static string NodeToPrefixedCSharpType(IAstNode node) {
        return node switch {
            ValueNode => $"const {NodeToCSharpType(node)}",
            LiteralListNode => $"static readonly {NodeToCSharpType(node)}",
            _ => throw new NotImplementedException(node?.GetType().ToString() ?? "null")
        };
    }

    public static (string, bool isFunc) OperatorToCSharpOperator(Operator op) {
        return op switch {
            Operator.Plus => ("+", false),
            Operator.Minus => ("-", false),
            Operator.Multiply => ("*", false),
            Operator.IntDivide => ("/", false),
            Operator.Modulus => ("%", false),    
            Operator.Equal => ("==", false),
            Operator.Power => ($"{typeof(Math).FullName}.{nameof(Math.Pow)}", true),
            Operator.Divide => ($"{typeof(WrapperFunctions).FullName}.{nameof(WrapperFunctions.FloatDiv)}", true),
            Operator.And => ("&&", false),
            Operator.Or => ("||", false),
            Operator.Not => ("!", false),
            Operator.Xor => ("^", false),
            Operator.LessThan => ("<", false),
            Operator.GreaterThan => (">", false),
            Operator.GreaterThanEqual => (">=", false),
            Operator.LessThanEqual => ("<=", false),
            Operator.NotEqual => ("!=", false),
            _ => throw new NotImplementedException(op.ToString())
        };
    }
}