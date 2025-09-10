using System.ComponentModel.DataAnnotations;

namespace observatorio.saude.Domain.Entities;

/// <summary>
///     Representa os serviços oferecidos por um estabelecimento de saúde.
/// </summary>
public class Servico
{
    /// <summary>
    ///     Código CNES do estabelecimento.
    /// </summary>
    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "O campo Código CNES é obrigatório.")]
    [Display(Name = "Código CNES", Description = "Identificador único do estabelecimento no CNES.")]
    public long CodCnes { get; set; }

    /// <summary>
    ///     Indica se o estabelecimento realiza atendimento ambulatorial pelo SUS.
    /// </summary>
    [Display(Name = "Atendimento Ambulatorial SUS", Description = "Informa se há atendimento ambulatorial via SUS.")]
    public bool? FazAtendimentoAmbulatorialSus { get; set; }

    /// <summary>
    ///     Indica se há centro cirúrgico no estabelecimento.
    /// </summary>
    [Display(Name = "Centro Cirúrgico", Description = "Informa se há centro cirúrgico disponível.")]
    public bool? TemCentroCirurgico { get; set; }

    /// <summary>
    ///     Indica se há centro obstétrico no estabelecimento.
    /// </summary>
    [Display(Name = "Centro Obstétrico", Description = "Informa se há centro obstétrico disponível.")]
    public bool? TemCentroObstetrico { get; set; }

    /// <summary>
    ///     Indica se há centro neonatal no estabelecimento.
    /// </summary>
    [Display(Name = "Centro Neonatal", Description = "Informa se há centro neonatal disponível.")]
    public bool? TemCentroNeonatal { get; set; }

    /// <summary>
    ///     Indica se o estabelecimento realiza atendimento hospitalar.
    /// </summary>
    [Display(Name = "Atendimento Hospitalar", Description = "Informa se o local realiza atendimento hospitalar.")]
    public bool? FazAtendimentoHospitalar { get; set; }

    /// <summary>
    ///     Indica se há serviços de apoio no estabelecimento.
    /// </summary>
    [Display(Name = "Serviço de Apoio", Description = "Informa se há serviços de apoio diagnóstico ou terapêutico.")]
    public bool? TemServicoApoio { get; set; }

    /// <summary>
    ///     Indica se o estabelecimento realiza qualquer atendimento ambulatorial (não necessariamente SUS).
    /// </summary>
    [Display(Name = "Atendimento Ambulatorial", Description = "Informa se há atendimento ambulatorial em geral.")]
    public bool? FazAtendimentoAmbulatorial { get; set; }
}