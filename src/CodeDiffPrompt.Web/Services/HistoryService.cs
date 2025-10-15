using CodeDiffPrompt.Web.Data;
using CodeDiffPrompt.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeDiffPrompt.Web.Services;

public class HistoryService
{
    private readonly AppDbContext _db;
    private readonly ILogger<HistoryService> _logger;

    public HistoryService(AppDbContext db, ILogger<HistoryService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Guid> SaveRecordAsync(
        string before,
        string after,
        string language,
        string? fileName,
        string promptText,
        string diffText,
        string llmResponse,
        CancellationToken ct = default)
    {
        var beforeSnapshot = new CodeSnapshot { Id = Guid.NewGuid(), Content = before };
        var afterSnapshot = new CodeSnapshot { Id = Guid.NewGuid(), Content = after };

        var record = new PromptRecord
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Language = language,
            FileName = fileName,
            PromptText = promptText,
            DiffText = diffText,
            LlmResponse = llmResponse,
            Before = beforeSnapshot,
            After = afterSnapshot
        };

        _db.CodeSnapshots.Add(beforeSnapshot);
        _db.CodeSnapshots.Add(afterSnapshot);
        _db.PromptRecords.Add(record);

        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Saved prompt record {RecordId}", record.Id);

        return record.Id;
    }

    public async Task<List<HistoryItemDto>> GetRecentHistoryAsync(int limit = 20, CancellationToken ct = default)
    {
        var records = await _db.PromptRecords
            .OrderByDescending(r => r.CreatedAt)
            .Take(limit)
            .Select(r => new HistoryItemDto(
                r.Id,
                r.CreatedAt,
                r.Language,
                r.FileName,
                r.DiffText.Length > 200 ? r.DiffText.Substring(0, 200) + "..." : r.DiffText
            ))
            .ToListAsync(ct);

        return records;
    }

    public async Task<PromptRecord?> GetRecordByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.PromptRecords
            .Include(r => r.Before)
            .Include(r => r.After)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }
}


