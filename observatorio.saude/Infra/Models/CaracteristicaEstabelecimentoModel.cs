using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace observatorio.saude.Infra.Models;

[Table("dim_estabelecimento")]
public class CaracteristicaEstabelecimentoModel
{
    [Key]
    [Column("cod_unidade")]
    [MaxLength(100)]
    public string CodUnidade { get; set; }

    [Column("nm_razao_social")]
    [MaxLength(255)]
    public string? NmRazaoSocial { get; set; }

    [Column("nm_fantasia")]
    [MaxLength(255)]
    public string? NmFantasia { get; set; }

    [Column("num_cnpj")] public string? NumCnpj { get; set; }

    [Column("num_cnpj_entidade")] public string? NumCnpjEntidade { get; set; }

    [Column("email")] [MaxLength(255)] public string? Email { get; set; }

    [Column("num_telefone")]
    [MaxLength(50)]
    public string? NumTelefone { get; set; }

    [Column("cod_motivo_desab")] public int? CodMotivoDesab { get; set; }

    public virtual EstabelecimentoModel? Estabelecimento { get; set; }
}