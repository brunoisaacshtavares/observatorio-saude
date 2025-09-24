using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace observatorio.saude.Migrations
{
    /// <inheritdoc />
    public partial class indiceLocalizacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_dim_localizacao_latitude_longitude",
                table: "dim_localizacao",
                columns: new[] { "latitude", "longitude" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_dim_localizacao_latitude_longitude",
                table: "dim_localizacao");
        }
    }
}
