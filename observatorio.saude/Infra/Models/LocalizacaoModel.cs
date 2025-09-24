using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace observatorio.saude.Infra.Models;

[Table("dim_localizacao")]
[Index(nameof(CodUf))]
[Index(nameof(Latitude), nameof(Longitude))]
public class LocalizacaoModel
{
    [Key]
    [Column("cod_unidade")]
    [MaxLength(100)]
    public string CodUnidade { get; set; }

    [Column("cod_cep")] public long? CodCep { get; set; }

    [Column("endereco")] [MaxLength(200)] public string? Endereco { get; set; }

    [Column("numero")] public long? Numero { get; set; }

    [Column("bairro")] [MaxLength(100)] public string? Bairro { get; set; }

    [Column("latitude", TypeName = "numeric(18, 15)")]
    public decimal? Latitude { get; set; }

    [Column("longitude", TypeName = "numeric(18, 15)")]
    public decimal? Longitude { get; set; }

    [Column("cod_ibge")] public int? CodIbge { get; set; }

    [Column("cod_uf")] public long? CodUf { get; set; }

    public virtual EstabelecimentoModel? Estabelecimento { get; set; }
}