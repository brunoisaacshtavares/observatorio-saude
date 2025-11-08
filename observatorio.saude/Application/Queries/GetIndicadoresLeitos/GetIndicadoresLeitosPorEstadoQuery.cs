using MediatR;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Enums;

namespace observatorio.saude.Application.Queries.GetIndicadoresLeitos;

/// <summary>
///     Representa a requisição para obter indicadores de leitos por estado.
/// </summary>
public class GetIndicadoresLeitosPorEstadoQuery : IRequest<IEnumerable<IndicadoresLeitosEstadoDto>>
{
    /// <summary>
    ///     O ano para o qual os indicadores devem ser calculados.
    /// </summary>
    public int? Ano { get; set; }

    /// <summary>
    ///     Lista de siglas de UF para filtrar os resultados. Por exemplo: ["SP", "RJ"].
    /// </summary>
    public List<string>? Ufs { get; set; }

    /// <summary>
    ///     O Tipo de leito para filtrar os estabelecimentos. Por exemplo: UtiAdulto, UtiNeonatal, etc.
    /// </summary>
    public TipoLeito? Tipo { get; set; }

    /// <summary>
    ///     O ano e o mês buscado concatenado em uma string ex: 08/2023 => 202308
    /// </summary>
    public long? Anomes { get; set; }
}