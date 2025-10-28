using FluentAssertions;
using Moq;
using observatorio.saude.Application.Queries.GetTopLeitos;
using observatorio.saude.Application.Services.Clients;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;

namespace observatorio.saude.tests.Application.Queries.GetTopLeitos;

public class GetTopLeitosHandlerTest
{
    private readonly GetTopLeitosHandler _handler;
    private readonly Mock<IIbgeApiClient> _ibgeApiClientMock;
    private readonly Mock<ILeitosRepository> _leitosRepositoryMock;

    private readonly List<UfDataResponse> _mockUfs = new()
    {
        new UfDataResponse
            { Id = 35, Sigla = "SP", Nome = "São Paulo", Regiao = new RegiaoResponse { Nome = "Sudeste" } },
        new UfDataResponse
            { Id = 33, Sigla = "RJ", Nome = "Rio de Janeiro", Regiao = new RegiaoResponse { Nome = "Sudeste" } }
    };

    public GetTopLeitosHandlerTest()
    {
        _leitosRepositoryMock = new Mock<ILeitosRepository>();
        _ibgeApiClientMock = new Mock<IIbgeApiClient>();

        _ibgeApiClientMock.Setup(c => c.FindUfsAsync()).ReturnsAsync(_mockUfs);

        _handler = new GetTopLeitosHandler(
            _leitosRepositoryMock.Object,
            _ibgeApiClientMock.Object);
    }

    private List<LeitosHospitalarDto> GetMockListResult(long ufId = 35)
    {
        return new List<LeitosHospitalarDto>
        {
            new()
            {
                NomeEstabelecimento = "Hospital Alfa", TotalLeitos = 100, LeitosDisponiveis = 20,
                LocalizacaoUf = ufId.ToString()
            },
            new()
            {
                NomeEstabelecimento = "Clínica Beta", TotalLeitos = 50, LeitosDisponiveis = 50,
                LocalizacaoUf = ufId.ToString()
            },
            new()
            {
                NomeEstabelecimento = "Posto Gama", TotalLeitos = 0, LeitosDisponiveis = 0,
                LocalizacaoUf = ufId.ToString()
            },
            new()
            {
                NomeEstabelecimento = "Hospital Delta", TotalLeitos = 10, LeitosDisponiveis = 1, LocalizacaoUf = "99"
            }
        };
    }

    [Fact]
    public async Task Handle_QuandoSemFiltroUF_DeveChamarRepositorioComCodUfNulo()
    {
        var query = new GetTopLeitosQuery { Uf = null, Count = 5, Ano = 2023 };
        var mockResult = GetMockListResult();

        _leitosRepositoryMock
            .Setup(r => r.GetTopLeitosAsync(
                It.IsAny<int?>(), null,It.IsAny<int>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResult);

        await _handler.Handle(query, CancellationToken.None);

        _leitosRepositoryMock.Verify(r => r.GetTopLeitosAsync(
            query.Ano,null,
            query.Count,
            null,
            It.IsAny<CancellationToken>()), Times.Once);

        _ibgeApiClientMock.Verify(c => c.FindUfsAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_QuandoComFiltroUFValido_DeveChamarRepositorioComCodUfCorreto()
    {
        var ufSigla = "RJ";
        long codUfEsperado = 33;
        var query = new GetTopLeitosQuery { Uf = ufSigla, Count = 5, Ano = 2023 };
        var mockResult = GetMockListResult(codUfEsperado);

        _leitosRepositoryMock
            .Setup(r => r.GetTopLeitosAsync(
                It.IsAny<int?>(), null,It.IsAny<int>(), codUfEsperado, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResult);

        await _handler.Handle(query, CancellationToken.None);

        _ibgeApiClientMock.Verify(c => c.FindUfsAsync(), Times.AtLeast(1));

        _leitosRepositoryMock.Verify(r => r.GetTopLeitosAsync(
            query.Ano,null,
            query.Count,
            codUfEsperado,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_QuandoComFiltroUFInvalido_DeveChamarRepositorioComCodUfNulo()
    {
        var query = new GetTopLeitosQuery { Uf = "XX", Count = 5, Ano = 2023 };
        var mockResult = GetMockListResult();

        _leitosRepositoryMock
            .Setup(r => r.GetTopLeitosAsync(
                It.IsAny<int?>(), null,It.IsAny<int>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResult);

        await _handler.Handle(query, CancellationToken.None);

        _ibgeApiClientMock.Verify(c => c.FindUfsAsync(), Times.AtLeast(1));

        _leitosRepositoryMock.Verify(r => r.GetTopLeitosAsync(
            query.Ano,null,
            query.Count,
            null,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveCalcularOcupacaoEMapearUfCorretamente()
    {
        var query = new GetTopLeitosQuery { Uf = null, Count = 10, Ano = 2023 };
        long ufIdTeste = 35;
        var mockResult = GetMockListResult(ufIdTeste);

        _leitosRepositoryMock
            .Setup(r => r.GetTopLeitosAsync(
                It.IsAny<int?>(), null,It.IsAny<int>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResult);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull().And.HaveCount(4);

        var item1 = result[0];
        item1.NomeEstabelecimento.Should().Be("Hospital Alfa");
        item1.LeitosOcupados.Should().Be(80);
        item1.PorcentagemOcupacao.Should().Be(80);
        item1.LocalizacaoUf.Should().Be("SP");

        var item2 = result[1];
        item2.NomeEstabelecimento.Should().Be("Clínica Beta");
        item2.LeitosOcupados.Should().Be(0);
        item2.PorcentagemOcupacao.Should().Be(0);
        item2.LocalizacaoUf.Should().Be("SP");

        var item3 = result[2];
        item3.NomeEstabelecimento.Should().Be("Posto Gama");
        item3.LeitosOcupados.Should().Be(0);
        item3.PorcentagemOcupacao.Should().Be(0);
        item3.LocalizacaoUf.Should().Be("SP");

        var item4 = result[3];
        item4.LeitosOcupados.Should().Be(9);
        item4.PorcentagemOcupacao.Should().Be(90);
        item4.LocalizacaoUf.Should().Be("Não Informada");

        _ibgeApiClientMock.Verify(c => c.FindUfsAsync(), Times.AtLeastOnce);
    }

    [Theory]
    [InlineData(100, 30, 70)]
    [InlineData(3, 1, 67)]
    [InlineData(10, 4, 60)]
    public async Task Handle_DeveArredondarPorcentagemDeOcupacaoCorretamente(
        int totalLeitos, int leitosDisponiveis, int porcentagemEsperada)
    {
        var query = new GetTopLeitosQuery { Uf = null, Count = 1, Ano = 2023 };
        var mockItems = new List<LeitosHospitalarDto>
        {
            new()
            {
                TotalLeitos = totalLeitos,
                LeitosDisponiveis = leitosDisponiveis,
                LocalizacaoUf = "35"
            }
        };

        _leitosRepositoryMock
            .Setup(r => r.GetTopLeitosAsync(
                It.IsAny<int?>(), null,It.IsAny<int>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockItems);

        var result = await _handler.Handle(query, CancellationToken.None);

        var item = result.Single();
        item.LeitosOcupados.Should().Be(totalLeitos - leitosDisponiveis);
        item.PorcentagemOcupacao.Should().Be(porcentagemEsperada);
    }
}