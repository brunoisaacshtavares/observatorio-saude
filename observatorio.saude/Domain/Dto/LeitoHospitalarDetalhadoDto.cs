namespace observatorio.saude.Domain.Dto;

/// <summary>
///     Detalhes sobre a capacidade de leitos (gerais e de UTI).
/// </summary>
public class CapacidadeLeitosDto
{
    public int TotalLeitos { get; set; }
    public int LeitosSus { get; set; }
    public int QtdUtiTotalExist { get; set; }
    public int QtdUtiTotalSus { get; set; }
    public int QtdUtiAdultoExist { get; set; }
    public int QtdUtiAdultoSus { get; set; }
    public int QtdUtiPediatricoExist { get; set; }
    public int QtdUtiPediatricoSus { get; set; }
    public int QtdUtiNeonatalExist { get; set; }
    public int QtdUtiNeonatalSus { get; set; }
    public int QtdUtiQueimadoExist { get; set; }
    public int QtdUtiQueimadoSus { get; set; }
    public int QtdUtiCoronarianaExist { get; set; }
    public int QtdUtiCoronarianaSus { get; set; }
}

/// <summary>
///     Detalhes de localização do estabelecimento.
/// </summary>
public class LocalizacaoDetalhesDto
{
    public string? Uf { get; set; }
    public string? EnderecoCompleto { get; set; }
}

/// <summary>
///     Detalhes sobre os serviços disponíveis no estabelecimento.
/// </summary>
public class ServicosDisponiveisDto
{
    public bool? FazAtendimentoAmbulatorialSus { get; set; }
    public bool? TemCentroCirurgico { get; set; }
    public bool? TemCentroObstetrico { get; set; }
    public bool? TemCentroNeonatal { get; set; }
    public bool? FazAtendimentoHospitalar { get; set; }
    public bool? TemServicoApoio { get; set; }
    public bool? FazAtendimentoAmbulatorial { get; set; }
}

/// <summary>
///     Detalhes sobre a organização e gestão do estabelecimento.
/// </summary>
public class OrganizacaoDetalhesDto
{
    public long? TipoUnidade { get; set; }
    public char? TipoGestao { get; set; }
    public string DescricaoEsferaAdministrativa { get; set; }
    public long? CodAtividade { get; set; }
}

/// <summary>
///     Detalhes sobre o turno de atendimento.
/// </summary>
public class TurnoAtendimentoDto
{
    public long CodTurnoAtendimento { get; set; }
    public string? DscrTurnoAtendimento { get; set; }
}

/// <summary>
///     Representa as informações de leitos hospitalares para exibição.
/// </summary>
public class LeitosHospitalarDetalhadoDto
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
    ///     Detalhes sobre a capacidade de leitos.
    /// </summary>
    public CapacidadeLeitosDto? Capacidade { get; set; }

    /// <summary>
    ///     Detalhes de localização do estabelecimento.
    /// </summary>
    public LocalizacaoDetalhesDto? Localizacao { get; set; }

    /// <summary>
    ///     Detalhes sobre os serviços disponíveis.
    /// </summary>
    public ServicosDisponiveisDto? Servicos { get; set; }

    /// <summary>
    ///     Detalhes sobre a organização e gestão.
    /// </summary>
    public OrganizacaoDetalhesDto? Organizacao { get; set; }

    /// <summary>
    ///     Detalhes sobre o turno de atendimento.
    /// </summary>
    public TurnoAtendimentoDto? Turno { get; set; }
}