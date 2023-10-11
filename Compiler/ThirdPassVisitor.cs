using AbstractSyntaxTree.Nodes;
using SymbolTable;

namespace Compiler;

public class ThirdPassVisitor {
    public readonly IList<string> CompileErrors = new List<string>();
    private IScope currentScope;

    static ThirdPassVisitor() {
        Rules.Add(CompilerRules.ExpressionMustBeAssignedRule);
        Rules.Add(CompilerRules.SystemCallMustBeAssignedRule);
        Rules.Add(CompilerRules.CannotMutateControlVariableRule);
        Rules.Add(CompilerRules.ArrayInitialization);
        Rules.Add(CompilerRules.FunctionConstraintsRule);
        Rules.Add(CompilerRules.MethodCallsShouldBeResolvedRule);
    }

    public ThirdPassVisitor(SymbolTableImpl symbolTable) {
        SymbolTable = symbolTable;
        currentScope = symbolTable.GlobalScope;
    }

    private SymbolTableImpl SymbolTable { get; }
    private static IList<Func<IAstNode[], IScope, string?>> Rules { get; } = new List<Func<IAstNode[], IScope, string?>>();

    private void Enter(IAstNode node) {
        switch (node) {
            case MainNode:
                currentScope = currentScope.Resolve("main") as IScope ?? throw new ArgumentNullException();
                break;
        }
    }

    private void Exit(IAstNode node) {
        if (node is MainNode) {
            currentScope = currentScope.EnclosingScope ?? throw new ArgumentNullException();
        }
    }

    private void ApplyRules(IAstNode[] nodes, IScope scope) {
        foreach (var rule in Rules) {
            if (rule(nodes, scope) is { } e) {
                CompileErrors.Add(e);
            }
        }
    }

    private void Visit(IAstNode[] nodeHierarchy, Action<IAstNode[], IScope> action) {
        var currentNode = nodeHierarchy.Last();

        Enter(currentNode);
        try {
            action(nodeHierarchy, currentScope);
            foreach (var child in currentNode.Children) {
                var tree = nodeHierarchy.Append(child).ToArray();
                Visit(tree, action);
            }
        }
        finally {
            Exit(currentNode);
        }
    }

    private void Visit(IAstNode[] nodeHierarchy) {
        Visit(nodeHierarchy, ApplyRules);
    }

    public IAstNode Visit(IAstNode ast) {
        Visit(new[] { ast });
        return ast;
    }
}