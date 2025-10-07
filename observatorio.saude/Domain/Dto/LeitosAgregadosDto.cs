namespace observatorio.saude.Domain.Dto;

/// <summary>
/// DTO for transporting raw aggregated data from the repository to the handler.
/// </summary>
public class LeitosAgregadosDto
{
    /// <summary>
    /// The total number of existing beds.
    /// </summary>
    public int TotalLeitos { get; set; }
    
    /// <summary>
    /// The total number of beds available for SUS.
    /// </summary>
    public int TotalLeitosSus { get; set; }
    
    /// <summary>
    /// The total number of ICU beds.
    /// </summary>
    public int TotalUti { get; set; }
}