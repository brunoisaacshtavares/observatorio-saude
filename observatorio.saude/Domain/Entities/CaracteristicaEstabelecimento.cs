using System.ComponentModel.DataAnnotations;

namespace observatorio.saude.Domain.Entities;

/// <summary>
///     Representa as características principais de um estabelecimento de saúde.
/// </summary>
public class CaracteristicaEstabelecimento
{
    /// <summary>
    ///     Identificador único da unidade de saúde.
    /// </summary>
    [Required]
    [Display(Name = "Código da Unidade", Description = "Identificador único da unidade de saúde.")]
    public required string CodUnidade { get; set; }

    /// <summary>
    ///     Nome jurídico da unidade.
    /// </summary>
    [Display(Name = "Razão Social", Description = "Nome jurídico da unidade.")]
    public string? NmRazaoSocial { get; set; }

    /// <summary>
    ///     Nome comercial da unidade.
    /// </summary>
    [Display(Name = "Nome Fantasia", Description = "Nome comercial da unidade.")]
    public string? NmFantasia { get; set; }

    /// <summary>
    ///     CNPJ da unidade de saúde.
    /// </summary>
    [Display(Name = "CNPJ", Description = "CNPJ da unidade de saúde.")]
    public string? NumCnpj { get; set; }

    /// <summary>
    ///     CNPJ da entidade responsável pela unidade.
    /// </summary>
    [Display(Name = "CNPJ da Entidade Mantenedora", Description = "CNPJ da entidade responsável.")]
    public string? NumCnpjEntidade { get; set; }

    /// <summary>
    ///     Endereço de e-mail para contato da unidade.
    /// </summary>
    [EmailAddress]
    [Display(Name = "E-mail de Contato", Description = "Endereço de e-mail da unidade.")]
    public string? Email { get; set; }

    /// <summary>
    ///     Número de telefone para contato da unidade.
    /// </summary>
    [Phone]
    [Display(Name = "Telefone de Contato", Description = "Número de telefone da unidade.")]
    public string? NumTelefone { get; set; }
}