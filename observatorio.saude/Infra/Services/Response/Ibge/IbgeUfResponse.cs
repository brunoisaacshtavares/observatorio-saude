using System.Text.Json.Serialization;

/// <summary>
///     Representa o nível de agregação dos dados (e.g., Unidade da Federação).
/// </summary>
public class Nivel
{
    /// <summary>
    ///     Identificador do nível de agregação.
    /// </summary>
    [JsonPropertyName("id")] 
    public string Id { get; set; }

    /// <summary>
    ///     Nome do nível de agregação.
    /// </summary>
    [JsonPropertyName("nome")] 
    public string Nome { get; set; }
}

/// <summary>
///     Representa uma localidade geográfica (e.g., estado) com seu identificador e nome.
/// </summary>
public class Localidade
{
    /// <summary>
    ///     Identificador da localidade.
    /// </summary>
    [JsonPropertyName("id")] 
    public string Id { get; set; }

    /// <summary>
    ///     Nível de agregação da localidade.
    /// </summary>
    [JsonPropertyName("nivel")] 
    public Nivel Nivel { get; set; }

    /// <summary>
    ///     Nome da localidade.
    /// </summary>
    [JsonPropertyName("nome")] 
    public string Nome { get; set; }
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
    public Localidade Localidade { get; set; }

    /// <summary>
    ///     Um dicionário contendo os dados da série, onde a chave é o ano e o valor é o dado correspondente.
    /// </summary>
    [JsonPropertyName("serie")] 
    public Dictionary<string, string> SerieData { get; set; }
}

/// <summary>
///     Representa um conjunto de resultados de uma consulta ao IBGE.
/// </summary>
public class Resultado
{
    /// <summary>
    ///     Lista de classificações aplicadas na consulta.
    /// </summary>
    [JsonPropertyName("classificacoes")] 
    public List<object> Classificacoes { get; set; }

    /// <summary>
    ///     Lista de séries de dados contidas neste resultado.
    /// </summary>
    [JsonPropertyName("series")] 
    public List<Serie> Series { get; set; }
}

/// <summary>
///     Classe de resposta principal para a API do IBGE, contendo metadados da variável e os resultados.
/// </summary>
public class IbgeUfResponse
{
    /// <summary>
    ///     Identificador da variável consultada.
    /// </summary>
    [JsonPropertyName("id")] 
    public string Id { get; set; }

    /// <summary>
    ///     Nome da variável consultada.
    /// </summary>
    [JsonPropertyName("variavel")] 
    public string Variavel { get; set; }

    /// <summary>
    ///     Unidade de medida da variável.
    /// </summary>
    [JsonPropertyName("unidade")] 
    public string Unidade { get; set; }

    /// <summary>
    ///     Lista de resultados contendo as séries de dados.
    /// </summary>
    [JsonPropertyName("resultados")] 
    public List<Resultado> Resultados { get; set; }
}

/// <summary>
///     Representa os dados de uma Unidade da Federação.
/// </summary>
public class UfDataResponse
{
    /// <summary>
    ///     Código numérico de identificação da UF.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    ///     Sigla de duas letras da UF (e.g., "SP", "RJ").
    /// </summary>
    public string Sigla { get; set; }
    
    /// <summary>
    ///     Nome completo da UF (e.g., "São Paulo", "Rio de Janeiro").
    /// </summary>
    public string Nome { get; set; }
    
    /// <summary>
    ///     A região geográfica à qual a UF pertence.
    /// </summary>
    public RegiaoResponse Regiao { get; set; }
}

/// <summary>
///     Representa os dados de uma região geográfica do Brasil.
/// </summary>
public class RegiaoResponse
{
    /// <summary>
    ///     Código numérico de identificação da região.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    ///     Sigla da região (e.g., "SE", "SUL").
    /// </summary>
    public string Sigla { get; set; }
    
    /// <summary>
    ///     Nome completo da região (e.g., "Sudeste", "Sul").
    /// </summary>
    public string Nome { get; set; }
}