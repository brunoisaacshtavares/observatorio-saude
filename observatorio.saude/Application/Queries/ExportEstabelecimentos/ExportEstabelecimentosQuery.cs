using MediatR;

namespace observatorio.saude.Application.Queries.ExportEstabelecimentos;

public class ExportEstabelecimentosQuery : IRequest<ExportFileResult>
{
    /// <summary>
    ///     Filtro opcional pela sigla do estado (ex: "SP").
    /// </summary>
    public string? Uf { get; set; }

    /// <summary>
    ///     Formato do arquivo ("xlsx" ou "csv").
    /// </summary>
    public string Formato { get; set; } = "xlsx";
}

public class ExportFileResult
{
    public required byte[] FileData { get; set; }
    public required string ContentType { get; set; }
    public required string FileName { get; set; }
}