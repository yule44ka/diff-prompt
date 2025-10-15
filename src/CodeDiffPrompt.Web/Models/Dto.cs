namespace CodeDiffPrompt.Web.Models;

public record AnalyzeRequest(
    string Before, 
    string After, 
    string Language, 
    string? FileName, 
    string? PromptOverride);

public record AnalyzeResponse(
    Guid Id, 
    string Prompt, 
    string Diff, 
    string LlmResponse);

public record HistoryItemDto(
    Guid Id,
    DateTime CreatedAt,
    string Language,
    string? FileName,
    string DiffPreview);


