using Microsoft.EntityFrameworkCore;
using observatorio.saude.Infra.Data;
using observatorio.saude.Domain.Interface;
using observatorio.saude.Domain.Utils;
using observatorio.saude.Infra.Models;

namespace observatorio.saude.Infra.Repositories;

public class EstabelecimentoRepository(ApplicationDbContext context) : IEstabelecimentoRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<PaginatedResult<EstabelecimentoModel>> GetPagedWithDetailsAsync(int pageNumber, int pageSize)
    {
        var query = _context.EstabelecimentoModel
            .Include(e => e.CaracteristicaEstabelecimento)
            .Include(e => e.Localizacao)
            .Include(e => e.Organizacao)
            .Include(e => e.Turno)
            .Include(e => e.Servico)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(e => e.CodCnes)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<EstabelecimentoModel>(items, pageNumber, pageSize, totalCount);
    }
}