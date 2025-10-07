using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace observatorio.saude.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToFatoLeitoAnomesCodCnes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_fato_leito_anomes_cod_cnes",
                table: "fato_leito",
                columns: new[] { "anomes", "cod_cnes" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_fato_leito_anomes_cod_cnes",
                table: "fato_leito");
        }
    }
}
