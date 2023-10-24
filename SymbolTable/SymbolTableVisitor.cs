using AbstractSyntaxTree.Nodes;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;

namespace SymbolTable;

public class SymbolTableVisitor {
    private IScope currentScope;

    public SymbolTableVisitor() => currentScope = SymbolTable.GlobalScope;

    public SymbolTableImpl SymbolTable { get; } = new();

    public IAstNode Visit(IAstNode astNode) {
        return astNode switch {
            MainNode n => VisitMainNode(n),
            ProcedureDefNode n => VisitProcedureDefNode(n),
            FunctionDefNode n => VisitFunctionDefNode(n),
            ClassDefNode n => VisitClassDefNode(n),
            null => throw new NotImplementedException("null"),
            _ => VisitChildren(astNode)
        };
    }

    private IAstNode VisitProcedureDefNode(ProcedureDefNode procedureDefNode) {
        var name = procedureDefNode.Signature switch {
            MethodSignatureNode { Id: IdentifierNode idn } => idn.Id,
            _ => throw new NotImplementedException("null")
        };

        var ms = new ProcedureSymbol(name, currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(procedureDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return procedureDefNode;
    }

    private IAstNode VisitFunctionDefNode(FunctionDefNode functionDefNode) {
        var name = functionDefNode.Signature switch {
            MethodSignatureNode { Id: IdentifierNode idn } => idn.Id,
            _ => throw new NotImplementedException("null")
        };

        ISymbolType rt = null; // todo

        var ms = new FunctionSymbol(name, rt, currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(functionDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return functionDefNode;
    }

    private IAstNode VisitMainNode(MainNode mainNode) {
        var ms = new ProcedureSymbol("main", currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(mainNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return mainNode;
    }

    private IAstNode VisitClassDefNode(ClassDefNode classDefNode) {
        var ms = new ClassSymbol(classDefNode.Name, currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(classDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return classDefNode;
    }

    private IAstNode VisitChildren(IAstNode node) {
        foreach (var child in node.Children) {
            Visit(child);
        }

        return node;
    }
}