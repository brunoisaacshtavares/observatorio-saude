using System.ComponentModel.DataAnnotations;

namespace observatorio.saude.Domain.Entities;

/// <summary>
///     Representa um estabelecimento de saúde com suas principais características, localização e serviços.
/// </summary>
public class Estabelecimento
{
    /// <summary>
    ///     Código CNES (Cadastro Nacional de Estabelecimentos de Saúde).
    /// </summary>
    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "O campo Código CNES é obrigatório e deve ser um valor positivo.")]
    [Display(Name = "Código CNES", Description = "Código único do estabelecimento no CNES.")]
    public long CodCnes { get; set; }

    /// <summary>
    ///     Data da extração dos dados.
    /// </summary>
    [Required]
    [Range(typeof(DateTime), "1/1/2000", "1/1/2100", ErrorMessage = "O campo Data de Extração deve conter uma data válida.")]
    [Display(Name = "Data de Extração", Description = "Data em que os dados foram extraídos.")]
    public DateTime DataExtracao { get; set; }

    /// <summary>
    ///     Características gerais do estabelecimento.
    /// </summary>
    [Display(Name = "Características do Estabelecimento",
        Description = "Informações como nome, CNPJ, telefone e email.")]
    public CaracteristicaEstabelecimento? Caracteristicas { get; set; }

    /// <summary>
    ///     Informações de localização do estabelecimento.
    /// </summary>
    [Display(Name = "Localização", Description = "Endereço, bairro, cidade e outras informações geográficas.")]
    public Localizacao? Localizacao { get; set; }

    /// <summary>
    ///     Dados sobre a organização do estabelecimento.
    /// </summary>
    [Display(Name = "Organização", Description = "Tipo de gestão, esferas administrativas, entre outros.")]
    public Organizacao? Organizacao { get; set; }

    /// <summary>
    ///     Informações sobre os turnos de funcionamento.
    /// </summary>
    [Display(Name = "Turno", Description = "Turnos em que o estabelecimento realiza atendimento.")]
    public Turno? Turno { get; set; }

    /// <summary>
    ///     Serviços prestados pelo estabelecimento.
    /// </summary>
    [Display(Name = "Serviço", Description = "Lista de serviços e procedimentos oferecidos.")]
    public Servico? Servico { get; set; }
}