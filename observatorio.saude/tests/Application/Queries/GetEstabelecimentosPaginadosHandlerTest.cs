using Xunit;
using Moq;
using FluentAssertions;
using observatorio.saude.Domain.Interface;
using observatorio.saude.Application.Queries.GetEstabelecimentosPaginados;
using observatorio.saude.Domain.Utils;
using observatorio.saude.Infra.Models;

namespace observatorio.saude.tests.Application.Queries;

public class GetEstabelecimentosPaginadosHandlerTest
{
    private readonly Mock<IEstabelecimentoRepository> _repositoryMock;
    private readonly GetEstabelecimentosPaginadosHandler _handler;

    public GetEstabelecimentosPaginadosHandlerTest()
    {
        _repositoryMock = new Mock<IEstabelecimentoRepository>();
        _handler = new GetEstabelecimentosPaginadosHandler(_repositoryMock.Object);
    }
    
    private PaginatedResult<EstabelecimentoModel> CriarResultadoFalsoDoRepositorio()
    {
        var itemModel = new EstabelecimentoModel
        {
            CodCnes = 1234567,
            DataExtracao = new DateTime(2025, 09, 08),
            CaracteristicaEstabelecimento = new CaracteristicaEstabelecimentoModel { NmFantasia = "Hospital de Teste" },
            Localizacao = new LocalizacaoModel { Bairro = "Bairro dos Testes" },
            Organizacao = new OrganizacaoModel { TpGestao = 'M' },
            Turno = new TurnoModel { DscrTurnoAtendimento = "24 HORAS" },
            Servico = new ServicoModel { StCentroCirurgico = true, StFazAtendimentoAmbulatorialSus = false }
        };

        var items = new List<EstabelecimentoModel> { itemModel };
        
        return new PaginatedResult<EstabelecimentoModel>(items, 1, 10, 1);
    }

    [Fact]
    public async Task Handle_QuandoRepositorioRetornaDados_DeveMapearCorretamenteERetornarResultadoPaginado()
    {
        var query = new GetEstabelecimentosPaginadosQuery { PageNumber = 1, PageSize = 10 };
        var resultadoFalsoDoRepo = CriarResultadoFalsoDoRepositorio();

        _repositoryMock
            .Setup(r => r.GetPagedWithDetailsAsync(query.PageNumber, query.PageSize))
            .ReturnsAsync(resultadoFalsoDoRepo);
        
        var result = await _handler.Handle(query, CancellationToken.None);
        
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.CurrentPage.Should().Be(resultadoFalsoDoRepo.CurrentPage);
        result.PageSize.Should().Be(resultadoFalsoDoRepo.PageSize);
        result.TotalCount.Should().Be(resultadoFalsoDoRepo.TotalCount);
        
        var itemMapeado = result.Items.First();
        var itemOriginal = resultadoFalsoDoRepo.Items.First();

        itemMapeado.CodCnes.Should().Be(itemOriginal.CodCnes);
        itemMapeado.DataExtracao.Should().Be(itemOriginal.DataExtracao);
        
        itemMapeado.Caracteristicas.Should().NotBeNull();
        itemMapeado.Caracteristicas.NmFantasia.Should().Be(itemOriginal.CaracteristicaEstabelecimento.NmFantasia);

        itemMapeado.Localizacao.Should().NotBeNull();
        itemMapeado.Localizacao.Bairro.Should().Be(itemOriginal.Localizacao.Bairro);

        itemMapeado.Organizacao.Should().NotBeNull();
        itemMapeado.Organizacao.TpGestao.Should().Be(itemOriginal.Organizacao.TpGestao);

        itemMapeado.Turno.Should().NotBeNull();
        itemMapeado.Turno.DscrTurnoAtendimento.Should().Be(itemOriginal.Turno.DscrTurnoAtendimento);

        itemMapeado.Servico.Should().NotBeNull();
        itemMapeado.Servico.TemCentroCirurgico.Should().Be(itemOriginal.Servico.StCentroCirurgico);
        itemMapeado.Servico.FazAtendimentoAmbulatorialSus.Should().Be(itemOriginal.Servico.StFazAtendimentoAmbulatorialSus);
        
        _repositoryMock.Verify(r => r.GetPagedWithDetailsAsync(query.PageNumber, query.PageSize), Times.Once);
    }

    [Fact]
    public async Task Handle_QuandoRepositorioRetornaVazio_DeveRetornarResultadoPaginadoVazio()
    {
        var query = new GetEstabelecimentosPaginadosQuery { PageNumber = 1, PageSize = 10 };
        var resultadoVazioDoRepo = new PaginatedResult<EstabelecimentoModel>(new List<EstabelecimentoModel>(), 1, 10, 0);

        _repositoryMock
            .Setup(r => r.GetPagedWithDetailsAsync(query.PageNumber, query.PageSize))
            .ReturnsAsync(resultadoVazioDoRepo);
        
        var result = await _handler.Handle(query, CancellationToken.None);
        
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }
}