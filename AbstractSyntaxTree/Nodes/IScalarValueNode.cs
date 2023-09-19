namespace AbstractSyntaxTree.Nodes;

public interface IScalarValueNode : IAstNode {
    public string Value { get; }
}