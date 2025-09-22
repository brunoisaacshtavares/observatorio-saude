using System.ComponentModel.DataAnnotations;

namespace observatorio.saude.Domain.Dto;

public class ExportEstabelecimentoDto
{
    [Display(Name = "CNES")] public long CodCnes { get; set; }

    [Display(Name = "Razão Social")] public string RazaoSocial { get; set; }

    [Display(Name = "Nome Fantasia")] public string NomeFantasia { get; set; }

    [Display(Name = "Endereço")] public string Endereco { get; set; }

    [Display(Name = "Bairro")] public string Bairro { get; set; }

    [Display(Name = "CEP")] public string Cep { get; set; }

    [Display(Name = "UF")] public string Uf { get; set; }

    [Display(Name = "Esfera Administrativa")]
    public string EsferaAdministrativa { get; set; }

    public long? CodUfParaMapeamento { get; set; }
}