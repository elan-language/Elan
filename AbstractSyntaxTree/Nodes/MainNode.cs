using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record MainNode(IAstNode StatementBlock) : IAstNode, IHasScope {
    public IEnumerable<IAstNode> Children => new[] { StatementBlock };
    public IAstNode Replace(IAstNode from, IAstNode to) => new MainNode(to);

    public string Name => "_main"; // TODO make constant
}