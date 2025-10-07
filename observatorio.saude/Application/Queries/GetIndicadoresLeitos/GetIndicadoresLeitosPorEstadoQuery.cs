using MediatR;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Application.Queries.GetIndicadoresLeitos;

public class GetIndicadoresLeitosPorEstadoQuery : IRequest<IEnumerable<IndicadoresLeitosEstadoDto>>
{
    public int Ano { get; set; }
    public List<string>? Ufs { get; set; }
}