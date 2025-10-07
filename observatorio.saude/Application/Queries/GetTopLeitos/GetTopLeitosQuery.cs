using MediatR;
using observatorio.saude.Domain.Dto;
using System.ComponentModel;

namespace observatorio.saude.Application.Queries.GetTopLeitos;

/// <summary>
/// Representa uma requisição para obter a lista de hospitais com maior ocupação.
/// </summary>
public class GetTopLeitosQuery : IRequest<List<LeitosHospitalarDto>>
{
    /// <summary>
    /// O número de itens a ser retornado na lista de top hospitais. O padrão é 10.
    /// </summary>
    [DefaultValue(10)]
    public int Count { get; set; } = 10;
    
    /// <summary>
    /// O ano para o qual a ocupação deve ser calculada.
    /// Caso não seja informado, o ano atual será utilizado.
    /// </summary>
    public int? Ano { get; set; }
    
    /// <summary>
    /// O código UF (Unidade Federativa) para filtrar os estabelecimentos. Por exemplo: "SP", "RJ", "MG".
    /// </summary>
    public string? Uf { get; set; }
}