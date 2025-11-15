using System;
using System.Windows.Forms;
using HarmanAI.Desktop.Bridges;
using HarmanAI.Desktop.Infrastructure.Logging;
using HarmanAI.Desktop.Infrastructure.Security;
using HarmanAI.Desktop.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HarmanAI.Desktop;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((ctx, services) =>
            {
                services.AddSingleton<LogConfigurator>();
                services.AddSingleton<TrayApp>();
                services.AddSingleton<VoiceEngine>();
                services.AddSingleton<UIAutomationController>();
                services.AddSingleton<ScreenAnalyzer>();
                services.AddSingleton<PythonBridge>();
                services.AddSingleton<NodeBrowserBridge>();
                services.AddSingleton<CommandExecutor>();
                services.AddSingleton<MemoryClient>();
                services.AddSingleton<LicenseManager>();
                services.AddSingleton<UpdateManager>();
                services.AddSingleton<SystemHasher>();
                services.AddHostedService<HeartbeatService>();
            })
            .UseConsoleLifetime();

        using var host = builder.Build();
        var logger = host.Services.GetRequiredService<LogConfigurator>().Configure();
        logger.Information("Harman AI desktop starting up");

        var tray = host.Services.GetRequiredService<TrayApp>();
        tray.Run();
    }
}
