using Microsoft.EntityFrameworkCore;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;
using observatorio.saude.Domain.Utils;
using observatorio.saude.Infra.Data;
using observatorio.saude.Infra.Models;

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
        var latestRecordsQuery = GetLatestRecords(ano);
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
        var queryLeitos = GetLatestRecords(ano);

        var queryComUf = from leito in queryLeitos
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

    public async Task<PaginatedResult<LeitosHospitalarDto>> GetPagedLeitosAsync(
        int pageNumber,
        int pageSize,
        string? nome,
        long? codCnes,
        int? ano,
        long? codUf,
        CancellationToken cancellationToken)
    {
        var anoParaBuscar = ano ?? DateTime.Now.Year;
        var anoInicio = (long)anoParaBuscar * 100 + 1;
        var anoFim = anoInicio + 11;

        var queryLeitos = _context.LeitosModel.AsNoTracking();

        var latestRecordsQuery = from leito in queryLeitos
            join latest in queryLeitos
                    .Where(l => l.Anomes >= anoInicio && l.Anomes <= anoFim)
                    .GroupBy(l => l.CodCnes)
                    .Select(g => new { g.Key, MaxAnomes = g.Max(l => l.Anomes) })
                on new { leito.CodCnes, leito.Anomes } equals new { CodCnes = latest.Key, Anomes = latest.MaxAnomes }
            select leito;

        if (codCnes.HasValue) latestRecordsQuery = latestRecordsQuery.Where(l => l.CodCnes == codCnes.Value);

        if (!string.IsNullOrWhiteSpace(nome))
            latestRecordsQuery = latestRecordsQuery.Where(l => EF.Functions.Like(l.NmEstabelecimento, $"%{nome.ToUpper()}%"));

        var finalQuery = from leito in latestRecordsQuery
            join estabelecimento in _context.EstabelecimentoModel.AsNoTracking()
                on leito.CodCnes equals estabelecimento.CodCnes
            join localizacao in _context.LocalizacaoModel.AsNoTracking()
                on estabelecimento.CodUnidade equals localizacao.CodUnidade
            where !codUf.HasValue || localizacao.CodUf == codUf
            select new
            {
                Leito = leito,
                Localizacao = localizacao
            };

        var totalCount = await finalQuery.CountAsync(cancellationToken);

        var pagedData = await finalQuery
            .OrderByDescending(x => x.Leito.QtdLeitosExistentes)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new LeitosHospitalarDto
            {
                CodCnes = x.Leito.CodCnes,
                NomeEstabelecimento = x.Leito.NmEstabelecimento,
                LocalizacaoUf = x.Localizacao.CodUf.ToString() ?? "0",
                EnderecoCompleto = $"{x.Localizacao.Endereco}, {x.Localizacao.Numero} - {x.Localizacao.Bairro}",
                TotalLeitos = x.Leito.QtdLeitosExistentes,
                LeitosDisponiveis = x.Leito.QtdLeitosSus
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<LeitosHospitalarDto>(pagedData, pageNumber, pageSize, totalCount);
    }

    public async Task<List<LeitosHospitalarDto>> GetTopLeitosAsync(int? ano, int topCount, long? codUf,
        CancellationToken cancellationToken)
    {
        var anoParaBuscar = ano ?? DateTime.Now.Year;
        var anoInicio = (long)anoParaBuscar * 100 + 1;
        var anoFim = anoInicio + 11;

        var queryLeitos = _context.LeitosModel.AsNoTracking();

        var latestRecordsQuery = from leito in queryLeitos
            join latest in queryLeitos
                    .Where(l => l.Anomes >= anoInicio && l.Anomes <= anoFim)
                    .GroupBy(l => l.CodCnes)
                    .Select(g => new { g.Key, MaxAnomes = g.Max(l => l.Anomes) })
                on new { leito.CodCnes, leito.Anomes } equals new { CodCnes = latest.Key, Anomes = latest.MaxAnomes }
            select leito;

        var finalQuery = from leito in latestRecordsQuery
            join estabelecimento in _context.EstabelecimentoModel.AsNoTracking()
                on leito.CodCnes equals estabelecimento.CodCnes
            join localizacao in _context.LocalizacaoModel.AsNoTracking()
                on estabelecimento.CodUnidade equals localizacao.CodUnidade
            where !codUf.HasValue || localizacao.CodUf == codUf
            select new LeitosHospitalarDto
            {
                CodCnes = leito.CodCnes,
                NomeEstabelecimento = leito.NmEstabelecimento,
                LocalizacaoUf = localizacao.CodUf.ToString() ?? "0",
                EnderecoCompleto = $"{localizacao.Endereco}, {localizacao.Numero} - {localizacao.Bairro}",
                TotalLeitos = leito.QtdLeitosExistentes,
                LeitosDisponiveis = leito.QtdLeitosSus
            };

        var pagedData = await finalQuery
            .Select(dto => new LeitosHospitalarDto
            {
                CodCnes = dto.CodCnes,
                NomeEstabelecimento = dto.NomeEstabelecimento,
                LocalizacaoUf = dto.LocalizacaoUf,
                EnderecoCompleto = dto.EnderecoCompleto,
                TotalLeitos = dto.TotalLeitos,
                LeitosDisponiveis = dto.LeitosDisponiveis,
                PorcentagemOcupacao = dto.TotalLeitos > 0
                    ? (decimal)(dto.TotalLeitos - dto.LeitosDisponiveis) / dto.TotalLeitos * 100
                    : 0
            })
            .OrderByDescending(x => x.PorcentagemOcupacao)
            .ThenBy(x => x.CodCnes)
            .Take(topCount)
            .ToListAsync(cancellationToken);

        return pagedData;
    }

    private IQueryable<LeitoModel> GetLatestRecords(int? ano)
    {
        var query = _context.LeitosModel.AsNoTracking();

        if (ano.HasValue)
        {
            var anoInicio = (long)ano.Value * 100 + 1;
            var anoFim = (long)ano.Value * 100 + 12;
            query = query.Where(l => l.Anomes >= anoInicio && l.Anomes <= anoFim);
        }

        var subqueryLatestEntries = query
            .GroupBy(l => l.CodCnes)
            .Select(g => new
            {
                CodCnes = g.Key,
                MaxAnomes = g.Max(l => l.Anomes)
            });

        return from leito in query
            join latest in subqueryLatestEntries
                on new { leito.CodCnes, leito.Anomes } equals new
                    { latest.CodCnes, Anomes = latest.MaxAnomes }
            select leito;
    }
}