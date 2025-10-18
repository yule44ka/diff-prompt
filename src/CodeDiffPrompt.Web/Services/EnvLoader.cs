namespace CodeDiffPrompt.Web.Services;

public static class EnvLoader
{
    public static void Load(string filePath = ".env")
    {
        if (!File.Exists(filePath))
            return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var trimmedLine = line.Trim();
            
            // Skip empty lines and comments
            if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith('#'))
                continue;

            var parts = trimmedLine.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length != 2)
                continue;

            var key = parts[0].Trim();
            var value = parts[1].Trim();
            
            // Remove quotes if present
            if (value.StartsWith('"') && value.EndsWith('"'))
                value = value[1..^1];
            else if (value.StartsWith('\'') && value.EndsWith('\''))
                value = value[1..^1];

            Environment.SetEnvironmentVariable(key, value);
        }
    }
}


