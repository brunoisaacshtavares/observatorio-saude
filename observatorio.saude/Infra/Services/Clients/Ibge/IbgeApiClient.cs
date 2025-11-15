using System.Net;
using System.Text.Json;
using observatorio.saude.Application.Services.Clients;

namespace observatorio.saude.Infra.Services.Clients.Ibge;

public class IbgeApiClient(IConfiguration configuration, HttpClient httpClient) : IIbgeApiClient
{
    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _httpClient = httpClient;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<PopulacaoUfResultado> FindPopulacaoUfAsync(int? ano)
    {
        var anoInicial = ano ?? DateTime.Now.Year;

        const int maxTentativas = 3;

        for (var i = 0; i < maxTentativas; i++)
        {
            var anoTentar = anoInicial - i;

            var urlTemplate = _configuration.GetValue<string>("Ibge:FindPopulacaoUf");
            var fullUrl = urlTemplate?.Replace("{ano}", anoTentar.ToString());

            var httpResponseMessage = await _httpClient.GetAsync(fullUrl);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                var ibgeData = await JsonSerializer.DeserializeAsync<List<IbgeUfResponse>>(contentStream, _jsonOptions);

                if (ibgeData != null && ibgeData.Count > 0) return new PopulacaoUfResultado(anoTentar, ibgeData);
            }
            else if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
            }
            else
            {
                httpResponseMessage.EnsureSuccessStatusCode();
            }
        }

        return new PopulacaoUfResultado(null, new List<IbgeUfResponse>());
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