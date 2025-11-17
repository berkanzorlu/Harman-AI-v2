using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using HarmanAI.Desktop.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HarmanAI.Desktop.Services;

public class HeartbeatService : BackgroundService
{
    private readonly LicenseManager _licenseManager;
    private readonly IConfiguration _configuration;
    private readonly SystemHasher _hasher;
    private readonly HttpClient _client = new();
    private readonly ILogger<HeartbeatService> _logger;

    public HeartbeatService(LicenseManager licenseManager, IConfiguration configuration, SystemHasher hasher, ILogger<HeartbeatService> logger)
    {
        _licenseManager = licenseManager;
        _configuration = configuration;
        _hasher = hasher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var token = _licenseManager.Token;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    await _client.PostAsJsonAsync(_configuration["PortalApiUrl"] + "/license/verify", new
                    {
                        token,
                        hardwareId = _hasher.ComputeHardwareId()
                    }, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Heartbeat failed");
            }

            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}
