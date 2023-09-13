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
        return new FileCodeModel(main);
    }

    private MainCodeModel BuildMainModel(MainNode mainNode) => new(mainNode.StatementNodes.Select(Visit));

    public ICodeModel Visit(IAstNode astNode) {
        return astNode switch {
            FileNode fn => HandleScope(BuildFileModel, fn),
            MainNode mn => HandleScope(BuildMainModel, mn),
            MethodCallNode mc => HandleScope(BuildMethodCallModel, mc),
            ScalarValueNode svn => HandleScope(BuildScalarValueModel, svn),
            null => throw new NotImplementedException("null"),
            _ => throw new NotImplementedException(astNode.GetType().ToString() ?? "null")
        };
    }

    private MethodCallModel BuildMethodCallModel(MethodCallNode methodCallNode) => new(Visit(methodCallNode.Id), methodCallNode.Parameters.Select(Visit));

    private ScalarValueModel BuildScalarValueModel(ScalarValueNode scalarValueNode) => new(scalarValueNode.Id);
}