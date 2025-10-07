namespace observatorio.saude.Domain.Dto;

/// <summary>
/// Represents a summary of hospital bed indicators for a specific period.
/// </summary>
public class IndicadoresLeitosDto
{
    /// <summary>
    /// The total number of existing hospital beds.
    /// </summary>
    public int TotalLeitos { get; set; }
    
    /// <summary>
    /// The total number of beds available for SUS (Brazil's public health system).
    /// </summary>
    public int LeitosDisponiveis { get; set; }
    
    /// <summary>
    /// The average occupancy rate of beds.
    /// </summary>
    public double OcupacaoMedia { get; set; }
    
    /// <summary>
    /// The number of critical care beds (e.g., in ICUs).
    /// </summary>
    public int Criticos { get; set; }
}