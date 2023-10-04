using AbstractSyntaxTree.Nodes;
using SymbolTable;

namespace Compiler;

public class SecondPassVisitor {
    private IScope currentScope;

    static SecondPassVisitor() {
        Transforms.Add(CompilerTransforms.TransformSystemCallNodes);
    }

    public SecondPassVisitor(SymbolTableImpl symbolTable) {
        SymbolTable = symbolTable;
        currentScope = symbolTable.GlobalScope;
    }

    private SymbolTableImpl SymbolTable { get; }
    private static IList<Func<IAstNode[], IScope, IAstNode?>> Transforms { get; } = new List<Func<IAstNode[], IScope, IAstNode?>>();

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

    private IAstNode InsertIntoTree(IAstNode oldNode, IAstNode newNode, IAstNode[] parentNodes) {
        if (parentNodes.Any()) {
            var parent = parentNodes.Last();
            var newParent = parent.Replace(oldNode, newNode);
            return InsertIntoTree(parent, newParent, parentNodes.SkipLast(1).ToArray());
        }

        return newNode;
    }

    private IAstNode ApplyTransform(IAstNode[] nodes, IScope scope, Func<IAstNode[], IScope, IAstNode?> transform, IAstNode newRoot) {
        var transformed = transform(nodes, scope);
        if (transformed is not null) {
            newRoot = InsertIntoTree(nodes.Last(), transformed, nodes.SkipLast(1).ToArray());
            return newRoot;
        }

        return newRoot;
    }

    private IAstNode ApplyTransforms(IAstNode[] nodes, IScope scope, IAstNode newRoot) {
        foreach (var transform in Transforms) {
            newRoot = ApplyTransform(nodes, scope, transform, newRoot);
        }

        return newRoot;
    }

    private IAstNode Visit(IAstNode[] nodeHierarchy, IAstNode newRoot) {
        var currentNode = nodeHierarchy.Last();

        Enter(currentNode);
        try {
            newRoot = ApplyTransforms(nodeHierarchy, currentScope, newRoot);
            foreach (var child in currentNode.Children) {
                var tree = nodeHierarchy.Append(child).ToArray();
                newRoot = Visit(tree, newRoot);
            }
        }
        finally {
            Exit(currentNode);
        }

        return newRoot;
    }

    public IAstNode Visit(IAstNode ast) {
        return Visit(new[] { ast }, ast);
    }
}