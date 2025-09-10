namespace observatorio.saude.Domain.Utils;

/// <summary>
///     Representa um resultado paginado contendo uma lista de itens e informações de paginação.
/// </summary>
/// <typeparam name="T">Tipo dos itens contidos no resultado.</typeparam>
public class PaginatedResult<T>
{
    /// <summary>
    ///     Inicializa uma nova instância de <see cref="PaginatedResult{T}" />.
    /// </summary>
    /// <param name="items">Lista de itens da página atual.</param>
    /// <param name="currentPage">Número da página atual (1-based).</param>
    /// <param name="pageSize">Quantidade de itens por página.</param>
    /// <param name="totalCount">Total de itens disponíveis.</param>
    public PaginatedResult(List<T> items, int currentPage, int pageSize, int totalCount)
    {
        Items = items;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    /// <summary>
    ///     Obtém ou define a lista de itens da página atual.
    /// </summary>
    public List<T> Items { get; set; }

    /// <summary>
    ///     Obtém ou define o número da página atual (1-based).
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    ///     Obtém ou define a quantidade de itens por página.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    ///     Obtém ou define o total de itens disponíveis na consulta.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    ///     Obtém o total de páginas calculado a partir do total de itens e do tamanho da página.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}