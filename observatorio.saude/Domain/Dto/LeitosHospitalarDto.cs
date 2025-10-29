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
    ///     O número de leitos disponíveis para o SUS.
    /// </summary>
    public int LeitosSus{ get; set; }

    /// <summary>
    ///     O número total de leitos existentes no estabelecimento.
    /// </summary>
    public int TotalLeitos { get; set; }
    

    /// <summary>
    ///     O código CNES do estabelecimento.
    /// </summary>
    public long CodCnes { get; set; }
}