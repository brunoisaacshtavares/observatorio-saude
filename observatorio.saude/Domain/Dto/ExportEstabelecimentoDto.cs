using System.ComponentModel.DataAnnotations;

namespace observatorio.saude.Domain.Dto;

/// <summary>
///     Data Transfer Object for exporting establishment information.
/// </summary>
public class ExportEstabelecimentoDto
{
    [Display(Name = "CNES")] public long CodCnes { get; set; }

    [Display(Name = "Razão Social")] public string RazaoSocial { get; set; } = string.Empty;

    [Display(Name = "Nome Fantasia")] public string NomeFantasia { get; set; } = string.Empty;

    [Display(Name = "Endereço")] public string Endereco { get; set; } = string.Empty;

    [Display(Name = "Bairro")] public string Bairro { get; set; } = string.Empty;

    [Display(Name = "CEP")] public string Cep { get; set; } = string.Empty;

    [Display(Name = "UF")] public string Uf { get; set; } = string.Empty;

    [Display(Name = "Esfera Administrativa")]
    public string EsferaAdministrativa { get; set; } = string.Empty;

    public long? CodUfParaMapeamento { get; set; }
}