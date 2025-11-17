using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using HarmanAI.Desktop.Models;
using Microsoft.Extensions.Configuration;

namespace HarmanAI.Desktop.Bridges;

public class PythonBridge
{
    private readonly HttpClient _client = new();
    private readonly IConfiguration _configuration;

    public PythonBridge(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<InterpretationResult?> InterpretAsync(string text, CancellationToken ct)
    {
        var baseUrl = _configuration["PythonBackendUrl"] ?? throw new InvalidOperationException("PythonBackendUrl not configured");
        var url = baseUrl.TrimEnd('/') + "/v1/interpret";
        var response = await _client.PostAsJsonAsync(url, new { text }, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<InterpretationResult>(cancellationToken: ct);
    }
}
