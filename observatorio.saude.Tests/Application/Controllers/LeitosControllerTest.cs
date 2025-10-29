using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using observatorio.saude.Application.Controllers;
using observatorio.saude.Application.Queries.GetIndicadoresLeitos;
using observatorio.saude.Application.Queries.GetLeitosPaginados;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Utils;

namespace observatorio.saude.tests.Application.Controllers;

public class LeitosControllerTest
{
    private readonly LeitosController _controller;
    private readonly Mock<IMediator> _mediatorMock;

    public LeitosControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new LeitosController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetIndicadores_QuandoChamado_DeveRetornarOkComIndicadores()
    {
        var query = new GetIndicadoresLeitosQuery { Ano = 2025 };
        var resultadoEsperado = new IndicadoresLeitosDto { TotalLeitos = 5000 };

        _mediatorMock
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultadoEsperado);

        var actionResult = await _controller.GetIndicadores(query);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(resultadoEsperado);

        _mediatorMock.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetIndicadoresPorEstado_QuandoChamado_DeveRetornarOkComListaDeIndicadores()
    {
        var query = new GetIndicadoresLeitosPorEstadoQuery { Ufs = ["DF"] };
        var resultadoEsperado = new List<IndicadoresLeitosEstadoDto>
        {
            new() { CodUf = 35, TotalLeitos = 1500}
        };

        _mediatorMock
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultadoEsperado);

        var actionResult = await _controller.GetIndicadoresPorEstado(query);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(resultadoEsperado);

        _mediatorMock.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetLeitos_QuandoChamado_DeveRetornarOkComResultadoPaginado()
    {
        var paginadosQuery = new GetLeitosPaginadosQuery { PageNumber = 1, PageSize = 10 };

        var resultadoPaginadoEsperado = new PaginatedResult<LeitosHospitalarDto>(
            new List<LeitosHospitalarDto> { new() { CodCnes = 12345 } }, 1, 1, 10
        );

        _mediatorMock
            .Setup(m => m.Send(paginadosQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultadoPaginadoEsperado);

        var actionResult = await _controller.GetLeitos(paginadosQuery);

        actionResult
            .Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);

        actionResult
            .Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(resultadoPaginadoEsperado);

        _mediatorMock.Verify(m => m.Send(paginadosQuery, It.IsAny<CancellationToken>()), Times.Once);
    }
}