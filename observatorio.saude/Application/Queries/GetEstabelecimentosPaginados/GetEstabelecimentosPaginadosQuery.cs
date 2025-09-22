using System.ComponentModel;
using MediatR;
using observatorio.saude.Domain.Entities;
using observatorio.saude.Domain.Utils;

namespace observatorio.saude.Application.Queries.GetEstabelecimentosPaginados;

public class GetEstabelecimentosPaginadosQuery : IRequest<PaginatedResult<Estabelecimento>>
{
    /// <summary>
    ///     O número da página a ser retornada. O padrão é 1.
    /// </summary>
    [DefaultValue(1)]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    ///     O número de itens por página. O padrão é 10.
    /// </summary>
    [DefaultValue(10)]
    public int PageSize { get; set; } = 10;

    /// <summary>
    ///     O código UF (Unidade Federativa) para filtrar os estabelecimentos. Por exemplo: "SP", "RJ", "MG".
    /// </summary>
    public string? Uf { get; set; }
}