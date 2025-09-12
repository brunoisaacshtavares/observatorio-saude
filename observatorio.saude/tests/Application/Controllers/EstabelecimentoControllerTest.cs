using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using observatorio.saude.Application.Controllers;
using observatorio.saude.Application.Queries.GetNumeroEstabelecimentos;
using observatorio.saude.Domain.Dto;
using Xunit;

namespace observatorio.saude.tests.Application.Controllers;

public class EstabelecimentoControllerTest
{
    private readonly EstabelecimentoController _controller;
    private readonly Mock<IMediator> _mediatorMock;

    public EstabelecimentoControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new EstabelecimentoController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetContagemPorEstado_QuandoChamado_DeveRetornarOkComListaDeContagem()
    {
        var resultadoEsperado = new List<NumeroEstabelecimentoEstadoDto>
        {
            new() { CodUf = 35, Total = 1500 },
            new() { CodUf = 33, Total = 1200 }
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
}