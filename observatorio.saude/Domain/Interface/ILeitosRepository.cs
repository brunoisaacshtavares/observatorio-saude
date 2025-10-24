using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Enums;
using observatorio.saude.Domain.Utils;

namespace observatorio.saude.Domain.Interface;

public interface ILeitosRepository
{
    Task<LeitosAgregadosDto?> GetLeitosAgregadosAsync(int? ano = null);

    Task<IEnumerable<IndicadoresLeitosEstadoDto>> GetIndicadoresPorEstadoAsync(int? ano = null,
        List<long>? codUfs = null);

    Task<PaginatedResult<LeitosHospitalarDto>> GetPagedLeitosAsync(
        int pageNumber,
        int pageSize,
        string? nome,
        long? codCnes,
        int? ano,
        TipoLeito? tipo,
        long? codUf,
        CancellationToken cancellationToken);

    Task<List<LeitosHospitalarDto>> GetTopLeitosAsync(
        int? ano,
        int topCount,
        long? codUf,
        CancellationToken cancellationToken);
}