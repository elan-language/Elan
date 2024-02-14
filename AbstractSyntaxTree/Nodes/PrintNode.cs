using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record PrintNode(IAstNode? Expression, int Line, int Column) : IAstNode, ICanWrapExpression {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>().SafeAppend(Expression);

    public IAstNode Replace(IAstNode from, IAstNode to) => new PrintNode(to, 0, 0);
}