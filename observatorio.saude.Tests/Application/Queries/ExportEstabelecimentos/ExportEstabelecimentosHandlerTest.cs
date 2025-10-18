using FluentAssertions;
using Moq;
using observatorio.saude.Application.Queries.ExportEstabelecimentos;
using observatorio.saude.Application.Services;
using observatorio.saude.Application.Services.Clients;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;

namespace observatorio.saude.tests.Application.Queries.ExportEstabelecimentos;

public class ExportEstabelecimentosHandlerTest
{
    private readonly Mock<IEstabelecimentoRepository> _estabelecimentoRepositoryMock;
    private readonly Mock<IFileExportService> _fileExportServiceMock;
    private readonly ExportEstabelecimentosHandler _handler;
    private readonly Mock<IIbgeApiClient> _ibgeApiClientMock;

    public ExportEstabelecimentosHandlerTest()
    {
        _estabelecimentoRepositoryMock = new Mock<IEstabelecimentoRepository>();
        _fileExportServiceMock = new Mock<IFileExportService>();
        _ibgeApiClientMock = new Mock<IIbgeApiClient>();
        _handler = new ExportEstabelecimentosHandler(
            _estabelecimentoRepositoryMock.Object,
            _ibgeApiClientMock.Object,
            _fileExportServiceMock.Object);

        SetupIbgeMocks();
    }

    private void SetupIbgeMocks()
    {
        var ufs = new List<UfDataResponse>
        {
            new() { Id = 35, Sigla = "SP", Nome = "São Paulo", Regiao = new RegiaoResponse { Nome = "Sudeste" } },
            new() { Id = 33, Sigla = "RJ", Nome = "Rio de Janeiro", Regiao = new RegiaoResponse { Nome = "Sudeste" } },
            new() { Id = 11, Sigla = "RO", Nome = "Rondônia", Regiao = new RegiaoResponse { Nome = "Norte" } }
        };

        var populacao = new List<IbgeUfResponse>
        {
            new()
            {
                Resultados = new List<Resultado>
                {
                    new()
                    {
                        Series = new List<Serie>
                        {
                            new()
                            {
                                Localidade = new Localidade { Id = "35" },
                                SerieData = new Dictionary<string, string> { { "2025", "45919049" } }
                            },
                            new()
                            {
                                Localidade = new Localidade { Id = "33" },
                                SerieData = new Dictionary<string, string> { { "2025", "16388484" } }
                            },
                            new()
                            {
                                Localidade = new Localidade { Id = "11" },
                                SerieData = new Dictionary<string, string> { { "2025", "1815278" } }
                            }
                        }
                    }
                }
            }
        };

        _ibgeApiClientMock.Setup(c => c.FindUfsAsync()).ReturnsAsync(ufs);
        _ibgeApiClientMock.Setup(c => c.FindPopulacaoUfAsync(null)).ReturnsAsync(populacao);
    }

    [Fact]
    public async Task Handle_QuandoFormatoCsvESemFiltroUF_DeveRetornarCsvComDadosAgregados()
    {
        var query = new ExportEstabelecimentosQuery { Formato = "csv", Uf = null };
        var contagem = new List<NumeroEstabelecimentoEstadoDto>
        {
            new() { CodUf = 35, TotalEstabelecimentos = 459 },
            new() { CodUf = 33, TotalEstabelecimentos = 163 }
        };
        var fileBytes = new byte[] { 1, 2, 3, 4 };

        _estabelecimentoRepositoryMock
            .Setup(r => r.GetContagemPorEstadoAsync(It.IsAny<long?>()))
            .ReturnsAsync(contagem);

        _fileExportServiceMock
            .Setup(s => s.GenerateCsv(It.IsAny<IEnumerable<NumeroEstabelecimentoEstadoDto>>()))
            .Returns(fileBytes);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.FileData.Should().BeEquivalentTo(fileBytes);
        result.ContentType.Should().Be("text/csv");
        result.FileName.Should().StartWith("resumo_por_estado_");
        result.FileName.Should().EndWith(".csv");

        _estabelecimentoRepositoryMock.Verify(r => r.GetContagemPorEstadoAsync(null), Times.Once);
        _fileExportServiceMock.Verify(s => s.GenerateCsv(contagem), Times.Once);
        _fileExportServiceMock.Verify(s => s.GenerateExcel(It.IsAny<IEnumerable<NumeroEstabelecimentoEstadoDto>>()),
            Times.Never);

        contagem.Should().ContainSingle(item =>
            item.CodUf == 35 &&
            item.Populacao == 45919049 &&
            item.SiglaUf == "SP" &&
            item.Regiao == "Sudeste");
    }

