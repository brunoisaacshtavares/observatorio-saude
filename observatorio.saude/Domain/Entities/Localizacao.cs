using System.ComponentModel.DataAnnotations;

namespace observatorio.saude.Domain.Entities;

/// <summary>
///     Representa as informações de localização geográfica e endereço de um estabelecimento de saúde.
/// </summary>
public class Localizacao
{
    /// <summary>
    ///     Código da unidade de saúde associado.
    /// </summary>
    [Required]
    [Display(Name = "Código da Unidade", Description = "Identificador da unidade de saúde.")]
    public string CodUnidade { get; set; }

    /// <summary>
    ///     Código do CEP do endereço.
    /// </summary>
    [Display(Name = "CEP", Description = "Código de Endereçamento Postal do local.")]
    public long? CodCep { get; set; }

    /// <summary>
    ///     Nome da rua ou logradouro.
    /// </summary>
    [Display(Name = "Endereço", Description = "Nome da rua, avenida ou logradouro.")]
    public string? Endereco { get; set; }

    /// <summary>
    ///     Número do endereço.
    /// </summary>
    [Display(Name = "Número", Description = "Número da edificação no endereço.")]
    public long? Numero { get; set; }

    /// <summary>
    ///     Nome do bairro.
    /// </summary>
    [Display(Name = "Bairro", Description = "Nome do bairro onde a unidade está localizada.")]
    public string? Bairro { get; set; }

    /// <summary>
    ///     Latitude geográfica do estabelecimento.
    /// </summary>
    [Display(Name = "Latitude", Description = "Coordenada geográfica de latitude.")]
    public decimal? Latitude { get; set; }

    /// <summary>
    ///     Longitude geográfica do estabelecimento.
    /// </summary>
    [Display(Name = "Longitude", Description = "Coordenada geográfica de longitude.")]
    public decimal? Longitude { get; set; }

    /// <summary>
    ///     Código IBGE do município.
    /// </summary>
    [Display(Name = "Código IBGE", Description = "Código do município segundo o IBGE.")]
    public int? CodIbge { get; set; }

    /// <summary>
    ///     Código da Unidade Federativa (UF).
    /// </summary>
    [Display(Name = "Código da UF", Description = "Código do estado da federação.")]
    public long? CodUf { get; set; }
}