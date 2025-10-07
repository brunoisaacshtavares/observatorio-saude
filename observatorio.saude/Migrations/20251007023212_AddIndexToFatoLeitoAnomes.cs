using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace observatorio.saude.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToFatoLeitoAnomes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_fato_leito_anomes",
                table: "fato_leito",
                column: "anomes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_fato_leito_anomes",
                table: "fato_leito");
        }
    }
}
