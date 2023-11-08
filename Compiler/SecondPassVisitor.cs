using System.Data;
using AbstractSyntaxTree.Nodes;
using SymbolTable;

namespace Compiler;

public class SecondPassVisitor {
    static SecondPassVisitor() {
        Transforms.Add(CompilerTransforms.TransformMethodCallNodes);
        Transforms.Add(CompilerTransforms.TransformIndexNodes);
        Transforms.Add(CompilerTransforms.TransformLiteralListNodes);
    }

    public SecondPassVisitor(SymbolTableImpl symbolTable) => SymbolTable = symbolTable;

    private SymbolTableImpl SymbolTable { get; }
    private static IList<Func<IAstNode[], IScope, IAstNode?>> Transforms { get; } = new List<Func<IAstNode[], IScope, IAstNode?>>();

    private static string SignatureId(IAstNode signature) =>
        signature switch {
            MethodSignatureNode { Id: IdentifierNode idn } => idn.Id,
            _ => throw new NotImplementedException()
        };

    private IScope Enter(IAstNode node, IScope currentScope) {
        return node switch {
            MainNode => currentScope.Resolve(Constants.WellKnownMainId) as IScope ?? throw new ArgumentNullException(),
            ClassDefNode cdn => currentScope.Resolve(cdn.Name) as IScope ?? throw new ArgumentNullException(),
            FunctionDefNode fdn => currentScope.Resolve(SignatureId(fdn.Signature)) as IScope ?? throw new ArgumentNullException(),
            ProcedureDefNode pdn => currentScope.Resolve(SignatureId(pdn.Signature)) as IScope ?? throw new ArgumentNullException(),
            ConstructorNode cn => currentScope.Resolve(Constants.WellKnownConstructorId) as IScope ?? throw new ArgumentNullException(),
            _ => currentScope,
        };
    }

    private IScope Exit(IAstNode node, IScope currentScope) {
        return node switch {
            MainNode => currentScope.EnclosingScope ?? throw new ArgumentNullException(),
            ClassDefNode cdn => currentScope.EnclosingScope ?? throw new ArgumentNullException(),
            FunctionDefNode fdn => currentScope.EnclosingScope ?? throw new ArgumentNullException(),
            ProcedureDefNode pdn => currentScope.EnclosingScope ?? throw new ArgumentNullException(),
            ConstructorNode cn => currentScope.EnclosingScope ?? throw new ArgumentNullException(),
            _ => currentScope
        };
    }

    private IAstNode InsertIntoTree(IAstNode oldNode, IAstNode newNode, IAstNode[] parentNodes) {
        if (parentNodes.Any()) {
            var parent = parentNodes.Last();
            var newParent = parent.Replace(oldNode, newNode);
            return InsertIntoTree(parent, newParent, parentNodes.SkipLast(1).ToArray());
        }

        return newNode;
    }

    private IAstNode? ApplyTransform(IAstNode[] nodes, IScope scope, Func<IAstNode[], IScope, IAstNode?> transform) {
        var transformed = transform(nodes, scope);
        if (transformed is not null) {
            return InsertIntoTree(nodes.Last(), transformed, nodes.SkipLast(1).ToArray());
        }

        return null;
    }

    private IAstNode? ApplyTransforms(IAstNode[] nodes, IScope scope) {
        foreach (var transform in Transforms) {
            var newRoot = ApplyTransform(nodes, scope, transform);
            if (newRoot is not null) {
                return newRoot;
            }
        }

        return null;
    }

    private IAstNode? Visit(IAstNode[] nodeHierarchy, IScope currentScope) {
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