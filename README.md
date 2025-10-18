# Code Diff Prompt

Web-based tool for analyzing code changes using AI. Generates unified diffs and sends them to Claude for intelligent code review.

## Features

- Built-in code editor with syntax highlighting
- Automatic diff generation between versions
- AI-powered code review via Claude API
- History of past analyses
- Support for different programming languages

## Prerequisites

- .NET 8.0 SDK
- Anthropic API key

## Quick Start

1. **Clone and navigate**
   ```bash
   cd src/CodeDiffPrompt.Web
   ```

2. **Set up environment**
   
   Create a `.env` file:
   ```bash
    cp .env.example .env
   ```
   Set API key in `.env` file.

3. **Run migrations**
   ```bash
   dotnet ef database update
   ```

4. **Start the app**
   ```bash
   dotnet run
   ```

5. **Open browser**
   ```
   https://localhost:5001
   ```

## Usage

1. **Capture Original** - Save the initial version of your code
2. **Edit code** - Make your changes in the editor
3. **Generate Prompt** - Create a diff and review prompt
4. **Analyze Changes** - Send to Claude for AI review

View past analyses in the History page.

## Configuration

Edit `appsettings.json` to customize:
- Claude model and parameters
- Database connection
- Logging settings

## Tech Stack

- ASP.NET Core 8.0
- Blazor Server
- Entity Framework Core
- SQLite
- Monaco Editor
- Claude API
