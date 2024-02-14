using System.Collections.Immutable;
using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;

namespace Compiler;

public static class CompilerTransforms {
    #region rules

    public static IAstNode? TransformLiteralListNodes(IAstNode[] nodes, IScope currentScope) =>
        nodes.Last() switch {
            LiteralListNode lln when lln.ItemNodes.First() is IdentifierNode idn and not IdentifierWithTypeNode => lln.Replace(idn, SymbolHelpers.TypeIdentifier(idn, currentScope)),
            _ => null
        };

    public static IAstNode? TransformProcedureParameterNodes(IAstNode[] nodes, IScope currentScope) {
        static IAstNode? TransformProcedureCallNode(ProcedureCallNode pcn, IScope scope) {
            var procedureScope = SymbolHelpers.ResolveMethodCall(scope, pcn).Item1 as ProcedureSymbol ?? SymbolHelpers.GetProcedure(pcn, scope); // may be no such procedure

            var parameterNodes = pcn.Parameters.Where(p => p is not ParameterCallNode).ToArray();

            if (procedureScope is not null && parameterNodes.Any()) {
                var transformedParameters = pcn.Parameters.Select((p, i) => TransformParameter(p, i, procedureScope));
                return pcn with { Parameters = transformedParameters.ToImmutableArray() };
            }

            return null;
        }

        static IAstNode TransformParameter(IAstNode p, int i, ProcedureSymbol scope) {
            // avoid failed with mismatched parameter counts
            if (i >= scope.ParameterNames.Length) {
                return new ParameterCallNode(p, false, 0, 0);
            }

            var matchingParm = scope.ParameterNames[i];

            var symbol = scope.Resolve(matchingParm) as ParameterSymbol ?? throw new NullReferenceException();
            return new ParameterCallNode(p, symbol.ByRef, 0, 0);
        }

        return nodes.Last() switch {
            ProcedureCallNode pcn => TransformProcedureCallNode(pcn, currentScope),
            _ => null
        };
    }

    #endregion
}