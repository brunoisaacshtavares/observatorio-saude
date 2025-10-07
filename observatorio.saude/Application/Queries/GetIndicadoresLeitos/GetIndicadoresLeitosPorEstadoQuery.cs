using MediatR;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Application.Queries.GetIndicadoresLeitos;

/// <summary>
/// Representa a requisição para obter indicadores de leitos por estado.
/// </summary>
public class GetIndicadoresLeitosPorEstadoQuery : IRequest<IEnumerable<IndicadoresLeitosEstadoDto>>
{
    /// <summary>
    /// O ano para o qual os indicadores devem ser calculados.
    /// </summary>
    public int Ano { get; set; }
    
    /// <summary>
    /// Lista de siglas de UF para filtrar os resultados. Por exemplo: ["SP", "RJ"].
    /// </summary>
    public List<string>? Ufs { get; set; }
}