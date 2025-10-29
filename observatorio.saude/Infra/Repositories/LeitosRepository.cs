using Microsoft.EntityFrameworkCore;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Enums;
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
    
    public async Task<LeitosAgregadosDto?> GetLeitosAgregadosAsync(int? ano = null, long? anomes = null, TipoLeito? tipo = null)
    {
        var latestRecordsQuery = GetLatestRecords(ano, anomes); 
        
        var aggregatedData = await latestRecordsQuery
            .GroupBy(l => 1)
            .Select(g => new LeitosAgregadosDto
            {
                TotalLeitos = g.Sum(l => !tipo.HasValue ? l.QtdLeitosExistentes :
                    tipo == TipoLeito.UTI_ADULTO ? l.QtdUtiAdultoExist :
                    tipo == TipoLeito.UTI_NEONATAL ? l.QtdUtiNeonatalExist :
                    tipo == TipoLeito.UTI_PEDIATRICO ? l.QtdUtiPediatricoExist :
                    tipo == TipoLeito.UTI_QUEIMADO ? l.QtdUtiQueimadoExist :
                    tipo == TipoLeito.UTI_CORONARIANA ? l.QtdUtiCoronarianaExist :
                    l.QtdLeitosExistentes),

                LeitosSus = g.Sum(l => !tipo.HasValue ? l.QtdLeitosSus :
                    tipo == TipoLeito.UTI_ADULTO ? l.QtdUtiAdultoSus :
                    tipo == TipoLeito.UTI_NEONATAL ? l.QtdUtiNeonatalSus :
                    tipo == TipoLeito.UTI_PEDIATRICO ? l.QtdUtiPediatricoSus :
                    tipo == TipoLeito.UTI_QUEIMADO ? l.QtdUtiQueimadoSus :
                    tipo == TipoLeito.UTI_CORONARIANA ? l.QtdUtiCoronarianaSus :
                    l.QtdLeitosSus),

                TotalUti = g.Sum(l => !tipo.HasValue ? l.QtdUtiTotalExist :
                    tipo == TipoLeito.UTI_ADULTO ? l.QtdUtiAdultoExist :
                    tipo == TipoLeito.UTI_NEONATAL ? l.QtdUtiNeonatalExist :
                    tipo == TipoLeito.UTI_PEDIATRICO ? l.QtdUtiPediatricoExist :
                    tipo == TipoLeito.UTI_QUEIMADO ? l.QtdUtiQueimadoExist :
                    tipo == TipoLeito.UTI_CORONARIANA ? l.QtdUtiCoronarianaExist :
                    l.QtdUtiTotalExist)
            })
            .FirstOrDefaultAsync();

        return aggregatedData;
    }
    
    public async Task<IEnumerable<IndicadoresLeitosEstadoDto>> GetIndicadoresPorEstadoAsync(
        int? ano = null, 
        long? anomes = null, 
        List<long>? codUfs = null, 
        TipoLeito? tipo = null)
    {
        var queryLeitos = GetLatestRecords(ano, anomes);

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
                TotalLeitos = g.Sum(l => !tipo.HasValue ? l.QtdLeitosExistentes :
                    tipo == TipoLeito.UTI_ADULTO ? l.QtdUtiAdultoExist :
                    tipo == TipoLeito.UTI_NEONATAL ? l.QtdUtiNeonatalExist :
                    tipo == TipoLeito.UTI_PEDIATRICO ? l.QtdUtiPediatricoExist :
                    tipo == TipoLeito.UTI_QUEIMADO ? l.QtdUtiQueimadoExist :
                    tipo == TipoLeito.UTI_CORONARIANA ? l.QtdUtiCoronarianaExist :
                    l.QtdLeitosExistentes),

                LeitosSus = g.Sum(l => !tipo.HasValue ? l.QtdLeitosSus :
                    tipo == TipoLeito.UTI_ADULTO ? l.QtdUtiAdultoSus :
                    tipo == TipoLeito.UTI_NEONATAL ? l.QtdUtiNeonatalSus :
                    tipo == TipoLeito.UTI_PEDIATRICO ? l.QtdUtiPediatricoSus :
                    tipo == TipoLeito.UTI_QUEIMADO ? l.QtdUtiQueimadoSus :
                    tipo == TipoLeito.UTI_CORONARIANA ? l.QtdUtiCoronarianaSus :
                    l.QtdLeitosSus),
                
                Criticos = g.Sum(l => !tipo.HasValue ? l.QtdUtiTotalExist :
                    tipo == TipoLeito.UTI_ADULTO ? l.QtdUtiAdultoExist :
                    tipo == TipoLeito.UTI_NEONATAL ? l.QtdUtiNeonatalExist :
                    tipo == TipoLeito.UTI_PEDIATRICO ? l.QtdUtiPediatricoExist :
                    tipo == TipoLeito.UTI_QUEIMADO ? l.QtdUtiQueimadoExist :
                    tipo == TipoLeito.UTI_CORONARIANA ? l.QtdUtiCoronarianaExist :
                    l.QtdUtiTotalExist)
            };

        return await queryFinal.ToListAsync();
    }
    
    public async Task<PaginatedResult<LeitosHospitalarDto>> GetPagedLeitosAsync(
        int pageNumber,
        int pageSize,
        string? nome,
        long? codCnes,
        int? ano,
        long? anomes,
        TipoLeito? tipo,
        long? codUf,
        CancellationToken cancellationToken)
    {
        var queryLeitos = _context.LeitosModel.AsNoTracking();
        
        IQueryable<LeitoModel> latestRecordsQuery;
        if (anomes.HasValue)
        {
            latestRecordsQuery = queryLeitos.Where(l => l.Anomes == anomes.Value);
        }
        else
        {
            var anoParaBuscar = ano ?? DateTime.Now.Year;
            var anoInicio = (long)anoParaBuscar * 100 + 1;
            var anoFim = anoInicio + 11;

            latestRecordsQuery = from leito in queryLeitos
                join latest in queryLeitos
                        .Where(l => l.Anomes >= anoInicio && l.Anomes <= anoFim)
                        .GroupBy(l => l.CodCnes)
                        .Select(g => new { g.Key, MaxAnomes = g.Max(l => l.Anomes) })
                    on new { leito.CodCnes, leito.Anomes } equals new { CodCnes = latest.Key, Anomes = latest.MaxAnomes }
                select leito;
        }

        if (codCnes.HasValue) latestRecordsQuery = latestRecordsQuery.Where(l => l.CodCnes == codCnes.Value);
        if (!string.IsNullOrWhiteSpace(nome))
            latestRecordsQuery =
                latestRecordsQuery.Where(l => EF.Functions.Like(l.NmEstabelecimento, $"%{nome.ToUpper()}%"));
        
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
        
        if (tipo.HasValue)
        {
            if (tipo == TipoLeito.UTI_ADULTO)
                finalQuery = finalQuery.Where(x => x.Leito.QtdUtiAdultoExist > 0);
            else if (tipo == TipoLeito.UTI_NEONATAL)
                finalQuery = finalQuery.Where(x => x.Leito.QtdUtiNeonatalExist > 0);
            else if (tipo == TipoLeito.UTI_PEDIATRICO)
                finalQuery = finalQuery.Where(x => x.Leito.QtdUtiPediatricoExist > 0);
            else if (tipo == TipoLeito.UTI_QUEIMADO)
                finalQuery = finalQuery.Where(x => x.Leito.QtdUtiQueimadoExist > 0);
            else if (tipo == TipoLeito.UTI_CORONARIANA)
                finalQuery = finalQuery.Where(x => x.Leito.QtdUtiCoronarianaExist > 0);
        }

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

                TotalLeitos = !tipo.HasValue ? x.Leito.QtdLeitosExistentes :
                    tipo == TipoLeito.UTI_ADULTO ? x.Leito.QtdUtiAdultoExist :
                    tipo == TipoLeito.UTI_NEONATAL ? x.Leito.QtdUtiNeonatalExist :
                    tipo == TipoLeito.UTI_PEDIATRICO ? x.Leito.QtdUtiPediatricoExist :
                    tipo == TipoLeito.UTI_QUEIMADO ? x.Leito.QtdUtiQueimadoExist :
                    tipo == TipoLeito.UTI_CORONARIANA ? x.Leito.QtdUtiCoronarianaExist :
                    x.Leito.QtdLeitosExistentes,

                LeitosSus = !tipo.HasValue ? x.Leito.QtdLeitosSus :
                    tipo == TipoLeito.UTI_ADULTO ? x.Leito.QtdUtiAdultoSus :
                    tipo == TipoLeito.UTI_NEONATAL ? x.Leito.QtdUtiNeonatalSus :
                    tipo == TipoLeito.UTI_PEDIATRICO ? x.Leito.QtdUtiPediatricoSus :
                    tipo == TipoLeito.UTI_QUEIMADO ? x.Leito.QtdUtiQueimadoSus :
                    tipo == TipoLeito.UTI_CORONARIANA ? x.Leito.QtdUtiCoronarianaSus :
                    x.Leito.QtdLeitosSus
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<LeitosHospitalarDto>(pagedData, pageNumber, pageSize, totalCount);
    }
    
    private IQueryable<LeitoModel> GetLatestRecords(int? ano, long? anomes)
    {
        var query = _context.LeitosModel.AsNoTracking();
        
        if (anomes.HasValue)
        {
            return query.Where(l => l.Anomes == anomes.Value);
        }
        
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