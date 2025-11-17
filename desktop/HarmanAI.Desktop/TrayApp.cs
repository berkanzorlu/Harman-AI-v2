using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HarmanAI.Desktop.Bridges;
using HarmanAI.Desktop.Models;
using HarmanAI.Desktop.Services;
using Microsoft.VisualBasic;

namespace HarmanAI.Desktop;

public class TrayApp
{
    private readonly VoiceEngine _voice;
    private readonly PythonBridge _python;
    private readonly CommandExecutor _executor;
    private readonly MemoryClient _memory;
    private readonly LicenseManager _licenseManager;

    public TrayApp(VoiceEngine voice, PythonBridge python, CommandExecutor executor, MemoryClient memory, LicenseManager licenseManager)
    {
        _voice = voice;
        _python = python;
        _executor = executor;
        _memory = memory;
        _licenseManager = licenseManager;
    }

    public void Run()
    {
        EnsureLicenseAsync().GetAwaiter().GetResult();
        _voice.CommandRecognized += HandleCommandAsync;
        _voice.Start();
        Application.Run();
    }

    private async void HandleCommandAsync(object? sender, string text)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var result = await _python.InterpretAsync(text, cts.Token);
        if (result == null)
        {
            return;
        }

        foreach (var command in result.Commands)
        {
            await _executor.ExecuteAsync(command, cts.Token);
        }

        await _memory.WriteAsync(new
        {
            userId = "local",
            role = "user",
            content = text,
            actionType = "speech"
        }, cts.Token);
    }

    private async Task EnsureLicenseAsync()
    {
        if (_licenseManager.Token != null)
        {
            return;
        }

        var input = Interaction.InputBox("Enter Harman AI license key", "License Activation", string.Empty);
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new InvalidOperationException("License key required");
        }

        var success = await _licenseManager.ActivateAsync(input, CancellationToken.None);
        if (!success)
        {
            throw new InvalidOperationException("Activation failed");
        }
    }
}
