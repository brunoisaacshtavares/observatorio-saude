using FluentAssertions;
using Moq;
using observatorio.saude.Application.Queries.GetIndicadoresLeitos;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;

namespace observatorio.saude.tests.Application.Queries.GetIndicadoresLeitos;

public class GetIndicadoresLeitosHandlerTest
{
    private readonly GetIndicadoresLeitosHandler _handler;
    private readonly Mock<ILeitosRepository> _leitoRepositoryMock;

    public GetIndicadoresLeitosHandlerTest()
    {
        _leitoRepositoryMock = new Mock<ILeitosRepository>();
        _handler = new GetIndicadoresLeitosHandler(_leitoRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_QuandoNaoHaDadosAgregados_DeveRetornarDtoVazio()
    {
        var query = new GetIndicadoresLeitosQuery { Ano = 2023 };

        _leitoRepositoryMock
            .Setup(r => r.GetLeitosAgregadosAsync(It.IsAny<int?>()))
            .ReturnsAsync((LeitosAgregadosDto)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalLeitos.Should().Be(0);
        result.LeitosDisponiveis.Should().Be(0);
        result.Criticos.Should().Be(0);
        result.OcupacaoMedia.Should().Be(0);

        _leitoRepositoryMock.Verify(r => r.GetLeitosAgregadosAsync(query.Ano), Times.Once);
    }

    [Fact]
    public async Task Handle_QuandoTotalLeitosZero_DeveRetornarOcupacaoMediaZero()
    {
        var query = new GetIndicadoresLeitosQuery { Ano = 2023 };
        var mockDados = new LeitosAgregadosDto
        {
            TotalLeitos = 0,
            TotalLeitosSus = 50,
            TotalUti = 10
        };

        _leitoRepositoryMock
            .Setup(r => r.GetLeitosAgregadosAsync(It.IsAny<int?>()))
            .ReturnsAsync(mockDados);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalLeitos.Should().Be(0);
        result.LeitosDisponiveis.Should().Be(50);
        result.Criticos.Should().Be(10);
        result.OcupacaoMedia.Should().Be(0);
    }

    [Fact]
    public async Task Handle_DeveCalcularOcupacaoMediaCorretamente()
    {
        var query = new GetIndicadoresLeitosQuery { Ano = 2023 };
        var mockDados = new LeitosAgregadosDto
        {
            TotalLeitos = 200,
            TotalLeitosSus = 50,
            TotalUti = 20
        };

        _leitoRepositoryMock
            .Setup(r => r.GetLeitosAgregadosAsync(It.IsAny<int?>()))
            .ReturnsAsync(mockDados);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalLeitos.Should().Be(200);
        result.LeitosDisponiveis.Should().Be(50);
        result.Criticos.Should().Be(20);
        result.OcupacaoMedia.Should().Be(75.0);
    }

    [Fact]
    public async Task Handle_DeveArredondarOcupacaoMediaParaDuasCasasDecimais()
    {
        var query = new GetIndicadoresLeitosQuery { Ano = 2023 };
        var mockDados = new LeitosAgregadosDto
        {
            TotalLeitos = 300,
            TotalLeitosSus = 100,
            TotalUti = 10
        };

        _leitoRepositoryMock
            .Setup(r => r.GetLeitosAgregadosAsync(It.IsAny<int?>()))
            .ReturnsAsync(mockDados);

        var result = await _handler.Handle(query, CancellationToken.None);

        var ocupacaoEsperadaArredondada = 66.67;

        result.OcupacaoMedia.Should().Be(ocupacaoEsperadaArredondada);
    }
}