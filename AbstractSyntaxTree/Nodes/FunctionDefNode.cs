using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record FunctionDefNode(IAstNode Signature, IAstNode StatementBlock, IAstNode Return, bool Standalone/*true*/, int Line, int Column) : IAstNode, IHasScope {
    public IEnumerable<IAstNode> Children => new[] { Signature, StatementBlock, Return };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Signature => this with { Signature = to },
            _ when from == StatementBlock => this with { StatementBlock = to },
            _ when from == Return => this with { Return = to },
            _ => throw new NotImplementedException()
        };
    }

    public string Name => ((INamedAstNode)Signature).Name;
}