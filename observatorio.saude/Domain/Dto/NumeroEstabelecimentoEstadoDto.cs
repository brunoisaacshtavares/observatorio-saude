namespace observatorio.saude.Domain.Dto;

public class NumeroEstabelecimentoEstadoDto
{
    public long CodUf { get; set; }


    public string NomeUf { get; set; }

    public string SiglaUf { get; set; }

    public string Regiao { get; set; }

    public int TotalEstabelecimentos { get; set; }

    public long Populacao { get; set; }

    public double CoberturaEstabelecimentos { get; set; }
}