using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoDeDados.Migrations
{
    /// <inheritdoc />
    public partial class PrimeiraMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Convenios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Convenios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sobrenome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataDeNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RG = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RGUF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Celular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelefoneFixo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConvenioId = table.Column<int>(type: "int", nullable: false),
                    NumeroCarteirinha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidadeCarteirinha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDeExclusao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pacientes_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Convenios_Nome",
                table: "Convenios",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_ConvenioId",
                table: "Pacientes",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_CPF",
                table: "Pacientes",
                column: "CPF",
                unique: true,
                filter: "[CPF] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_RG",
                table: "Pacientes",
                column: "RG",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Convenios");
        }
    }
}
