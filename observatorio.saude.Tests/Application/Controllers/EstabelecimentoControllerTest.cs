using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using observatorio.saude.Application.Controllers;
using observatorio.saude.Application.Queries.GetEstabelecimentosPaginados;
using observatorio.saude.Application.Queries.GetNumeroEstabelecimentos;
using observatorio.saude.Application.Services;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Entities;
using observatorio.saude.Domain.Utils;

namespace observatorio.saude.tests.Application.Controllers;

public class EstabelecimentoControllerTest
{
    private readonly EstabelecimentoController _controller;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IFileExportService > _fileExportServiceMock;

    public EstabelecimentoControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _fileExportServiceMock = new Mock<IFileExportService>();
        _controller = new EstabelecimentoController(_mediatorMock.Object, _fileExportServiceMock.Object);
    }

    [Fact]
    public async Task GetContagemPorEstado_QuandoChamado_DeveRetornarOkComListaDeContagem()
    {
        var resultadoEsperado = new List<NumeroEstabelecimentoEstadoDto>
        {
            new() { CodUf = 35, TotalEstabelecimentos = 1500 },
            new() { CodUf = 33, TotalEstabelecimentos = 1200 }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetNumerostabelecimentosPorEstadoQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultadoEsperado);

        var actionResult = await _controller.GetNumeroPorEstado();

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(resultadoEsperado);

        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetNumerostabelecimentosPorEstadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetEstabelecimentos_QuandoChamado_DeveRetornarOkComResultadoPaginado()
    {
        var paginadosQuery = new GetEstabelecimentosPaginadosQuery { PageNumber = 1, PageSize = 10 };

        var resultadoPaginadoEsperado = new PaginatedResult<Estabelecimento>(
            new List<Estabelecimento> { new() { CodCnes = 12345 } }, 1, 1, 10
        );

        _mediatorMock
            .Setup(m => m.Send(paginadosQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultadoPaginadoEsperado);

        var actionResult = await _controller.GetEstabelecimentos(paginadosQuery);

        actionResult
            .Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);

        actionResult
            .Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(resultadoPaginadoEsperado);

        _mediatorMock.Verify(m => m.Send(paginadosQuery, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetNumero_QuandoChamado_DeveRetornarOkComNumeroTotal()
    {
        var resultadoEsperado = new NumeroEstabelecimentosDto { TotalEstabelecimentos = 5000 };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetNumeroEstabelecimentosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultadoEsperado);

        var actionResult = await _controller.GetNumero();

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(resultadoEsperado);

        _mediatorMock.Verify(m => m.Send(It.IsAny<GetNumeroEstabelecimentosQuery>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}