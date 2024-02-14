using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record MainNode(IAstNode StatementBlock, int Line, int Column) : IAstNode, IHasScope {
    public IEnumerable<IAstNode> Children => new[] { StatementBlock };
    public IAstNode Replace(IAstNode from, IAstNode to) => new MainNode(to, 0, 0);

    public string Name => "_main"; // TODO make constant
}