using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace observatorio.saude.Migrations
{
    /// <inheritdoc />
    public partial class AddFatoLeitoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fato_leito",
                columns: table => new
                {
                    cod_cnes = table.Column<long>(type: "bigint", nullable: false),
                    anomes = table.Column<long>(type: "bigint", nullable: false),
                    nm_estabelecimento = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    dscr_tipo_unidade = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    qtd_leitos_existentes = table.Column<int>(type: "integer", nullable: false),
                    qtd_leitos_sus = table.Column<int>(type: "integer", nullable: false),
                    qtd_uti_total_exist = table.Column<int>(type: "integer", nullable: false),
                    qtd_uti_total_sus = table.Column<int>(type: "integer", nullable: false),
                    qtd_uti_adulto_exist = table.Column<int>(type: "integer", nullable: false),
                    qtd_uti_adulto_sus = table.Column<int>(type: "integer", nullable: false),
                    qtd_uti_pediatrico_exist = table.Column<int>(type: "integer", nullable: false),
                    qtd_uti_pediatrico_sus = table.Column<int>(type: "integer", nullable: false),
                    qtd_uti_neonatal_exist = table.Column<int>(type: "integer", nullable: false),
                    qtd_uti_neonatal_sus = table.Column<int>(type: "integer", nullable: false),
                    qtd_uti_queimado_exist = table.Column<int>(type: "integer", nullable: false),
                    qtd_uti_queimado_sus = table.Column<int>(type: "integer", nullable: false),
                    qtd_uti_coronariana_exist = table.Column<int>(type: "integer", nullable: false),
                    qtd_uti_coronariana_sus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fato_leito", x => new { x.cod_cnes, x.anomes });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fato_leito");
        }
    }
}
