﻿using System.Text.RegularExpressions;
using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using Compiler;
using CSharpLanguageModel.Models;
using SymbolTable;
using SymbolTable.Symbols;
using ValueType = AbstractSyntaxTree.ValueType;

namespace CSharpLanguageModel;

public static class CodeHelpers {
    public enum MethodType {
        SystemCall,
        Procedure,
        Function
    }

    private const int IndentSpaces = 2;

    private static readonly string[] CSharpKeywordsExceptElanKeywords = {
        "base",
        "break",
        "byte",
        "checked",
        "const",
        "continue",
        "delegate",
        "do",
        "double",
        "enum",
        "event",
        "explicit",
        "extern",
        "finally",
        "fixed",
        "goto",
        "implicit",
        "interface",
        "internal",
        "lock",
        "long",
        "namespace",
        "new",
        "null",
        "object",
        "operator",
        "out",
        "override",
        "params",
        "protected",
        "public",
        "readonly",
        "ref",
        "sbyte",
        "sealed",
        "short",
        "sizeof",
        "stackalloc",
        "static",
        "struct",
        "this",
        "throw",
        "typeof",
        "uint",
        "ulong",
        "unchecked",
        "unsafe",
        "ushort",
        "using",
        "virtual",
        "void",
        "volatile"
    };

    public static string Indent1 => Indent(1);

    public static Regex CSharpKeywordRegex { get; } = new($@"{{\s*({string.Join("|", CSharpKeywordsExceptElanKeywords)})\s*}}");

    public static string Indent(ICodeModel model, int indent) => model.ToString(indent);

    public static string Indent(int level) => new(' ', level * IndentSpaces);

    public static string AsLineSeparatedString(this IEnumerable<ICodeModel> mm, int indent = 0) {
        return string.Join("\r\n", mm.Select(cm => $"{cm.ToString(indent)}"));
    }

    public static string AsLineSeparatedString(this IEnumerable<ICodeModel> mm, string term = "", int indent = 0) {
        return string.Join("\r\n", mm.Select(cm => $"{cm.ToString(indent)}{term}"));
    }

    public static string AsLineSeparatedString(this IEnumerable<ICodeModel> mm, string prefix = "", string term = "", int indent = 0) {
        return string.Join("\r\n", mm.Select(cm => $"{Indent(indent)}{prefix}{cm}{term}"));
    }

    public static string AsCommaSeparatedString(this IEnumerable<ICodeModel> mm) => string.Join(", ", mm.Select(v => v.ToString(0)));

    public static string ValueTypeToCSharpType(ValueType type) =>
        type switch {
            ValueType.Int => "int",
            ValueType.String => "string",
            ValueType.Float => "double",
            ValueType.Char => "char",
            ValueType.Bool => "bool",
            _ => throw new NotImplementedException(type.ToString())
        };

    public static string DataStructureTypeToCSharpType(DataStructure type) =>
        type switch {
            DataStructure.Iter => "System.Collections.Generic.IEnumerable",
            DataStructure.List => "StandardLibrary.ElanList",
            DataStructure.Array => "StandardLibrary.ElanArray",
            DataStructure.Dictionary => "StandardLibrary.ElanDictionary",
            _ => throw new NotImplementedException(type.ToString())
        };

    private static string FunctionCallNodeToCSharpType(FunctionCallNode fcn, IScope currentScope) {
        var symbol = SymbolHelpers.ResolveMethodCall(currentScope, fcn).Item1 as FunctionSymbol;
        var symbolType = symbol?.ReturnType;
        var typeNode = SymbolHelpers.MapSymbolToTypeNode(symbolType, currentScope);
        return NodeToCSharpType(typeNode);
    }

    private static string NodeToCSharpType(IAstNode node) {
        return node switch {
            ValueNode vn => NodeToCSharpType(vn.TypeNode),
            LiteralListNode lln => $"StandardLibrary.ElanList<{NodeToCSharpType(lln.ItemNodes.First())}>",
            LiteralDictionaryNode ldn => $"StandardLibrary.ElanDictionary<{NodeToCSharpType(ldn.ItemNodes.First())}>",
            LiteralTupleNode ltn => $"({string.Join(", ", ltn.ItemNodes.Select(NodeToCSharpType))})",
            NewInstanceNode nin => $"{NodeToCSharpType(nin.Type)}",
            TypeNode tn => tn.Name,
            PairNode kn => $"{NodeToCSharpType(kn.Key)},{NodeToCSharpType(kn.Value)}",
            IdentifierWithTypeNode idt => NodeToCSharpType(idt.Type),
            ValueTypeNode vtn => ValueTypeToCSharpType(vtn.Type),
            EnumValueNode evn => NodeToCSharpType(evn.TypeNode),
            TupleTypeNode ttn => $"({string.Join(", ", ttn.Types.Select(NodeToCSharpType))})",
            _ => throw new NotImplementedException(node?.GetType().ToString() ?? "null")
        };
    }

    public static string NodeToCSharpType(IAstNode node, IScope currentScope) {
        return node switch {
            FunctionCallNode fcn => FunctionCallNodeToCSharpType(fcn, currentScope),
            _ => NodeToCSharpType(node)
        };
    }

    public static string NodeToPrefixedCSharpType(IAstNode node) {
        return node switch {
            ValueNode => $"const {NodeToCSharpType(node)}",
            LiteralListNode => $"static readonly {NodeToCSharpType(node)}",
            LiteralDictionaryNode => $"static readonly {NodeToCSharpType(node)}",
            LiteralTupleNode => $"static readonly {NodeToCSharpType(node)}",
            NewInstanceNode => $"static readonly {NodeToCSharpType(node)}",
            EnumValueNode => $"static readonly {NodeToCSharpType(node)}",
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

    public static string Prefix(string id) => CSharpKeywordsExceptElanKeywords.Contains(id) ? $"@{id}" : id;

    public static string DefaultInstance(int indent, ICodeModel type) => $"{Indent(indent + 1)}public static {type} DefaultInstance {{ get; }} = new {type}._Default{type}();";
}