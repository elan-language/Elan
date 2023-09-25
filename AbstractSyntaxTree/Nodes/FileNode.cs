using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record FileNode : IAstNode {
    public FileNode(ImmutableArray<IAstNode> GlobalNodes, MainNode MainNode) {
        this.GlobalNodes = GlobalNodes;
        this.MainNode = MainNode;
    }

    public ImmutableArray<IAstNode> GlobalNodes { get; init; }
    public MainNode MainNode { get; init; }
    public IEnumerable<IAstNode> Children => new[] { MainNode };

    public void Deconstruct(out ImmutableArray<IAstNode> GlobalNodes, out MainNode MainNode) {
        GlobalNodes = this.GlobalNodes;
        MainNode = this.MainNode;
    }
}