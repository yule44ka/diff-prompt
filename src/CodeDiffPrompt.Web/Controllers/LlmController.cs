using CodeDiffPrompt.Web.Models;
using CodeDiffPrompt.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeDiffPrompt.Web.Controllers;

[ApiController]
[Route("api/llm")]
public class LlmController : ControllerBase
{
    private readonly IDiffService _diffService;
    private readonly PromptBuilder _promptBuilder;
    private readonly ClaudeClient _claudeClient;
    private readonly HistoryService _historyService;
    private readonly ILogger<LlmController> _logger;

    public LlmController(
        IDiffService diffService,
        PromptBuilder promptBuilder,
        ClaudeClient claudeClient,
        HistoryService historyService,
        ILogger<LlmController> logger)
    {
        _diffService = diffService;
        _promptBuilder = promptBuilder;
        _claudeClient = claudeClient;
        _historyService = historyService;
        _logger = logger;
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<AnalyzeResponse>> Analyze(
        [FromBody] AnalyzeRequest request,
        CancellationToken ct)
    {
        try
        {
            // Generate diff
            var diff = _diffService.BuildUnifiedDiff(request.Before, request.After, request.FileName);

            // Build prompt
            var prompt = string.IsNullOrWhiteSpace(request.PromptOverride)
                ? _promptBuilder.Build(request.Language, request.FileName, diff)
                : request.PromptOverride;

            // Call Claude
            var llmResponse = await _claudeClient.AnalyzeAsync(prompt, ct);

            // Save to history
            var recordId = await _historyService.SaveRecordAsync(
                request.Before,
                request.After,
                request.Language,
                request.FileName,
                prompt,
                diff,
                llmResponse,
                ct);

            return Ok(new AnalyzeResponse(recordId, prompt, diff, llmResponse));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Analysis failed");
            return StatusCode(500, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during analysis");
            return StatusCode(500, new { error = "An unexpected error occurred during analysis." });
        }
    }
}


