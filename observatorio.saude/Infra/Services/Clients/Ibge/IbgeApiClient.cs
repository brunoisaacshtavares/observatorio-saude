using System.Text.Json;
using observatorio.saude.Application.Services.Clients;

namespace observatorio.saude.Infra.Services.Clients.Ibge;

public class IbgeApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory) : IIbgeApiClient
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<List<IbgeUfResponse>> FindPopulacaoUfAsync()
    {
        var fullUrl = _configuration.GetValue<string>("Ibge:FindUf");
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get, fullUrl);

        var httpClient = _httpClientFactory.CreateClient("IBGE");
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        httpResponseMessage.EnsureSuccessStatusCode();

        await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

        var ibgeData = await JsonSerializer.DeserializeAsync<List<IbgeUfResponse>>(contentStream);

        return ibgeData ?? new List<IbgeUfResponse>();
    }
}