    [Fact]
    public async Task Handle_QuandoFormatoExcelEComFiltroUFValido_DeveRetornarXlsxComFiltroAplicado()
    {
        var query = new ExportEstabelecimentosQuery { Formato = "xlsx", Uf = "RJ" };
        var contagem = new List<NumeroEstabelecimentoEstadoDto>
        {
            new() { CodUf = 33, TotalEstabelecimentos = 163 }
        };
        var fileBytes = new byte[] { 5, 6, 7, 8 };

        _estabelecimentoRepositoryMock
            .Setup(r => r.GetContagemPorEstadoAsync(33))
            .ReturnsAsync(contagem);

        _fileExportServiceMock
            .Setup(s => s.GenerateExcel(It.IsAny<IEnumerable<NumeroEstabelecimentoEstadoDto>>()))
            .Returns(fileBytes);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.FileData.Should().BeEquivalentTo(fileBytes);
        result.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        result.FileName.Should().EndWith(".xlsx");

        _estabelecimentoRepositoryMock.Verify(r => r.GetContagemPorEstadoAsync(33), Times.Once);
        _fileExportServiceMock.Verify(s => s.GenerateExcel(contagem), Times.Once);

        contagem.Should().ContainSingle(item =>
            item.CodUf == 33 &&
            item.Populacao == 16388484 &&
            item.SiglaUf == "RJ" &&
            item.Regiao == "Sudeste");
    }

    [Theory]
    [InlineData("PDF")]
    [InlineData("XLSX")]
    public async Task Handle_QuandoFormatoDiferenteDeCsv_DeveGerarExcel(string formato)
    {
        var query = new ExportEstabelecimentosQuery { Formato = formato, Uf = null };
        var contagem = new List<NumeroEstabelecimentoEstadoDto>();
        var fileBytes = new byte[] { 10, 20 };

        _estabelecimentoRepositoryMock
            .Setup(r => r.GetContagemPorEstadoAsync(It.IsAny<long?>()))
            .ReturnsAsync(contagem);

        _fileExportServiceMock
            .Setup(s => s.GenerateExcel(It.IsAny<IEnumerable<NumeroEstabelecimentoEstadoDto>>()))
            .Returns(fileBytes);

        var result = await _handler.Handle(query, CancellationToken.None);

        _fileExportServiceMock.Verify(s => s.GenerateExcel(contagem), Times.Once);
        _fileExportServiceMock.Verify(s => s.GenerateCsv(It.IsAny<IEnumerable<NumeroEstabelecimentoEstadoDto>>()),
            Times.Never);
        result.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Fact]
    public async Task Handle_QuandoUFInvalida_DeveIgnorarFiltroEBuscarTodos()
    {
        var query = new ExportEstabelecimentosQuery { Formato = "csv", Uf = "XX" };
        var contagem = new List<NumeroEstabelecimentoEstadoDto>();

        _estabelecimentoRepositoryMock
            .Setup(r => r.GetContagemPorEstadoAsync(null))
            .ReturnsAsync(contagem);

        await _handler.Handle(query, CancellationToken.None);

        _estabelecimentoRepositoryMock.Verify(r => r.GetContagemPorEstadoAsync(null), Times.Once);
        _estabelecimentoRepositoryMock.Verify(r => r.GetContagemPorEstadoAsync(It.IsAny<long>()), Times.Never);
    }

    [Fact]
    public async Task Handle_QuandoPopulacaoZero_DeveCalcularCoberturaComoZero()
    {
        var query = new ExportEstabelecimentosQuery { Formato = "csv" };

        var contagem = new List<NumeroEstabelecimentoEstadoDto>
        {
            new() { CodUf = 11, TotalEstabelecimentos = 500 }
        };

        var populacaoZero = new List<IbgeUfResponse>
        {
            new()
            {
                Resultados = new List<Resultado>
                {
                    new()
                    {
                        Series = new List<Serie>
                        {
                            new()
                            {
                                Localidade = new Localidade { Id = "35" },
                                SerieData = new Dictionary<string, string> { { "2025", "1" } }
                            },
                            new()
                            {
                                Localidade = new Localidade { Id = "33" },
                                SerieData = new Dictionary<string, string> { { "2025", "1" } }
                            }
                        }
                    }
                }
            }
        };

        _ibgeApiClientMock.Setup(c => c.FindPopulacaoUfAsync(null)).ReturnsAsync(populacaoZero);

        _estabelecimentoRepositoryMock
            .Setup(r => r.GetContagemPorEstadoAsync(It.IsAny<long?>()))
            .ReturnsAsync(contagem);

        _fileExportServiceMock
            .Setup(s => s.GenerateCsv(It.IsAny<IEnumerable<NumeroEstabelecimentoEstadoDto>>()))
            .Returns(Array.Empty<byte>());

        await _handler.Handle(query, CancellationToken.None);

        contagem.Should().ContainSingle(item =>
            item.CodUf == 11 &&
            item.Populacao == 0 &&
            item.CoberturaEstabelecimentos == 0);
    }
}