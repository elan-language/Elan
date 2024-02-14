namespace AbstractSyntaxTree.Nodes;

public record IdentifierWithTypeNode(string Id, IAstNode Type, int Line, int Column) : IdentifierNode(Id, Line, Column) {
    public override IEnumerable<IAstNode> Children => new[] { Type };
    public override IAstNode Replace(IAstNode from, IAstNode to) => this with { Type = to };
}