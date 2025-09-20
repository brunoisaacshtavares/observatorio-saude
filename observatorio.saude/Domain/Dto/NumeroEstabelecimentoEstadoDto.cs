using System.ComponentModel.DataAnnotations;

namespace observatorio.saude.Domain.Dto;

/// <summary>
///     Representa o total de estabelecimentos de saúde, população e a cobertura em um determinado estado.
/// </summary>
public class NumeroEstabelecimentoEstadoDto
{
    /// <summary>
    ///     Código do estado (UF).
    /// </summary>
    [Display(Name = "Código do Estado", Description = "Código numérico do estado, conforme tabela do IBGE.")]
    public long CodUf { get; set; }

    /// <summary>
    ///     Nome do estado.
    /// </summary>
    [Display(Name = "Nome do Estado", Description = "Nome completo do estado.")]
    public string NomeUf { get; set; }

    /// <summary>
    ///     Sigla do estado.
    /// </summary>
    [Display(Name = "Sigla do Estado", Description = "Sigla de duas letras do estado.")]
    public string SiglaUf { get; set; }

    /// <summary>
    ///     Região geográfica do estado.
    /// </summary>
    [Display(Name = "Região", Description = "Região geográfica do estado (e.g., Sudeste, Sul, etc.).")]
    public string Regiao { get; set; }

    /// <summary>
    ///     Total de estabelecimentos de saúde no estado.
    /// </summary>
    [Display(Name = "Total de Estabelecimentos", Description = "Número total de estabelecimentos de saúde no estado.")]
    public int TotalEstabelecimentos { get; set; }

    /// <summary>
    ///     População total do estado.
    /// </summary>
    [Display(Name = "População", Description = "Número total de habitantes no estado.")]
    public long Populacao { get; set; }

    /// <summary>
    ///     Cobertura de estabelecimentos por habitante no estado.
    /// </summary>
    [Display(Name = "Cobertura de Estabelecimentos",
        Description = "Relação entre o número de estabelecimentos e a população do estado.")]
    public double CoberturaEstabelecimentos { get; set; }
}