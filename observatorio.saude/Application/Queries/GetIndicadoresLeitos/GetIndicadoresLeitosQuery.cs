using MediatR;
using observatorio.saude.Domain.Dto;

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
}