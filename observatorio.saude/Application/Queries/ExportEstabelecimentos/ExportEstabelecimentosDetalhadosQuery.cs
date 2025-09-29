using MediatR;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Application.Queries.ExportEstabelecimentos;

public class ExportEstabelecimentosDetalhadosQuery : IRequest<IAsyncEnumerable<ExportEstabelecimentoDto>>
{
    /// <summary>
    ///     Filtro opcional pela sigla do estado (ex: ["SP", "RJ"]).
    /// </summary>
    public IEnumerable<string>? Uf { get; set; }

    /// <summary>
    ///     Define o formato do arquivo para exportação.
    ///     Valores válidos: "csv" ou "xlsx". O padrão é "csv".
    /// </summary>
    public string Format { get; set; } = "csv";
}