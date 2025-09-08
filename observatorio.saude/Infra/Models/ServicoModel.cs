using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace observatorio.saude.Infra.Models;

[Table("dim_servicos")]
public class ServicoModel
{
    [Key] [Column("cod_cnes")] public long CodCnes { get; set; }

    [Column("st_faz_atendimento_ambulatorial_sus")]
    public bool? StFazAtendimentoAmbulatorialSus { get; set; }

    [Column("st_centro_cirurgico")] public bool? StCentroCirurgico { get; set; }

    [Column("st_centro_obstetrico")] public bool? StCentroObstetrico { get; set; }

    [Column("st_centro_neonatal")] public bool? StCentroNeonatal { get; set; }

    [Column("st_atendimento_hospitalar")] public bool? StAtendimentoHospitalar { get; set; }

    [Column("st_servico_apoio")] public bool? StServicoApoio { get; set; }

    [Column("st_atendimento_ambulatorial")]
    public bool? StAtendimentoAmbulatorial { get; set; }

    public virtual EstabelecimentoModel? Estabelecimento { get; set; }
}