namespace observatorio.saude.Domain.Dto;

/// <summary>
///     Representa as informações de leitos hospitalares para exibição.
/// </summary>
public class LeitosHospitalarDto
{
    /// <summary>
    ///     O código CNES do estabelecimento.
    /// </summary>
    public long CodCnes { get; set; }

    /// <summary>
    ///     O nome do estabelecimento.
    /// </summary>
    public string? NomeEstabelecimento { get; set; }
    
    /// <summary>
    ///     Descrição do tipo de unidade. Ex: "HOSPITAL GERAL".
    /// </summary>
    public string? DscrTipoUnidade { get; set; }

    /// <summary>
    ///     A sigla da Unidade Federativa (UF).
    /// </summary>
    public string? LocalizacaoUf { get; set; }

    /// <summary>
    ///     O endereço completo do estabelecimento.
    /// </summary>
    public string? EnderecoCompleto { get; set; }

    /// <summary>
    ///     O número total de leitos existentes no estabelecimento.
    /// </summary>
    public int TotalLeitos { get; set; }

    /// <summary>
    ///     O número de leitos disponíveis para o SUS.
    /// </summary>
    public int LeitosSus{ get; set; }

    /// <summary>
    ///     Quantidade de leitos de UTI Total existentes.
    /// </summary>
    public int QtdUtiTotalExist { get; set; }

    /// <summary>
    ///     Quantidade de leitos de UTI Total para o SUS.
    /// </summary>
    public int QtdUtiTotalSus { get; set; }

    /// <summary>
    ///     Quantidade de leitos de UTI Adulto existentes.
    /// </summary>
    public int QtdUtiAdultoExist { get; set; }

    /// <summary>
    ///     Quantidade de leitos de UTI Adulto para o SUS.
    /// </summary>
    public int QtdUtiAdultoSus { get; set; }

    /// <summary>
    ///     Quantidade de leitos de UTI Pediátrico existentes.
    /// </summary>
    public int QtdUtiPediatricoExist { get; set; }

    /// <summary>
    ///     Quantidade de leitos de UTI Pediátrico para o SUS.
    /// </summary>
    public int QtdUtiPediatricoSus { get; set; }

    /// <summary>
    ///     Quantidade de leitos de UTI Neonatal existentes.
    /// </summary>
    public int QtdUtiNeonatalExist { get; set; }

    /// <summary>
    ///     Quantidade de leitos de UTI Neonatal para o SUS.
    /// </summary>
    public int QtdUtiNeonatalSus { get; set; }

    /// <summary>
    ///     Quantidade de leitos de UTI Queimado existentes.
    /// </summary>
    public int QtdUtiQueimadoExist { get; set; }

    /// <summary>
    ///     Quantidade de leitos de UTI Queimado para o SUS.
    /// </summary>
    public int QtdUtiQueimadoSus { get; set; }

    /// <summary>
    ///     Quantidade de leitos de UTI Coronariana existentes.
    /// </summary>
    public int QtdUtiCoronarianaExist { get; set; }

    /// <summary>
    ///     Quantidade de leitos de UTI Coronariana para o SUS.
    /// </summary>
    public int QtdUtiCoronarianaSus { get; set; }
}