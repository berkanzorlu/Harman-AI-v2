namespace HarmanAI.Desktop.Models;

public record DesktopCommand(string Type, Dictionary<string, string> Parameters);

public class InterpretationResult
{
    public string Intent { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public List<DesktopCommand> Commands { get; set; } = new();
}
