using AbstractSyntaxTree.Nodes;

namespace AbstractSyntaxTree;

public abstract class AbstractAstVisitor<T> {

    protected abstract void Enter(IAstNode node);

    protected abstract void Exit(IAstNode node);

    protected T HandleScope<TNode>(Func<TNode, T> builder, TNode node) where TNode : IAstNode {
        Enter(node);
        try {
            return builder(node);
        }
        finally {
            Exit(node);
        }
    }

    public abstract T Visit(IAstNode node);
}