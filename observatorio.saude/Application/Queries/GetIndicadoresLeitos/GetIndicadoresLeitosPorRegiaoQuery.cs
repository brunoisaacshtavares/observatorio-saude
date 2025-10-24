using MediatR;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Application.Queries.GetIndicadoresLeitos;

/// <summary>
///     Representa a requisição para obter indicadores de leitos agregados por região.
/// </summary>
public class GetIndicadoresLeitosPorRegiaoQuery : IRequest<IEnumerable<IndicadoresLeitosRegiaoDto>>
{
    /// <summary>
    ///     O ano para o qual os indicadores devem ser calculados. Se não for informado, usa o ano atual.
    /// </summary>
    public int? Ano { get; set; }

    /// <summary>
    ///     Lista de nomes de regiões para filtrar os resultados. Exemplo: ["NORTE", "SUDESTE"].
    ///     Se vazia, retorna todas as regiões.
    /// </summary>
    public List<string>? Regioes { get; set; }
}