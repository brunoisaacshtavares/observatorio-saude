using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using observatorio.saude.Infra.Data;
using observatorio.saude.Infra.Models;
using observatorio.saude.Infra.Repositories;
using Xunit;

namespace observatorio.saude.tests.Infra.Repositories;

public class EstabelecimentoRepositoryTest : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly EstabelecimentoRepository _repository;

    public EstabelecimentoRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new EstabelecimentoRepository(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    private async Task SeedDatabaseAsync(int count)
    {
        var turnos = new List<TurnoModel>
        {
            new() { CodTurnoAtendimento = 1, DscrTurnoAtendimento = "MANHA" },
            new() { CodTurnoAtendimento = 2, DscrTurnoAtendimento = "TARDE" },
            new() { CodTurnoAtendimento = 3, DscrTurnoAtendimento = "MANHA E TARDE" }
        };
        await _context.TurnoModel.AddRangeAsync(turnos);

        var estabelecimentos = new List<EstabelecimentoModel>();
        for (var i = 1; i <= count; i++)
        {
            var codCnes = 100L + i;
            var codUnidade = $"UNIDADE_TESTE_{i}";
            long codTurno = i % 3 + 1;

            var estabelecimento = new EstabelecimentoModel
            {
                CodCnes = codCnes,
                DataExtracao = new DateTime(2025, 9, 8),
                CodUnidade = codUnidade,
                CodTurnoAtendimento = codTurno,
                CaracteristicaEstabelecimento = new CaracteristicaEstabelecimentoModel
                {
                    CodUnidade = codUnidade,
                    NmFantasia = $"Fantasia {i}"
                },
                Localizacao = new LocalizacaoModel
                {
                    CodUnidade = codUnidade,
                    Bairro = "Bairro Teste"
                },
                Organizacao = new OrganizacaoModel
                {
                    CodCnes = codCnes,
                    TpGestao = 'M'
                },
                Servico = new ServicoModel
                {
                    CodCnes = codCnes,
                    StCentroCirurgico = true
                }
            };
            estabelecimentos.Add(estabelecimento);
        }

        await _context.EstabelecimentoModel.AddRangeAsync(estabelecimentos);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetContagemPorEstadoAsync_QuandoDadosExistem_DeveAgruparEContarCorretamente()
    {
        var estabelecimentos = new List<EstabelecimentoModel>
        {
            new()
            {
                CodCnes = 1, CodUnidade = "U1", Localizacao = new LocalizacaoModel { CodUnidade = "U1", CodUf = 35 }
            },
            new()
            {
                CodCnes = 2, CodUnidade = "U2", Localizacao = new LocalizacaoModel { CodUnidade = "U2", CodUf = 35 }
            },
            new()
            {
                CodCnes = 3, CodUnidade = "U3", Localizacao = new LocalizacaoModel { CodUnidade = "U3", CodUf = 35 }
            },
            new()
            {
                CodCnes = 4, CodUnidade = "U4", Localizacao = new LocalizacaoModel { CodUnidade = "U4", CodUf = 33 }
            },
            new()
            {
                CodCnes = 5, CodUnidade = "U5", Localizacao = new LocalizacaoModel { CodUnidade = "U5", CodUf = 33 }
            },
            new()
            {
                CodCnes = 6, CodUnidade = "U6", Localizacao = new LocalizacaoModel { CodUnidade = "U6", CodUf = null }
            },
            new() { CodCnes = 7, CodUnidade = "U7", Localizacao = null }
        };
        await _context.EstabelecimentoModel.AddRangeAsync(estabelecimentos);
        await _context.SaveChangesAsync();

        var result = await _repository.GetContagemPorEstadoAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var resultadoLista = result.ToList();

        resultadoLista[0].CodUf.Should().Be(35);
        resultadoLista[0].Total.Should().Be(3);

        resultadoLista[1].CodUf.Should().Be(33);
        resultadoLista[1].Total.Should().Be(2);
    }
    
    [Fact]
    public async Task GetPagedWithDetailsAsync_QuandoDadosExistem_DeveRetornarResultadoPaginadoCorretamente()
    {
        await SeedDatabaseAsync(12);
        const int pageNumber = 2;
        const int pageSize = 5;

        var result = await _repository.GetPagedWithDetailsAsync(pageNumber, pageSize);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(pageSize);
        result.CurrentPage.Should().Be(pageNumber);
        result.PageSize.Should().Be(pageSize);
        result.TotalCount.Should().Be(12);
        result.TotalPages.Should().Be(3);
        result.Items.First().CodCnes.Should().Be(106);
        result.Items.Last().CodCnes.Should().Be(110);
        result.Items.First().CaracteristicaEstabelecimento.Should().NotBeNull();
        result.Items.First().Localizacao.Should().NotBeNull();
        result.Items.First().Organizacao.Should().NotBeNull();
        result.Items.First().Turno.Should().NotBeNull();
        result.Items.First().Servico.Should().NotBeNull();
    }

    [Fact]
    public async Task GetPagedWithDetailsAsync_QuandoRequisitandoUltimaPagina_DeveRetornarItensRestantes()
    {
        await SeedDatabaseAsync(12);
        const int pageNumber = 3;
        const int pageSize = 5;

        var result = await _repository.GetPagedWithDetailsAsync(pageNumber, pageSize);

        result.Should().NotBeNull();
        result.TotalCount.Should().Be(12);
        result.TotalPages.Should().Be(3);
        result.Items.Should().HaveCount(2);
        result.Items.First().CodCnes.Should().Be(111);
        result.Items.Last().CodCnes.Should().Be(112);
    }

    [Fact]
    public async Task GetPagedWithDetailsAsync_QuandoNaoHaDados_DeveRetornarResultadoVazio()
    {
        var result = await _repository.GetPagedWithDetailsAsync(1, 10);

        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }
}