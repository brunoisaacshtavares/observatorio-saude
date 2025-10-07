namespace observatorio.saude.Domain.Dto;

public class IndicadoresLeitosDto
{
    public int TotalLeitos { get; set; }
    public int LeitosDisponiveis { get; set; }
    public double OcupacaoMedia { get; set; }
    public int Criticos { get; set; }
}