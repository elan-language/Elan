using System.Collections.Immutable;
using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;

namespace Compiler;

public static class CompilerTransforms {
    public static IAstNode? TransformSystemCallNodes(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is MethodCallNode mcn && currentScope.Resolve(mcn.Name) is SystemCallSymbol scs) {
            return new SystemCallNode(mcn.Id, mcn.Parameters);
        }

        return null;
    }
}