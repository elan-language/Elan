using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record TestDefNode(IAstNode Id, IAstNode TestStatements, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Id, TestStatements };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == TestStatements => this with { TestStatements = to },
            _ => throw new NotImplementedException()
        };
    }

    public string Name => ((IdentifierNode)Id).Id;
}