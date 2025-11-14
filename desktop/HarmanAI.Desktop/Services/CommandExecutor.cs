using HarmanAI.Desktop.Bridges;
using HarmanAI.Desktop.Models;

namespace HarmanAI.Desktop.Services;

public class CommandExecutor
{
    private readonly UIAutomationController _ui;
    private readonly NodeBrowserBridge _browserBridge;

    public CommandExecutor(UIAutomationController ui, NodeBrowserBridge browserBridge)
    {
        _ui = ui;
        _browserBridge = browserBridge;
    }

    public async Task ExecuteAsync(DesktopCommand command, CancellationToken ct)
    {
        switch (command.Type)
        {
            case "sendKeys":
                if (command.Parameters.TryGetValue("text", out var text))
                {
                    _ui.SendKeys(text);
                }
                break;
            case "browser.open":
                await _browserBridge.SendActionAsync("/open", new { url = command.Parameters["url"] }, ct);
                break;
            default:
                break;
        }
    }
}
