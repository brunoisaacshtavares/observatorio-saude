using MediatR;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Enums;

namespace observatorio.saude.Application.Queries.GetIndicadoresLeitos;

/// <summary>
///     Representa a requisição para obter indicadores agregados de leitos.
/// </summary>
public class GetIndicadoresLeitosQuery : IRequest<IndicadoresLeitosDto>
{
    /// <summary>
    ///     O ano para o qual os indicadores devem ser calculados.
    /// </summary>
    public int? Ano { get; set; }

    /// <summary>
    ///     O Tipo de leito para filtrar os indicadores. Por exemplo: UtiAdulto, UtiNeonatal, etc.
    /// </summary>
    public TipoLeito? Tipo { get; set; }
    
    /// <summary>
    ///     O ano e o mês buscado concatenado em uma string ex: 08/2023 => 202308
    /// </summary>
    public long? Anomes { get; set; }
}