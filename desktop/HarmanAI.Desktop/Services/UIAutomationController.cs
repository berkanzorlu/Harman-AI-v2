namespace HarmanAI.Desktop.Services;

public class UIAutomationController
{
    public void ClickButton(string automationId)
    {
        // Placeholder for UIAutomation logic
    }

    public void SendKeys(string text)
    {
        System.Windows.Forms.SendKeys.SendWait(text);
    }
}
