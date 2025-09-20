using System.ComponentModel.DataAnnotations;

namespace observatorio.saude.Domain.Dto;

/// <summary>
///     Representa o número total de estabelecimentos de saúde.
/// </summary>
public class NumeroEstabelecimentosDto
{
    /// <summary>
    ///     O número total de estabelecimentos de saúde.
    /// </summary>
    [Display(Name = "Total de Estabelecimentos",
        Description = "O número total de estabelecimentos de saúde cadastrados.")]
    public long TotalEstabelecimentos { get; set; }
}