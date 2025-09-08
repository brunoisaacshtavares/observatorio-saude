using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace observatorio.saude.Infra.Models;

[Table("dim_turno")]
public class TurnoModel
{
    [Key]
    [Column("cod_turno_atendimento")]
    public long CodTurnoAtendimento { get; set; }

    [Column("dscr_turno_atendimento")] public string? DscrTurnoAtendimento { get; set; }

    public virtual ICollection<EstabelecimentoModel> Estabelecimento { get; set; } = new List<EstabelecimentoModel>();
}