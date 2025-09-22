using MediatR;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Application.Queries.ExportEstabelecimentos;

public class ExportEstabelecimentosDetalhadosQuery : IRequest<IAsyncEnumerable<ExportEstabelecimentoDto>>
{
    /// <summary>
    ///     Filtro opcional pela sigla do estado (ex: "SP").
    /// </summary>
    public string? Uf { get; set; }

}