using System.Text.Json.Serialization;

/// <summary>
///     Representa uma localidade geográfica (e.g., estado) com seu identificador e nome.
/// </summary>
public class Localidade
{
    /// <summary>
    ///     Identificador da localidade.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}

/// <summary>
///     Representa uma série de dados para uma localidade específica.
/// </summary>
public class Serie
{
    /// <summary>
    ///     A localidade à qual a série de dados se refere.
    /// </summary>
    [JsonPropertyName("localidade")]
    public required Localidade Localidade { get; set; }

    /// <summary>
    ///     Um dicionário contendo os dados da série, onde a chave é o ano e o valor é o dado correspondente.
    /// </summary>
    [JsonPropertyName("serie")]
    public required Dictionary<string, string> SerieData { get; set; }
}

/// <summary>
///     Representa um conjunto de resultados de uma consulta ao IBGE.
/// </summary>
public class Resultado
{
    /// <summary>
    ///     Lista de séries de dados contidas neste resultado.
    /// </summary>
    [JsonPropertyName("series")]
    public required List<Serie> Series { get; set; }
}

/// <summary>
///     Classe de resposta principal para a API do IBGE, contendo metadados da variável e os resultados.
/// </summary>
public class IbgeUfResponse
{
    /// <summary>
    ///     Lista de resultados contendo as séries de dados.
    /// </summary>
    [JsonPropertyName("resultados")]
    public required List<Resultado> Resultados { get; set; }
}

/// <summary>
///     Representa os dados de uma Unidade da Federação.
/// </summary>
public class UfDataResponse
{
    /// <summary>
    ///     Código numérico de identificação da UF.
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    ///     Sigla de duas letras da UF (e.g., "SP", "RJ").
    /// </summary>
    public required string Sigla { get; set; }

    /// <summary>
    ///     Nome completo da UF (e.g., "São Paulo", "Rio de Janeiro").
    /// </summary>
    public required string Nome { get; set; }

    /// <summary>
    ///     A região geográfica à qual a UF pertence.
    /// </summary>
    public required RegiaoResponse Regiao { get; set; }
}

/// <summary>
///     Representa os dados de uma região geográfica do Brasil.
/// </summary>
public class RegiaoResponse
{
    /// <summary>
    ///     Código numérico de identificação da região.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    ///     Sigla da região (e.g., "SE", "SUL").
    /// </summary>
    public required string Sigla { get; set; }

    /// <summary>
    ///     Nome completo da região (e.g., "Sudeste", "Sul").
    /// </summary>
    public required string Nome { get; set; }
}

/// <summary>
///     Contém o resultado da busca de população, incluindo o ano
/// </summary>
public record PopulacaoUfResultado(
    int? AnoEncontrado,
    List<IbgeUfResponse> Dados
);