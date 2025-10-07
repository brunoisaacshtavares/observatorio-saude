using System.Text.Json;
using observatorio.saude.Application.Services.Clients;

// Adicione este using

// Adicione este using

namespace observatorio.saude.Infra.Services.Clients.Ibge;

public class IbgeApiClient(IConfiguration configuration, HttpClient httpClient) : IIbgeApiClient
{
    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _httpClient = httpClient;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<IbgeUfResponse>> FindPopulacaoUfAsync(int? ano)
    {
        var anoParaBuscar = ano ?? DateTime.Now.Year;

        var urlTemplate = _configuration.GetValue<string>("Ibge:FindPopulacaoUf");
        var fullUrl = urlTemplate.Replace("{ano}", anoParaBuscar.ToString());

        var httpResponseMessage = await _httpClient.GetAsync(fullUrl);

        httpResponseMessage.EnsureSuccessStatusCode();

        await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

        var ibgeData = await JsonSerializer.DeserializeAsync<List<IbgeUfResponse>>(contentStream, _jsonOptions);

        return ibgeData ?? new List<IbgeUfResponse>();
    }

    public async Task<IEnumerable<UfDataResponse>> FindUfsAsync()
    {
        var fullUrl = _configuration.GetValue<string>("Ibge:FindUf");
        var response = await _httpClient.GetAsync(fullUrl);
        response.EnsureSuccessStatusCode();

        await using var contentStream = await response.Content.ReadAsStreamAsync();

        var ufsData = await JsonSerializer.DeserializeAsync<IEnumerable<UfDataResponse>>(contentStream, _jsonOptions);

        return ufsData ?? Enumerable.Empty<UfDataResponse>();
    }
}