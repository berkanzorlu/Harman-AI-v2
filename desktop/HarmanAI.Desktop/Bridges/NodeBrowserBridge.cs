using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HarmanAI.Desktop.Bridges;

public class NodeBrowserBridge
{
    private readonly HttpClient _client = new();
    private readonly IConfiguration _configuration;

    public NodeBrowserBridge(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<HttpResponseMessage> SendActionAsync(string endpoint, object payload, CancellationToken ct)
    {
        var baseUrl = _configuration["BrowserControllerUrl"] ?? throw new InvalidOperationException("BrowserControllerUrl not configured");
        var url = baseUrl.TrimEnd('/') + endpoint;
        return await _client.PostAsJsonAsync(url, payload, ct);
    }
}
