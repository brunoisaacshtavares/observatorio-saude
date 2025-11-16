using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace observatorio.saude.Infra.Models;

[Table("dim_organizacao")]
public class OrganizacaoModel
{
    [Key] [Column("cod_cnes")] public long CodCnes { get; set; }

    [Column("tp_unidade")] public long? TpUnidade { get; set; }

    [Column("tp_gestao")] public char? TpGestao { get; set; }

    [Column("cod_esfera_administrativa")]
    [MaxLength(1)]
    public char? CodEsferaAdministrativa { get; set; }

    [Column("dscr_esfera_administrativa")]
    [MaxLength(50)]
    public required string DscrEsferaAdministrativa { get; set; }

    [Column("cod_natureza_jur")] public long? CodNaturezaJur { get; set; }

    [Column("cod_atividade")] public long? CodAtividade { get; set; }

    [Column("cod_nivel_hierarquia")] public long? CodNivelHierarquia { get; set; }

    [Column("dscr_nivel_hierarquia")]
    [MaxLength(255)]
    public string? DscrNivelHierarquia { get; set; }

    [Column("cod_natureza_organizacao")] public long? CodNaturezaOrganizacao { get; set; }

    [Column("dscr_natureza_organizacao")]
    [MaxLength(255)]
    public string? DscrNaturezaOrganizacao { get; set; }

    public virtual EstabelecimentoModel? Estabelecimento { get; set; }
}