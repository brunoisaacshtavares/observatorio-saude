using MediatR;
using observatorio.saude.Domain.Entities;
using observatorio.saude.Domain.Utils;

namespace observatorio.saude.Application.Queries.GetEstabelecimentosPaginados;

public class GetEstabelecimentosPaginadosQuery : IRequest<PaginatedResult<Estabelecimento>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}