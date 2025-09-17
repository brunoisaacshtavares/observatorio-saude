using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Utils;
using observatorio.saude.Infra.Models;

namespace observatorio.saude.Domain.Interface;

public interface IEstabelecimentoRepository
{
    Task<IEnumerable<NumeroEstabelecimentoEstadoDto>> GetContagemPorEstadoAsync();
    Task<NumeroEstabelecimentosDto> GetContagemTotalAsync();

    Task<PaginatedResult<EstabelecimentoModel>> GetPagedWithDetailsAsync(int pageNumber, int pageSize,
        long? codUf = null);
}