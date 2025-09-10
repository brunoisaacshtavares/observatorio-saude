using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace observatorio.saude.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dim_estabelecimento",
                columns: table => new
                {
                    cod_unidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nm_razao_social = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    nm_fantasia = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    num_cnpj = table.Column<string>(type: "text", nullable: true),
                    num_cnpj_entidade = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    num_telefone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    cod_motivo_desab = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_estabelecimento", x => x.cod_unidade);
                });

            migrationBuilder.CreateTable(
                name: "dim_localizacao",
                columns: table => new
                {
                    cod_unidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cod_cep = table.Column<long>(type: "bigint", nullable: true),
                    endereco = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    numero = table.Column<long>(type: "bigint", nullable: true),
                    bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    latitude = table.Column<decimal>(type: "numeric(18,15)", nullable: true),
                    longitude = table.Column<decimal>(type: "numeric(18,15)", nullable: true),
                    cod_ibge = table.Column<int>(type: "integer", nullable: true),
                    cod_uf = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_localizacao", x => x.cod_unidade);
                });

            migrationBuilder.CreateTable(
                name: "dim_organizacao",
                columns: table => new
                {
                    cod_cnes = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tp_unidade = table.Column<long>(type: "bigint", nullable: true),
                    tp_gestao = table.Column<char>(type: "character(1)", nullable: true),
                    cod_esfera_administrativa = table.Column<char>(type: "character(1)", maxLength: 1, nullable: true),
                    dscr_esfera_administrativa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    cod_natureza_jur = table.Column<long>(type: "bigint", nullable: true),
                    cod_atividade = table.Column<long>(type: "bigint", nullable: true),
                    cod_nivel_hierarquia = table.Column<long>(type: "bigint", nullable: true),
                    dscr_nivel_hierarquia = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    cod_natureza_organizacao = table.Column<long>(type: "bigint", nullable: true),
                    dscr_natureza_organizacao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_organizacao", x => x.cod_cnes);
                });

            migrationBuilder.CreateTable(
                name: "dim_servicos",
                columns: table => new
                {
                    cod_cnes = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    st_faz_atendimento_ambulatorial_sus = table.Column<bool>(type: "boolean", nullable: true),
                    st_centro_cirurgico = table.Column<bool>(type: "boolean", nullable: true),
                    st_centro_obstetrico = table.Column<bool>(type: "boolean", nullable: true),
                    st_centro_neonatal = table.Column<bool>(type: "boolean", nullable: true),
                    st_atendimento_hospitalar = table.Column<bool>(type: "boolean", nullable: true),
                    st_servico_apoio = table.Column<bool>(type: "boolean", nullable: true),
                    st_atendimento_ambulatorial = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_servicos", x => x.cod_cnes);
                });

            migrationBuilder.CreateTable(
                name: "dim_turno",
                columns: table => new
                {
                    cod_turno_atendimento = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dscr_turno_atendimento = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_turno", x => x.cod_turno_atendimento);
                });

            migrationBuilder.CreateTable(
                name: "fato_estabelecimento",
                columns: table => new
                {
                    cod_cnes = table.Column<long>(type: "bigint", nullable: false),
                    cod_turno_atendimento = table.Column<long>(type: "bigint", nullable: false),
                    data_extracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    cod_unidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fato_estabelecimento", x => x.cod_cnes);
                    table.ForeignKey(
                        name: "FK_fato_estabelecimento_dim_estabelecimento_cod_unidade",
                        column: x => x.cod_unidade,
                        principalTable: "dim_estabelecimento",
                        principalColumn: "cod_unidade");
                    table.ForeignKey(
                        name: "FK_fato_estabelecimento_dim_localizacao_cod_unidade",
                        column: x => x.cod_unidade,
                        principalTable: "dim_localizacao",
                        principalColumn: "cod_unidade");
                    table.ForeignKey(
                        name: "FK_fato_estabelecimento_dim_organizacao_cod_cnes",
                        column: x => x.cod_cnes,
                        principalTable: "dim_organizacao",
                        principalColumn: "cod_cnes",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fato_estabelecimento_dim_servicos_cod_cnes",
                        column: x => x.cod_cnes,
                        principalTable: "dim_servicos",
                        principalColumn: "cod_cnes",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fato_estabelecimento_dim_turno_cod_turno_atendimento",
                        column: x => x.cod_turno_atendimento,
                        principalTable: "dim_turno",
                        principalColumn: "cod_turno_atendimento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fato_estabelecimento_cod_turno_atendimento",
                table: "fato_estabelecimento",
                column: "cod_turno_atendimento");

            migrationBuilder.CreateIndex(
                name: "IX_fato_estabelecimento_cod_unidade",
                table: "fato_estabelecimento",
                column: "cod_unidade",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fato_estabelecimento");

            migrationBuilder.DropTable(
                name: "dim_estabelecimento");

            migrationBuilder.DropTable(
                name: "dim_localizacao");

            migrationBuilder.DropTable(
                name: "dim_organizacao");

            migrationBuilder.DropTable(
                name: "dim_servicos");

            migrationBuilder.DropTable(
                name: "dim_turno");
        }
    }
}
