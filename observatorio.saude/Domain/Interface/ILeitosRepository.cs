using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Enums;
using observatorio.saude.Domain.Utils;

namespace observatorio.saude.Domain.Interface;

public interface ILeitosRepository
{
    Task<LeitosAgregadosDto?> GetLeitosAgregadosAsync(int? ano = null, long? anomes = null, TipoLeito? tipo = null);

    Task<IEnumerable<IndicadoresLeitosEstadoDto>> GetIndicadoresPorEstadoAsync(int? ano = null, long? anomes = null,
        List<long>? codUfs = null, TipoLeito? tipo = null);

    Task<PaginatedResult<LeitosHospitalarDto>> GetPagedLeitosAsync(
        int pageNumber,
        int pageSize,
        string? nome,
        long? codCnes,
        int? ano,
        long? anomes,
        TipoLeito? tipo,
        long? codUf,
        CancellationToken cancellationToken);

    Task<PaginatedResult<LeitosHospitalarDetalhadoDto>> GetDetailedPagedLeitosAsync(
        int pageNumber,
        int pageSize,
        string? nome,
        long? codCnes,
        int? ano,
        long? anomes,
        TipoLeito? tipo,
        long? codUf,
        CancellationToken cancellationToken);
}