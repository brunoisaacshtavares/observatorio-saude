using Microsoft.EntityFrameworkCore;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;
using observatorio.saude.Infra.Data;

namespace observatorio.saude.Infra.Repositories;

public class LeitosRepository : ILeitosRepository
{
    private readonly ApplicationDbContext _context;

    public LeitosRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<LeitosAgregadosDto?> GetLeitosAgregadosAsync(int? ano = null)
    {
        var query = _context.LeitosModel.AsNoTracking();

        if (ano.HasValue)
        {
            long anoInicio = ano.Value * 100 + 1;
            long anoFim = ano.Value * 100 + 12;
            query = query.Where(l => l.Anomes >= anoInicio && l.Anomes <= anoFim);
        }

        var subqueryLatestEntries = query
            .GroupBy(l => l.CodCnes)
            .Select(g => new
            {
                CodCnes = g.Key,
                MaxAnomes = g.Max(l => l.Anomes)
            });

        var latestRecordsQuery = from leito in query
            join latest in subqueryLatestEntries
                on new { leito.CodCnes, leito.Anomes } equals new
                    { latest.CodCnes, Anomes = latest.MaxAnomes }
            select leito;
        
        var aggregatedData = await latestRecordsQuery
            .GroupBy(l => 1)
            .Select(g => new LeitosAgregadosDto
            {
                TotalLeitos = g.Sum(l => l.QtdLeitosExistentes),
                TotalLeitosSus = g.Sum(l => l.QtdLeitosSus),
                TotalUti = g.Sum(l => l.QtdUtiTotalExist)
            })
            .FirstOrDefaultAsync();

        return aggregatedData;
    }

    public async Task<IEnumerable<IndicadoresLeitosEstadoDto>> GetIndicadoresPorEstadoAsync(int? ano = null,
        List<long>? codUfs = null)
    {
        var queryLeitos = _context.LeitosModel.AsNoTracking();

        if (ano.HasValue)
        {
            long anoInicio = ano.Value * 100 + 1;
            long anoFim = ano.Value * 100 + 12;
            queryLeitos = queryLeitos.Where(l => l.Anomes >= anoInicio && l.Anomes <= anoFim);
        }

        var subqueryLatestEntries = queryLeitos
            .GroupBy(l => l.CodCnes)
            .Select(g => new
            {
                CodCnes = g.Key,
                MaxAnomes = g.Max(l => l.Anomes)
            });
        
        var queryComUf = from leito in queryLeitos
            join latest in subqueryLatestEntries on new { leito.CodCnes, leito.Anomes } equals new
                { latest.CodCnes, Anomes = latest.MaxAnomes }
            join estabelecimento in _context.EstabelecimentoModel.Include(e => e.Localizacao) on leito.CodCnes
                equals estabelecimento.CodCnes
            where estabelecimento.Localizacao != null && estabelecimento.Localizacao.CodUf.HasValue
            select new { Leito = leito, Uf = estabelecimento.Localizacao.CodUf.Value };
        
        if (codUfs != null && codUfs.Any()) queryComUf = queryComUf.Where(x => codUfs.Contains(x.Uf));
        
        var queryFinal = from item in queryComUf
            group item.Leito by item.Uf
            into g
            select new IndicadoresLeitosEstadoDto
            {
                CodUf = g.Key,
                TotalLeitos = g.Sum(l => l.QtdLeitosExistentes),
                LeitosDisponiveis = g.Sum(l => l.QtdLeitosSus),
                Criticos = g.Sum(l => l.QtdUtiTotalExist)
            };

        return await queryFinal.ToListAsync();
    }
}