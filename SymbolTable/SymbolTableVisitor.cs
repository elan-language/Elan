using AbstractSyntaxTree.Nodes;
using SymbolTable.Symbols;

namespace SymbolTable; 

public class SymbolTableVisitor {
    private IScope currentScope;

    public SymbolTableVisitor() => currentScope = SymbolTable.GlobalScope;

    public SymbolTableImpl SymbolTable { get; } = new();

    public IAstNode Visit(IAstNode astNode) {
        return astNode switch {
            MainNode bn => VisitMainNode(bn),
            null => throw new NotImplementedException("null"),
            _ => VisitChildren(astNode)
        };
    }

    private IAstNode VisitMainNode(MainNode mainNode) {
        var ms = new ProcedureSymbol("main", currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(mainNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return mainNode;
    }

    private IAstNode VisitChildren(IAstNode node) {
        foreach (var child in node.Children) {
            Visit(child);
        }

        return node;
    }
}