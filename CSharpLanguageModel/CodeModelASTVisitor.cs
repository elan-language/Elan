using AbstractSyntaxTree.Nodes;
using CSharpLanguageModel.Models;

namespace CSharpLanguageModel;

public class CodeModelAstVisitor {
    private void Enter(IAstNode node) { }

    private void Exit(IAstNode node) { }

    private ICodeModel HandleScope<T>(Func<T, ICodeModel> func, T node) where T : IAstNode {
        Enter(node);
        try {
            return func(node);
        }
        finally {
            Exit(node);
        }
    }

    private FileCodeModel BuildFileModel(FileNode fileNode) {
        var main = Visit(fileNode.MainNode);
        var globals = fileNode.GlobalNodes.Select(Visit);
        return new FileCodeModel(globals, main);
    }

    private MainCodeModel BuildMainModel(MainNode mainNode) => new(mainNode.StatementNodes.Select(Visit));

    public ICodeModel Visit(IAstNode astNode) {
        return astNode switch {
            FileNode fn => HandleScope(BuildFileModel, fn),
            MainNode mn => HandleScope(BuildMainModel, mn),
            MethodCallNode mc => HandleScope(BuildMethodCallModel, mc),
            IScalarValueNode svn => HandleScope(BuildScalarValueModel, svn),
            VarDefNode vdn => HandleScope(BuildVarDefModel, vdn),
            ConstantDefNode cdn => HandleScope(BuildConstantDefModel, cdn),
            AssignmentNode vdn => HandleScope(BuildAssignmentModel, vdn),
            IdentifierNode idn => HandleScope(BuildIdentifierModel, idn),
            null => throw new NotImplementedException("null"),
            _ => throw new NotImplementedException(astNode.GetType().ToString() ?? "null")
        };
    }

    private VarDefModel BuildVarDefModel(VarDefNode varDefNode) => new(Visit(varDefNode.Id), Visit(varDefNode.Expression));

    private ConstantDefModel BuildConstantDefModel(ConstantDefNode constantDefNode) => new(Visit(constantDefNode.Id), CodeHelpers.NodeToCSharpType(constantDefNode.Expression), Visit(constantDefNode.Expression));

    private AssignmentModel BuildAssignmentModel(AssignmentNode assignmentNode) => new(Visit(assignmentNode.Id), Visit(assignmentNode.Expression));

    private MethodCallModel BuildMethodCallModel(MethodCallNode methodCallNode) => new(Visit(methodCallNode.Id), methodCallNode.Parameters.Select(Visit));

    private ScalarValueModel BuildScalarValueModel(IScalarValueNode scalarValueNode) => new(scalarValueNode.Value);

    private IdentifierModel BuildIdentifierModel(IdentifierNode identifierNode) => new(identifierNode.Id);
}