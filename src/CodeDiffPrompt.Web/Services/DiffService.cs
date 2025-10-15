using System.Text;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace CodeDiffPrompt.Web.Services;

public interface IDiffService
{
    string BuildUnifiedDiff(string oldText, string newText, string? fileName = null);
}

public class DiffService : IDiffService
{
    public string BuildUnifiedDiff(string oldText, string newText, string? fileName = null)
    {
        var differ = new Differ();
        var inlineBuilder = new InlineDiffBuilder(differ);
        var diff = inlineBuilder.BuildDiffModel(oldText ?? "", newText ?? "");

        var sb = new StringBuilder();
        sb.AppendLine($"--- a/{fileName ?? "before"}");
        sb.AppendLine($"+++ b/{fileName ?? "after"}");
        
        foreach (var line in diff.Lines)
        {
            var prefix = line.Type switch
            {
                ChangeType.Inserted => "+",
                ChangeType.Deleted => "-",
                ChangeType.Modified => "~",
                ChangeType.Imaginary => " ",
                _ => " "
            };
            sb.Append(prefix).AppendLine(line.Text);
        }
        
        return sb.ToString();
    }
}


