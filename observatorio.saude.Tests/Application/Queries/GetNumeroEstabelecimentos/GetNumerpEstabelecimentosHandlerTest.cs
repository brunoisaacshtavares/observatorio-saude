using FluentAssertions;
using Moq;
using observatorio.saude.Application.Queries.GetNumeroEstabelecimentos;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;
using Xunit;

namespace observatorio.saude.tests.Application.Queries.GetNumeroEstabelecimentos;

public class GetNumeroEstabelecimentosHandlerTest
{
    private readonly GetNumeroEstabelecimentosHandler _handler;
    private readonly Mock<IEstabelecimentoRepository> _repoMock;

    public GetNumeroEstabelecimentosHandlerTest()
    {
        _repoMock = new Mock<IEstabelecimentoRepository>();
        _handler = new GetNumeroEstabelecimentosHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_QuandoChamado_DeveRetornarContagemTotalCorreta()
    {
        var contagemTotalEsperada = new NumeroEstabelecimentosDto { TotalEstabelecimentos = 12345 };
        _repoMock.Setup(repo => repo.GetContagemTotalAsync()).ReturnsAsync(contagemTotalEsperada);
        var query = new GetNumeroEstabelecimentosQuery();
        
        var result = await _handler.Handle(query, CancellationToken.None);
        
        result.Should().NotBeNull();
        result.TotalEstabelecimentos.Should().Be(contagemTotalEsperada.TotalEstabelecimentos);
        
        _repoMock.Verify(repo => repo.GetContagemTotalAsync(), Times.Once);
    }
}