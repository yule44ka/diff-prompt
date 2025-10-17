using System.Net.Http.Json;
using System.Text.Json;
using CodeDiffPrompt.Web.Models;
using Microsoft.Extensions.Options;

namespace CodeDiffPrompt.Web.Services;

public class ClaudeClient
{
    private readonly HttpClient _http;
    private readonly AnthropicOptions _options;
    private readonly ILogger<ClaudeClient> _logger;

    public ClaudeClient(
        HttpClient httpClient, 
        IOptions<AnthropicOptions> options,
        ILogger<ClaudeClient> logger)
    {
        _http = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> AnalyzeAsync(string prompt, CancellationToken ct = default)
    {
        try
        {
            var requestBody = new
            {
                model = _options.Model,
                max_tokens = _options.MaxTokens,
                temperature = _options.Temperature,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_options.ApiBase}/v1/messages");
            httpRequest.Headers.Add("x-api-key", _options.ApiKey);
            httpRequest.Headers.Add("anthropic-version", _options.Version);
            httpRequest.Content = JsonContent.Create(requestBody);

            _logger.LogInformation("Sending request to Claude API: URL={Url}, Model={Model}, Version={Version}", 
                $"{_options.ApiBase}/v1/messages", _options.Model, _options.Version);

            var response = await _http.SendAsync(httpRequest, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
            var result = json.GetProperty("content")[0].GetProperty("text").GetString() ?? "";

            _logger.LogInformation("Received response from Claude API ({Length} chars)", result.Length);

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request to Claude API failed");
            throw new InvalidOperationException("Failed to communicate with Claude API. Please check your API key and network connection.", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse Claude API response");
            throw new InvalidOperationException("Failed to parse response from Claude API.", ex);
        }
    }
}


