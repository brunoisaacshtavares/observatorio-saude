using FluentAssertions;
using MediatR;
using Moq;
using observatorio.saude.Application.Queries.GetIndicadoresLeitos;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.tests.Application.Queries.GetIndicadoresLeitos;

public class GetIndicadoresLeitosPorRegiaoHandlerTest
{
    private readonly GetIndicadoresLeitosPorRegiaoHandler _handler;
    private readonly Mock<IMediator> _mediatorMock;

    public GetIndicadoresLeitosPorRegiaoHandlerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _handler = new GetIndicadoresLeitosPorRegiaoHandler(_mediatorMock.Object);
    }

    private List<IndicadoresLeitosEstadoDto> GetMockIndicadoresPorEstado()
    {
        return new List<IndicadoresLeitosEstadoDto>
        {
            new()
            {
                Regiao = "SUDESTE", Populacao = 1000000, TotalLeitos = 1000, LeitosDisponiveis = 100, Criticos = 100
            },
            new() { Regiao = "SUDESTE", Populacao = 500000, TotalLeitos = 600, LeitosDisponiveis = 200, Criticos = 50 },

            new()
            {
                Regiao = "NORDESTE", Populacao = 800000, TotalLeitos = 700, LeitosDisponiveis = 200, Criticos = 70
            },
            new()
            {
                Regiao = "NORDESTE", Populacao = 400000, TotalLeitos = 300, LeitosDisponiveis = 150, Criticos = 30
            },

            new() { Regiao = "SUL", Populacao = 600000, TotalLeitos = 500, LeitosDisponiveis = 50, Criticos = 40 },

            new() { Regiao = "NORTE", Populacao = 0, TotalLeitos = 0, LeitosDisponiveis = 0, Criticos = 0 }
        };
    }

    [Fact]
    public async Task Handle_DeveAgruparEstadosPorRegiao_ESomarIndicadoresCorretamente()
    {
        var mockEstados = GetMockIndicadoresPorEstado();
        var query = new GetIndicadoresLeitosPorRegiaoQuery { Ano = 2023 };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetIndicadoresLeitosPorEstadoQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockEstados);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull().And.HaveCount(4);
        result.Should().BeInDescendingOrder(x => x.TotalLeitos);

        var sudeste = result.First(r => r.NomeRegiao == "SUDESTE");
        sudeste.Populacao.Should().Be(1500000);
        sudeste.TotalLeitos.Should().Be(1600);
        sudeste.LeitosDisponiveis.Should().Be(300);
        sudeste.Criticos.Should().Be(150);
        sudeste.OcupacaoMedia.Should().Be(81.25);
        sudeste.CoberturaLeitosPor1kHab.Should().Be(1.07);

        var nordeste = result.First(r => r.NomeRegiao == "NORDESTE");
        nordeste.Populacao.Should().Be(1200000);
        nordeste.TotalLeitos.Should().Be(1000);

        var sul = result.First(r => r.NomeRegiao == "SUL");
        sul.Populacao.Should().Be(600000);
        sul.TotalLeitos.Should().Be(500);

        var norte = result.First(r => r.NomeRegiao == "NORTE");
        norte.TotalLeitos.Should().Be(0);
        norte.OcupacaoMedia.Should().Be(0);
        norte.CoberturaLeitosPor1kHab.Should().Be(0);
    }

    [Fact]
    public async Task Handle_QuandoComFiltroDeRegiao_DeveRetornarApenasRegioesFiltradas()
    {
        var mockEstados = GetMockIndicadoresPorEstado();
        var query = new GetIndicadoresLeitosPorRegiaoQuery
            { Ano = 2023, Regioes = new List<string> { "sul", "NORDESTE" } };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetIndicadoresLeitosPorEstadoQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockEstados);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull().And.HaveCount(2);
        result.Select(r => r.NomeRegiao).Should().Contain("SUL");
        result.Select(r => r.NomeRegiao).Should().Contain("NORDESTE");
        result.Select(r => r.NomeRegiao).Should().NotContain("SUDESTE");
    }

    [Fact]
    public async Task Handle_QuandoAnoNulo_DeveChamarHandlerDeEstadoComAnoNulo()
    {
        var query = new GetIndicadoresLeitosPorRegiaoQuery { Ano = null };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetIndicadoresLeitosPorEstadoQuery>(q => q.Ano == null),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<IndicadoresLeitosEstadoDto>());

        await _handler.Handle(query, CancellationToken.None);

        _mediatorMock.Verify(
            m => m.Send(It.Is<GetIndicadoresLeitosPorEstadoQuery>(q => q.Ano == null), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_QuandoHandlerDeEstadoRetornaVazio_DeveRetornarResultadoVazio()
    {
        var query = new GetIndicadoresLeitosPorRegiaoQuery();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetIndicadoresLeitosPorEstadoQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<IndicadoresLeitosEstadoDto>());

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull().And.BeEmpty();
    }
}