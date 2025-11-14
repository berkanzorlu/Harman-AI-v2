using System.Net.Http.Json;
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
        var url = _configuration["BrowserControllerUrl"] + endpoint;
        return await _client.PostAsJsonAsync(url, payload, ct);
    }
}
