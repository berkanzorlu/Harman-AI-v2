using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
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
        var baseUrl = _configuration["PythonBackendUrl"] ?? throw new InvalidOperationException("PythonBackendUrl not configured");
        var url = baseUrl.TrimEnd('/') + "/v1/memory/write";
        return _client.PostAsJsonAsync(url, payload, ct);
    }
}
