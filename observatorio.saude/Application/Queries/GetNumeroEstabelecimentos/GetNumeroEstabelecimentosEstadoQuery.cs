using MediatR;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Application.Queries.GetNumeroEstabelecimentos;

public class GetNumerostabelecimentosPorEstadoQuery : IRequest<IEnumerable<NumeroEstabelecimentoEstadoDto>>
{
}