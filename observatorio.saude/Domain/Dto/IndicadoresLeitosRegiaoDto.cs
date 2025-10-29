namespace observatorio.saude.Domain.Dto;

/// <summary>
///     Representa os indicadores de leitos consolidados por região.
/// </summary>
public class IndicadoresLeitosRegiaoDto
{
    public string NomeRegiao { get; set; } = string.Empty;
    public long Populacao { get; set; }
    public int TotalLeitos { get; set; }
    
    public int LeitosSus { get; set; }
    public int Criticos { get; set; }
    public double CoberturaLeitosPor1kHab { get; set; }
}