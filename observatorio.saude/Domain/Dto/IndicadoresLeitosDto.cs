namespace observatorio.saude.Domain.Dto;

/// <summary>
///     Represents a summary of hospital bed indicators for a specific period.
/// </summary>
public class IndicadoresLeitosDto
{
    /// <summary>
    ///     The total number of existing hospital beds.
    /// </summary>
    public int TotalLeitos { get; set; }

    /// <summary>
    ///     The average occupancy rate of beds.
    /// </summary>
    public double LeitosSus { get; set; }

    /// <summary>
    ///     The number of critical care beds (e.g., in ICUs).
    /// </summary>
    public int Criticos { get; set; }
}