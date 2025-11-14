using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace HarmanAI.Desktop.Services;

public class MemoryClient
{
    private readonly HttpClient _client = new();
    private readonly IConfiguration _configuration;

    public MemoryClient(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<HttpResponseMessage> WriteAsync(object payload, CancellationToken ct)
    {
        var url = _configuration["PythonBackendUrl"] + "/v1/memory/write";
        return _client.PostAsJsonAsync(url, payload, ct);
    }
}
