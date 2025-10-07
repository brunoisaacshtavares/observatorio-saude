using observatorio.saude.Domain.Dto;
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
        long? codUf, // Parâmetro adicionado
        CancellationToken cancellationToken);

    Task<List<LeitosHospitalarDto>> GetTopLeitosAsync(
        int? ano,
        int topCount,
        long? codUf, // Parâmetro adicionado
        CancellationToken cancellationToken);
}