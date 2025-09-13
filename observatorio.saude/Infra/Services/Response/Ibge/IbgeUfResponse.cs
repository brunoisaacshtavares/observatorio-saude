using System.Text.Json.Serialization;

public class Nivel
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("nome")] public string Nome { get; set; }
}

public class Localidade
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("nivel")] public Nivel Nivel { get; set; }

    [JsonPropertyName("nome")] public string Nome { get; set; }
}

public class Serie
{
    [JsonPropertyName("localidade")] public Localidade Localidade { get; set; }

    [JsonPropertyName("serie")] public Dictionary<string, string> SerieData { get; set; }
}

public class Resultado
{
    [JsonPropertyName("classificacoes")] public List<object> Classificacoes { get; set; }

    [JsonPropertyName("series")] public List<Serie> Series { get; set; }
}

public class IbgeUfResponse
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("variavel")] public string Variavel { get; set; }

    [JsonPropertyName("unidade")] public string Unidade { get; set; }

    [JsonPropertyName("resultados")] public List<Resultado> Resultados { get; set; }
}

public class UfDataResponse
{
    public long Id { get; set; }
    public string Sigla { get; set; }
    public string Nome { get; set; }
    public RegiaoResponse Regiao { get; set; }
}

public class RegiaoResponse
{
    public int Id { get; set; }
    public string Sigla { get; set; }
    public string Nome { get; set; }
}