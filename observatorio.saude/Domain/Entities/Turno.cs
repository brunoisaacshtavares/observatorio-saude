using System.ComponentModel.DataAnnotations;

namespace observatorio.saude.Domain.Entities;

/// <summary>
///     Representa o turno de atendimento de um estabelecimento de saúde.
/// </summary>
public class Turno
{
    /// <summary>
    ///     Código do turno de atendimento.
    /// </summary>
    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "O campo Código do Turno é obrigatório.")]
    [Display(Name = "Código do Turno", Description = "Código identificador do turno de atendimento.")]
    public long CodTurnoAtendimento { get; set; }

    /// <summary>
    ///     Descrição do turno de atendimento.
    /// </summary>
    [Display(Name = "Descrição do Turno", Description = "Nome ou descrição do turno (ex: Manhã, Tarde, Noite, 24h).")]
    public string? DscrTurnoAtendimento { get; set; }
}