using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using HarmanAI.Desktop.Infrastructure.Security;
using Microsoft.Extensions.Configuration;

namespace HarmanAI.Desktop.Services;

public class LicenseManager
{
    private readonly IConfiguration _configuration;
    private readonly SystemHasher _hasher;
    private readonly HttpClient _client = new();
    private string? _token;

    public LicenseManager(IConfiguration configuration, SystemHasher hasher)
    {
        _configuration = configuration;
        _hasher = hasher;
    }

    public async Task<bool> ActivateAsync(string licenseKey, CancellationToken ct)
    {
        var hardwareId = _hasher.ComputeHardwareId();
        var response = await _client.PostAsJsonAsync(_configuration["PortalApiUrl"] + "/license/activate", new
        {
            licenseKey,
            hardwareId
        }, ct);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        var payload = await response.Content.ReadFromJsonAsync<LicenseResponse>(cancellationToken: ct);
        _token = payload?.Token;
        return _token != null;
    }

    public string? Token => _token;

    private class LicenseResponse
    {
        public string? Token { get; set; }
    }
}
