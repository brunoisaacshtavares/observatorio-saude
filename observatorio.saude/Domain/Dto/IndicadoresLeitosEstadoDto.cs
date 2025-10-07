// observatorio.saude.Domain/Dto/IndicadoresLeitosEstadoDto.cs

namespace observatorio.saude.Domain.Dto;

public class IndicadoresLeitosEstadoDto
{
    public long CodUf { get; set; }
    public string? NomeUf { get; set; }
    public string? SiglaUf { get; set; }
    public string? Regiao { get; set; }
    public long Populacao { get; set; }
    public int TotalLeitos { get; set; }
    public int LeitosDisponiveis { get; set; }
    public double OcupacaoMedia { get; set; }
    public int Criticos { get; set; }
    public double CoberturaLeitosPor1kHab { get; set; }
}