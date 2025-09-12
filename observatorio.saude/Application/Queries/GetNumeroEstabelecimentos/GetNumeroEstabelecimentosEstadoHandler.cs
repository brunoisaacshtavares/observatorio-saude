using MediatR;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;

namespace observatorio.saude.Application.Queries.GetNumeroEstabelecimentos;

public class GetContagemEstabelecimentosPorEstadoQueryHandler(IEstabelecimentoRepository estabelecimentoRepository)
    : IRequestHandler<GetNumerostabelecimentosPorEstadoQuery, IEnumerable<NumeroEstabelecimentoEstadoDto>>
{
    public async Task<IEnumerable<NumeroEstabelecimentoEstadoDto>> Handle(
        GetNumerostabelecimentosPorEstadoQuery request, CancellationToken cancellationToken)
    {
        var contagemPorEstado = await estabelecimentoRepository.GetContagemPorEstadoAsync();

        return contagemPorEstado;
    }
}