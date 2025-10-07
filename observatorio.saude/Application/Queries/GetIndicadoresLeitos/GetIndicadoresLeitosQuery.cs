using MediatR;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Application.Queries.GetIndicadoresLeitos;

public class GetIndicadoresLeitosQuery : IRequest<IndicadoresLeitosDto>
{
    public int? Ano { get; set; }
}