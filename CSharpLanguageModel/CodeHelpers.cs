using CSharpLanguageModel.Models;

namespace CSharpLanguageModel;

public static class CodeHelpers {
    public static string AsLineSeparatedString(this IEnumerable<ICodeModel> mm, int indent = 0) {
        var indentation = new string(' ', indent);
        return string.Join("\r\n", mm.Select(cm => $"{indentation}{cm.AsCode()}")).Trim();
    }

    public static string AsCommaSeparatedString(this IEnumerable<ICodeModel> mm) => string.Join(", ", mm.Select(v => v.ToString())).Trim();

    public static string AsCode(this ICodeModel? cm) {
        return (cm?.ToString() ?? "").Trim();
    }
}