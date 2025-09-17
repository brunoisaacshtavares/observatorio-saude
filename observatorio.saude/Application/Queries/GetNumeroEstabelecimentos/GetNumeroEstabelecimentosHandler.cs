using MediatR;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;

namespace observatorio.saude.Application.Queries.GetNumeroEstabelecimentos;

public class GetNumeroEstabelecimentosHandler(IEstabelecimentoRepository estabelecimentoRepository)
    : IRequestHandler<GetNumeroEstabelecimentosQuery, NumeroEstabelecimentosDto>
{
    public async Task<NumeroEstabelecimentosDto> Handle(GetNumeroEstabelecimentosQuery request,
        CancellationToken cancellationToken)
    {
        var contagemTotal = await estabelecimentoRepository.GetContagemTotalAsync();

        return contagemTotal;
    }
}