using AbstractSyntaxTree.Nodes;
using AbstractSyntaxTree.Roles;
using SymbolTable;

namespace Compiler;

public class SecondPassVisitor {
    static SecondPassVisitor() {
        Transforms.Add(CompilerTransforms.TransformLiteralListNodes);
        Transforms.Add(CompilerTransforms.TransformProcedureParameterNodes);
    }

    public SecondPassVisitor(SymbolTableImpl symbolTable) => SymbolTable = symbolTable;

    private SymbolTableImpl SymbolTable { get; }
    private static IList<Func<IAstNode[], IScope, IAstNode?>> Transforms { get; } = new List<Func<IAstNode[], IScope, IAstNode?>>();

    private static string SignatureId(IAstNode signature) =>
        signature switch {
            MethodSignatureNode { Id: IdentifierNode idn } => idn.Id,
            _ => throw new NotImplementedException()
        };

    private static IScope Enter(IAstNode node, IScope currentScope) =>
        node switch {
            IHasScope hs => currentScope.Resolve(hs.Name) as IScope ?? throw new ArgumentNullException(),
            _ => currentScope
        };

    private static IScope Exit(IAstNode node, IScope currentScope) =>
        node switch {
            IHasScope => currentScope.EnclosingScope ?? throw new ArgumentNullException(),
            _ => currentScope
        };

    private static IAstNode InsertIntoTree(IAstNode oldNode, IAstNode newNode, IAstNode[] parentNodes) {
        if (parentNodes.Any()) {
            var parent = parentNodes.Last();
            var newParent = parent.Replace(oldNode, newNode);
            return InsertIntoTree(parent, newParent, parentNodes.SkipLast(1).ToArray());
        }

        return newNode;
    }

    private static IAstNode? ApplyTransform(IAstNode[] nodes, IScope scope, Func<IAstNode[], IScope, IAstNode?> transform) {
        var transformed = transform(nodes, scope);
        return transformed is not null ? InsertIntoTree(nodes.Last(), transformed, nodes.SkipLast(1).ToArray()) : null;
    }

    private static IAstNode? ApplyTransforms(IAstNode[] nodes, IScope scope) =>
        Transforms.Select(transform => ApplyTransform(nodes, scope, transform)).FirstOrDefault(newRoot => newRoot is not null);

    private static IAstNode? Visit(IAstNode[] nodeHierarchy, IScope currentScope) {
        var currentNode = nodeHierarchy.Last();

        currentScope = Enter(currentNode, currentScope);
        try {
            var newRoot = ApplyTransforms(nodeHierarchy, currentScope);
            if (newRoot is not null) {
                return newRoot;
            }

            foreach (var child in currentNode.Children) {
                var tree = nodeHierarchy.Append(child).ToArray();
                newRoot = Visit(tree, currentScope);
                if (newRoot is not null) {
                    return newRoot;
                }
            }
        }
        finally {
            currentScope = Exit(currentNode, currentScope);
        }

        return null;
    }

    public IAstNode Visit(IAstNode ast) {
        var lastRoot = ast;
        var currentRoot = ast;

        while (currentRoot is not null) {
            currentRoot = Visit(new[] { currentRoot }, SymbolTable.GlobalScope);
            lastRoot = currentRoot ?? lastRoot;
        }

        return lastRoot;
    }
}