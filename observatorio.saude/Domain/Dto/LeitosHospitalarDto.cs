namespace observatorio.saude.Domain.Dto;

/// <summary>
///     Representa as informações de leitos hospitalares para exibição.
/// </summary>
public class LeitosHospitalarDto
{
    /// <summary>
    ///     O nome do estabelecimento.
    /// </summary>
    public string? NomeEstabelecimento { get; set; }

    /// <summary>
    ///     A sigla da Unidade Federativa (UF).
    /// </summary>
    public string? LocalizacaoUf { get; set; }

    /// <summary>
    ///     O endereço completo do estabelecimento.
    /// </summary>
    public string? EnderecoCompleto { get; set; }

    /// <summary>
    ///     O número de leitos ocupados no estabelecimento.
    /// </summary>
    public int LeitosOcupados { get; set; }

    /// <summary>
    ///     O número de leitos disponíveis para o SUS.
    /// </summary>
    public int LeitosDisponiveis { get; set; }

    /// <summary>
    ///     O número total de leitos existentes no estabelecimento.
    /// </summary>
    public int TotalLeitos { get; set; }

    /// <summary>
    ///     A porcentagem de ocupação de leitos.
    /// </summary>
    public decimal PorcentagemOcupacao { get; set; }

    /// <summary>
    ///     O código CNES do estabelecimento.
    /// </summary>
    public long CodCnes { get; set; }
}