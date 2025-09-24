using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Utils;
using observatorio.saude.Infra.Models;

namespace observatorio.saude.Domain.Interface;

public interface IEstabelecimentoRepository
{
    Task<IEnumerable<NumeroEstabelecimentoEstadoDto>> GetContagemPorEstadoAsync(long? codUf = null);
    Task<NumeroEstabelecimentosDto> GetContagemTotalAsync();

    Task<PaginatedResult<EstabelecimentoModel>> GetPagedWithDetailsAsync(int pageNumber, int pageSize,
        long? codUf = null);

    IAsyncEnumerable<ExportEstabelecimentoDto> StreamAllForExportAsync(List<long>? codUfs = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<GeoFeatureData>> GetWithCoordinatesAsync(
        long? codUf = null,
        double? minLat = null,
        double? maxLat = null,
        double? minLon = null,
        double? maxLon = null);
}