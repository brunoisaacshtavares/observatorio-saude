using observatorio.saude.Domain.Utils;
using observatorio.saude.Infra.Models;

namespace observatorio.saude.Domain.Interface;

public interface IEstabelecimentoRepository
{
    Task<PaginatedResult<EstabelecimentoModel>> GetPagedWithDetailsAsync(int pageNumber, int pageSize);
}