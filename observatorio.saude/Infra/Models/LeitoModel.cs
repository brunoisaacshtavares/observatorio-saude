using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace observatorio.saude.Infra.Models;

/// <summary>
///     Representa a tabela fato com dados agregados sobre leitos hospitalares.
/// </summary>
[Table("fato_leito")]
[PrimaryKey(nameof(CodCnes), nameof(Anomes))]
[Index(nameof(Anomes), Name = "IX_fato_leito_anomes")]
public class LeitoModel
{
    [Column("cod_cnes")] public long CodCnes { get; set; }

    [Column("anomes")] public long Anomes { get; set; }

    [Column("nm_estabelecimento")]
    [MaxLength(255)]
    public string NmEstabelecimento { get; set; } = string.Empty;

    [Column("dscr_tipo_unidade")]
    [MaxLength(255)]
    public string DscrTipoUnidade { get; set; } = string.Empty;

    [Column("qtd_leitos_existentes")] public int QtdLeitosExistentes { get; set; }

    [Column("qtd_leitos_sus")] public int QtdLeitosSus { get; set; }

    [Column("qtd_uti_total_exist")] public int QtdUtiTotalExist { get; set; }

    [Column("qtd_uti_total_sus")] public int QtdUtiTotalSus { get; set; }

    [Column("qtd_uti_adulto_exist")] public int QtdUtiAdultoExist { get; set; }

    [Column("qtd_uti_adulto_sus")] public int QtdUtiAdultoSus { get; set; }

    [Column("qtd_uti_pediatrico_exist")] public int QtdUtiPediatricoExist { get; set; }

    [Column("qtd_uti_pediatrico_sus")] public int QtdUtiPediatricoSus { get; set; }

    [Column("qtd_uti_neonatal_exist")] public int QtdUtiNeonatalExist { get; set; }

    [Column("qtd_uti_neonatal_sus")] public int QtdUtiNeonatalSus { get; set; }

    [Column("qtd_uti_queimado_exist")] public int QtdUtiQueimadoExist { get; set; }

    [Column("qtd_uti_queimado_sus")] public int QtdUtiQueimadoSus { get; set; }

    [Column("qtd_uti_coronariana_exist")] public int QtdUtiCoronarianaExist { get; set; }

    [Column("qtd_uti_coronariana_sus")] public int QtdUtiCoronarianaSus { get; set; }
}