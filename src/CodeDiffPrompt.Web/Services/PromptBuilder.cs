namespace CodeDiffPrompt.Web.Services;

public class PromptBuilder
{
    public const string DefaultInstruction = @"You are a senior code reviewer. Analyze the following unified diff.
Explain what changed, why it might have been changed, potential risks, potential bugs, and possible improvements.
Return a concise, structured review.";

    public string Build(string language, string? fileName, string unifiedDiff, string? userInstruction = null)
    {
        var instruction = userInstruction ?? DefaultInstruction;
        
        var prompt = $@"### Task
{instruction}

### Context
Language: {language}
File: {fileName ?? "N/A"}

### Diff
```diff
{unifiedDiff}
```

### Requirements
- Use bullet points for clarity
- Mention any risky or breaking changes
- Propose fixes or tests where applicable
- Be concise but thorough";

        return prompt;
    }
}


