using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace HarmanAI.Desktop.Infrastructure.Logging;

public class LogConfigurator
{
    private readonly IConfiguration _configuration;

    public LogConfigurator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Serilog.ILogger Configure()
    {
        var path = _configuration["Logging:Path"] ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HarmanAI", "logs");
        Directory.CreateDirectory(path);
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(Path.Combine(path, "desktop.log"), rollingInterval: RollingInterval.Day)
            .CreateLogger();
        return Log.Logger;
    }
}
