namespace observatorio.saude.Domain.Dto;

/// <summary>
/// Represents hospital bed indicators aggregated by state.
/// </summary>
public class IndicadoresLeitosEstadoDto
{
    /// <summary>
    /// The IBGE code of the state (UF).
    /// </summary>
    public long CodUf { get; set; }
    
    /// <summary>
    /// The full name of the state.
    /// </summary>
    public string? NomeUf { get; set; }
    
    /// <summary>
    /// The two-letter abbreviation of the state.
    /// </summary>
    public string? SiglaUf { get; set; }
    
    /// <summary>
    /// The region of the state (e.g., "Sul", "Nordeste").
    /// </summary>
    public string? Regiao { get; set; }
    
    /// <summary>
    /// The population of the state.
    /// </summary>
    public long Populacao { get; set; }
    
    /// <summary>
    /// The total number of beds in the state.
    /// </summary>
    public int TotalLeitos { get; set; }
    
    /// <summary>
    /// The total number of beds available for SUS in the state.
    /// </summary>
    public int LeitosDisponiveis { get; set; }
    
    /// <summary>
    /// The average occupancy rate of beds in the state.
    /// </summary>
    public double OcupacaoMedia { get; set; }
    
    /// <summary>
    /// The number of critical care beds in the state.
    /// </summary>
    public int Criticos { get; set; }
    
    /// <summary>
    /// The ratio of beds per 1,000 inhabitants.
    /// </summary>
    public double CoberturaLeitosPor1kHab { get; set; }
}