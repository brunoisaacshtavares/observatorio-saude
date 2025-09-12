using FluentAssertions;
using Moq;
using observatorio.saude.Application.Queries.GetNumeroEstabelecimentos;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;
using Xunit;

namespace observatorio.saude.tests.Application.Queries;

public class GetContagemEstabelecimentosPorEstadoQueryHandlerTest
{
    private readonly GetContagemEstabelecimentosPorEstadoQueryHandler _handler;
    private readonly Mock<IEstabelecimentoRepository> _repositoryMock;

    public GetContagemEstabelecimentosPorEstadoQueryHandlerTest()
    {
        _repositoryMock = new Mock<IEstabelecimentoRepository>();
        _handler = new GetContagemEstabelecimentosPorEstadoQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_QuandoRepositorioRetornaDados_DeveRetornarListaDeContagemCorretamente()
    {
        var query = new GetNumerostabelecimentosPorEstadoQuery();

        var resultadoFalsoDoRepo = new List<NumeroEstabelecimentoEstadoDto>
        {
            new() { CodUf = 35, Total = 987 },
            new() { CodUf = 33, Total = 654 }
        };

        _repositoryMock
            .Setup(r => r.GetContagemPorEstadoAsync())
            .ReturnsAsync(resultadoFalsoDoRepo);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(resultadoFalsoDoRepo);
    }
}