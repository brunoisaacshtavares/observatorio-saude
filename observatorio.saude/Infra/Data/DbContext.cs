using Microsoft.EntityFrameworkCore;
using observatorio.saude.Infra.Models;

namespace observatorio.saude.Infra.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<CaracteristicaEstabelecimentoModel> CaracteristicaEstabelecimentoModel { get; set; }
    public DbSet<LocalizacaoModel> LocalizacaoModel { get; set; }
    public DbSet<OrganizacaoModel> OrganizacaoModel { get; set; }
    public DbSet<TurnoModel> TurnoModel { get; set; }
    public DbSet<ServicoModel> ServicoModel { get; set; }
    public DbSet<EstabelecimentoModel> EstabelecimentoModel { get; set; }
    public DbSet<LeitoModel> LeitosModel { get; set; }
}