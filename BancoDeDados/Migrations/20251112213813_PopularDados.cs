using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoDeDados.Migrations
{
    /// <inheritdoc />
    public partial class PopularDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserir Convênios
            migrationBuilder.InsertData(
                table: "Convenios",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Unimed" },
                    { 2, "Amil" },
                    { 3, "Bradesco Saúde" },
                    { 4, "SulAmérica" },
                    { 5, "Particular" }
                });

            // Inserir Pacientes
            migrationBuilder.InsertData(
                table: "Pacientes",
                columns: new[] { "Id", "Key", "Nome", "Sobrenome", "DataDeNascimento", "Genero", "CPF", "RG", "RGUF", "Email", "Celular", "TelefoneFixo", "ConvenioId", "NumeroCarteirinha", "ValidadeCarteirinha", "DataDeExclusao" },
                values: new object[,]
                {
                    { 1, Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"), "João", "Silva", new DateTime(1985, 3, 15), "Masculino", "12345678901", "123456789", "SP", "joao.silva@email.com", "(11) 98765-4321", "(11) 3456-7890", 1, "UNIMED123456", new DateTime(2025, 12, 31), null },
                    { 2, Guid.Parse("b2c3d4e5-f6a7-4b5c-8d9e-0f1a2b3c4d5e"), "Maria", "Santos", new DateTime(1990, 7, 22), "Feminino", "23456789012", "234567890", "RJ", "maria.santos@email.com", "(21) 99876-5432", null, 2, "AMIL234567", new DateTime(2026, 6, 30), null },
                    { 3, Guid.Parse("c3d4e5f6-a7b8-4c5d-8e9f-0a1b2c3d4e5f"), "Pedro", "Oliveira", new DateTime(1978, 11, 5), "Masculino", "34567890123", "345678901", "MG", "pedro.oliveira@email.com", "(31) 98765-1234", "(31) 3234-5678", 3, "BRAD345678", new DateTime(2025, 9, 15), null },
                    { 4, Guid.Parse("d4e5f6a7-b8c9-4d5e-8f9a-0b1c2d3e4f5a"), "Ana", "Costa", new DateTime(1995, 2, 18), "Feminino", "45678901234", "456789012", "SP", "ana.costa@email.com", "(11) 97654-3210", null, 1, "UNIMED456789", new DateTime(2026, 3, 31), null },
                    { 5, Guid.Parse("e5f6a7b8-c9d0-4e5f-8a9b-0c1d2e3f4a5b"), "Carlos", "Ferreira", new DateTime(1982, 9, 30), "Masculino", "56789012345", "567890123", "RS", "carlos.ferreira@email.com", "(51) 99123-4567", "(51) 3123-4567", 4, "SULA567890", new DateTime(2025, 11, 20), null },
                    { 6, Guid.Parse("f6a7b8c9-d0e1-4f5a-8b9c-0d1e2f3a4b5c"), "Juliana", "Rodrigues", new DateTime(1988, 5, 12), "Feminino", "67890123456", "678901234", "PR", "juliana.rodrigues@email.com", "(41) 98234-5678", null, 2, "AMIL678901", new DateTime(2026, 2, 28), null },
                    { 7, Guid.Parse("a7b8c9d0-e1f2-4a5b-8c9d-0e1f2a3b4c5d"), "Roberto", "Almeida", new DateTime(1975, 12, 8), "Masculino", "78901234567", "789012345", "BA", "roberto.almeida@email.com", "(71) 99345-6789", "(71) 3345-6789", 3, "BRAD789012", new DateTime(2025, 8, 10), null },
                    { 8, Guid.Parse("b8c9d0e1-f2a3-4b5c-8d9e-0f1a2b3c4d5e"), "Fernanda", "Lima", new DateTime(1992, 4, 25), "Feminino", "89012345678", "890123456", "SC", "fernanda.lima@email.com", "(48) 98456-7890", null, 5, "PART890123", new DateTime(2026, 12, 31), null },
                    { 9, Guid.Parse("c9d0e1f2-a3b4-4c5d-8e9f-0a1b2c3d4e5f"), "Lucas", "Martins", new DateTime(1987, 8, 14), "Masculino", "90123456789", "901234567", "CE", "lucas.martins@email.com", "(85) 97567-8901", "(85) 3456-7890", 1, "UNIMED901234", new DateTime(2025, 10, 5), null },
                    { 10, Guid.Parse("d0e1f2a3-b4c5-4d5e-8f9a-0b1c2d3e4f5a"), "Patrícia", "Souza", new DateTime(1993, 1, 20), "Feminino", "01234567890", "012345678", "PE", "patricia.souza@email.com", "(81) 99678-9012", null, 4, "SULA012345", new DateTime(2026, 5, 15), null },
                    { 11, Guid.Parse("e1f2a3b4-c5d6-4e5f-8a9b-0c1d2e3f4a5b"), "Bruno", "Carvalho", new DateTime(1980, 6, 7), "Masculino", null, "111222333", "GO", "bruno.carvalho@email.com", "(62) 98789-0123", "(62) 3567-8901", 2, "AMIL111222", new DateTime(2025, 7, 25), null },
                    { 12, Guid.Parse("f2a3b4c5-d6e7-4f5a-8b9c-0d1e2f3a4b5c"), "Camila", "Barbosa", new DateTime(1991, 10, 3), "Feminino", null, "222333444", "ES", "camila.barbosa@email.com", "(27) 97890-1234", null, 5, "PART222333", new DateTime(2026, 4, 30), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover Pacientes
            migrationBuilder.DeleteData(
                table: "Pacientes",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });

            // Remover Convênios
            migrationBuilder.DeleteData(
                table: "Convenios",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5 });
        }
    }
}
