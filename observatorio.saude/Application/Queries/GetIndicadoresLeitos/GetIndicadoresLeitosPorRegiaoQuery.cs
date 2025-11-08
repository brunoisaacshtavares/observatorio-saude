// observatorio.saude.Application.Queries.GetIndicadoresLeitos.GetIndicadoresLeitosPorRegiaoQuery.cs

using MediatR;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Enums;

// Adicionar using

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

    /// <summary>
    ///     O Tipo de leito para filtrar os estabelecimentos. Por exemplo: UtiAdulto, UtiNeonatal, etc.
    /// </summary>
    public TipoLeito? Tipo { get; set; }

    /// <summary>
    ///     O ano e o mês buscado concatenado em uma string ex: 08/2023 => 202308
    /// </summary>
    public long? Anomes { get; set; }
}