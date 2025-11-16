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
            .Setup(r => r.GetLeitosAgregadosAsync(It.IsAny<int?>(), null, null))
            .ReturnsAsync((LeitosAgregadosDto?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalLeitos.Should().Be(0);
        result.LeitosSus.Should().Be(0);
        result.Criticos.Should().Be(0);

        _leitoRepositoryMock.Verify(r => r.GetLeitosAgregadosAsync(query.Ano, null, null), Times.Once);
    }
}