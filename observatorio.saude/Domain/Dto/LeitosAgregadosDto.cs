// observatorio.saude.Domain/Dto/LeitosAgregadosDto.cs

namespace observatorio.saude.Domain.Dto;

/// <summary>
///     DTO para transportar dados brutos agregados do repositório para o handler.
/// </summary>
public class LeitosAgregadosDto
{
    public int TotalLeitos { get; set; }
    public int TotalLeitosSus { get; set; }
    public int TotalUti { get; set; }
}