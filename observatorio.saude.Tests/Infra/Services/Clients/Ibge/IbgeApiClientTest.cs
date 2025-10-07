using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using observatorio.saude.Infra.Services.Clients.Ibge;

namespace observatorio.saude.Tests.Infra.Services.Clients.Ibge;

public class IbgeApiClientTest
{
    private readonly IbgeApiClient _client;
    private readonly Mock<IConfiguration> _configMock;
    private readonly Mock<HttpMessageHandler> _handlerMock;
    private readonly HttpClient _httpClient;

    public IbgeApiClientTest()
    {
        _configMock = new Mock<IConfiguration>();
        _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_handlerMock.Object);

        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(s => s.Value).Returns("http://api.ibge.gov.br/populacao");
        _configMock.Setup(c => c.GetSection("Ibge:FindPopulacaoUf")).Returns(sectionMock.Object);

        var sectionMock2 = new Mock<IConfigurationSection>();
        sectionMock2.Setup(s => s.Value).Returns("http://api.ibge.gov.br/ufs");
        _configMock.Setup(c => c.GetSection("Ibge:FindUf")).Returns(sectionMock2.Object);

        _client = new IbgeApiClient(_configMock.Object, _httpClient);
    }

    private void SetupMockedResponse(HttpStatusCode statusCode, object content)
    {
        var jsonContent = JsonSerializer.Serialize(content);

        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(jsonContent)
            })
            .Verifiable();
    }

    [Fact]
    public async Task FindPopulacaoUfAsync_QuandoSucesso_DeveRetornarDadosCorretos()
    {
        var mockResponse = new List<IbgeUfResponse>
        {
            new()
            {
                Resultados = new List<Resultado>
                    { new() { Series = new List<Serie> { new() { Localidade = new Localidade { Id = "35" } } } } }
            }
        };
        SetupMockedResponse(HttpStatusCode.OK, mockResponse);

        var result = await _client.FindPopulacaoUfAsync(null);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Resultados.First().Series.First().Localidade.Id.Should().Be("35");

        _handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get && req.RequestUri.ToString() == "http://api.ibge.gov.br/populacao"
            ),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task FindPopulacaoUfAsync_QuandoRespostaVazia_DeveRetornarListaVazia()
    {
        var mockResponse = new List<IbgeUfResponse>();
        SetupMockedResponse(HttpStatusCode.OK, mockResponse);

        var result = await _client.FindPopulacaoUfAsync(null);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task FindPopulacaoUfAsync_QuandoJsonNulo_DeveRetornarListaVazia()
    {
        object? mockResponse = null;
        var jsonContent = "null";
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonContent)
        };

        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponseMessage)
            .Verifiable();

        var result = await _client.FindPopulacaoUfAsync(null);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task FindPopulacaoUfAsync_QuandoRespostaFalha_DeveLancarExcecao()
    {
        SetupMockedResponse(HttpStatusCode.NotFound, "{\"message\":\"Not Found\"}");

        Func<Task> act = async () => await _client.FindPopulacaoUfAsync(null);

        await act.Should().ThrowAsync<HttpRequestException>();
    }
}