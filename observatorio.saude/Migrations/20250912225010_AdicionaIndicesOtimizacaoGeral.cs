using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace observatorio.saude.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaIndicesOtimizacaoGeral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_fato_estabelecimento_data_extracao",
                table: "fato_estabelecimento",
                column: "data_extracao");

            migrationBuilder.CreateIndex(
                name: "IX_dim_localizacao_cod_uf",
                table: "dim_localizacao",
                column: "cod_uf");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_fato_estabelecimento_data_extracao",
                table: "fato_estabelecimento");

            migrationBuilder.DropIndex(
                name: "IX_dim_localizacao_cod_uf",
                table: "dim_localizacao");
        }
    }
}
