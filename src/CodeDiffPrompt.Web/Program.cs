using CodeDiffPrompt.Web.Data;
using CodeDiffPrompt.Web.Models;
using CodeDiffPrompt.Web.Services;
using Microsoft.EntityFrameworkCore;

// Загружаем переменные из .env файла
EnvLoader.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AnthropicOptions>(builder.Configuration.GetSection("Anthropic"));
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddHttpClient<ClaudeClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(2),
        PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1)
    });
builder.Services.AddScoped<IDiffService, DiffService>();
builder.Services.AddScoped<PromptBuilder>();
builder.Services.AddScoped<HistoryService>();

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();