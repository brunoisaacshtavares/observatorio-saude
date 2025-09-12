using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace observatorio.saude.Infra.Models;

[Table("fato_estabelecimento")]
[Index(nameof(CodUnidade))]
[Index(nameof(CodTurnoAtendimento))]
[Index(nameof(DataExtracao))]
public class EstabelecimentoModel
{
    [Key] [Column("cod_cnes")] public long CodCnes { get; set; }

    [Column("cod_turno_atendimento")] public long CodTurnoAtendimento { get; set; }

    [Column("data_extracao")] public DateTime DataExtracao { get; set; }

    [Column("cod_unidade")]
    [MaxLength(100)]
    public string? CodUnidade { get; set; }

    [ForeignKey("CodUnidade")]
    public virtual CaracteristicaEstabelecimentoModel? CaracteristicaEstabelecimento { get; set; }

    [ForeignKey("CodUnidade")] public virtual LocalizacaoModel? Localizacao { get; set; }

    [ForeignKey("CodCnes")] public virtual OrganizacaoModel? Organizacao { get; set; }

    [ForeignKey("CodTurnoAtendimento")] public virtual TurnoModel? Turno { get; set; }

    [ForeignKey("CodCnes")] public virtual ServicoModel? Servico { get; set; }
}