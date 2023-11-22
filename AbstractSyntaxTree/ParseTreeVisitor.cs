using AbstractSyntaxTree.Nodes;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace AbstractSyntaxTree;

public class ParseTreeVisitor : ElanBaseVisitor<IAstNode> {
    public override IAstNode VisitChildren(IRuleNode node) => node is ParserRuleContext c ? this.Build(c) : base.VisitChildren(node);

    public override IAstNode VisitTerminal(ITerminalNode node) => this.BuildTerminal(node);
}