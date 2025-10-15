namespace CodeDiffPrompt.Web.Data;

public class PromptRecord
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Language { get; set; } = "auto";
    public string PromptText { get; set; } = null!;
    public string DiffText { get; set; } = null!;
    public string? LlmResponse { get; set; }
    public string? FileName { get; set; }
    
    public Guid? BeforeId { get; set; }
    public CodeSnapshot? Before { get; set; }
    
    public Guid? AfterId { get; set; }
    public CodeSnapshot? After { get; set; }
}

public class CodeSnapshot
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
}


