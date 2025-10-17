namespace CodeDiffPrompt.Web.Models;

public class AnthropicOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string ApiBase { get; set; } = "https://api.anthropic.com";
    public string Version { get; set; } = "2023-06-01";
    public string Model { get; set; } = "claude-sonnet-4-20250514";
    public int MaxTokens { get; set; } = 4096;
    public double Temperature { get; set; } = 0.7;
}

public class AppOptions
{
    public string DefaultLanguage { get; set; } = "csharp";
    public int MaxHistoryRecords { get; set; } = 100;
}


