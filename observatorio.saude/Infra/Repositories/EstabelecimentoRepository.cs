using Microsoft.EntityFrameworkCore;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;
using observatorio.saude.Domain.Utils;
using observatorio.saude.Infra.Data;
using observatorio.saude.Infra.Models;

namespace observatorio.saude.Infra.Repositories;

public class EstabelecimentoRepository(ApplicationDbContext context) : IEstabelecimentoRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<NumeroEstabelecimentoEstadoDto>> GetContagemPorEstadoAsync(long? codUf = null)
    {
        var query = _context.EstabelecimentoModel
            .AsNoTracking()
            .Where(e => e.Localizacao.CodUf != null);

        if (codUf.HasValue)
        {
            query = query.Where(e => e.Localizacao.CodUf == codUf.Value);
        }
    
        var contagemPorEstado = await query
            .GroupBy(e => e.Localizacao.CodUf)
            .Select(g => new NumeroEstabelecimentoEstadoDto
            {
                CodUf = g.Key.Value,
                TotalEstabelecimentos = g.Count()
            })
            .OrderByDescending(r => r.TotalEstabelecimentos)
            .ToListAsync();

        return contagemPorEstado;
    }

    public async Task<NumeroEstabelecimentosDto> GetContagemTotalAsync()
    {
        var total = await _context.EstabelecimentoModel.CountAsync();

        return new NumeroEstabelecimentosDto { TotalEstabelecimentos = total };
    }

    public async Task<PaginatedResult<EstabelecimentoModel>> GetPagedWithDetailsAsync(int pageNumber, int pageSize,
        long? codUf = null)
    {
        var query = _context.EstabelecimentoModel
            .Include(e => e.CaracteristicaEstabelecimento)
            .Include(e => e.Localizacao)
            .Include(e => e.Organizacao)
            .Include(e => e.Turno)
            .Include(e => e.Servico)
            .AsQueryable();

        if (codUf.HasValue) query = query.Where(e => e.Localizacao.CodUf == codUf.Value);
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(e => e.CodCnes)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<EstabelecimentoModel>(items, pageNumber, pageSize, totalCount);
    }

    public async IAsyncEnumerable<ExportEstabelecimentoDto> StreamAllForExportAsync(long? codUf = null)
    {
        var query = _context.EstabelecimentoModel
            .AsNoTracking()
            .Select(e => new ExportEstabelecimentoDto
            {
                CodCnes = e.CodCnes,
                RazaoSocial = e.CaracteristicaEstabelecimento.NmRazaoSocial,
                NomeFantasia = e.CaracteristicaEstabelecimento.NmFantasia,
                Endereco = e.Localizacao.Endereco + ", " + e.Localizacao.Numero,
                Bairro = e.Localizacao.Bairro,
                Cep = e.Localizacao.CodCep.ToString(),
                CodUfParaMapeamento = e.Localizacao.CodUf,
                EsferaAdministrativa = e.Organizacao.DscrEsferaAdministrativa
            });

        if (codUf.HasValue) query = query.Where(e => e.CodUfParaMapeamento == codUf.Value);

        await foreach (var item in query.AsAsyncEnumerable()) yield return item;
    }
}