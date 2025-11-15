using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoDeDados.Migrations
{
    /// <inheritdoc />
    public partial class PopularTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserir Convênios
            migrationBuilder.InsertData(
                table: "Convenios",
                columns: new[] { "Nome" },
                values: new object[,]
                {
                    { "Unimed" },
                    { "Bradesco Saúde" },
                    { "SulAmérica" },
                    { "Amil" },
                    { "NotreDame Intermédica" }
                });

            // Inserir Pacientes
            migrationBuilder.InsertData(
                table: "Pacientes",
                columns: new[] { "Key", "Nome", "Sobrenome", "DataDeNascimento", "Genero", "CPF", "RG", "RGUF", "Email", "Celular", "TelefoneFixo", "ConvenioId", "NumeroCarteirinha", "ValidadeCarteirinha", "DataDeExclusao" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "João", "Silva", new DateTime(1985, 3, 15), "Masculino", "12345678901", "123456789", "SP", "joao.silva@email.com", "(11) 98765-4321", "(11) 3456-7890", 1, "CARD001", new DateTime(2026, 12, 31), null },
                    { Guid.NewGuid(), "Maria", "Santos", new DateTime(1990, 7, 22), "Feminino", "23456789012", "234567890", "RJ", "maria.santos@email.com", "(21) 99876-5432", null, 2, "CARD002", new DateTime(2025, 6, 30), null },
                    { Guid.NewGuid(), "Pedro", "Oliveira", new DateTime(1978, 11, 5), "Masculino", "34567890123", "345678901", "MG", "pedro.oliveira@email.com", "(31) 97654-3210", "(31) 2345-6789", 3, "CARD003", new DateTime(2026, 3, 31), null },
                    { Guid.NewGuid(), "Ana", "Costa", new DateTime(1995, 2, 18), "Feminino", "45678901234", "456789012", "SP", "ana.costa@email.com", "(11) 96543-2109", null, 1, "CARD004", new DateTime(2025, 12, 31), null },
                    { Guid.NewGuid(), "Carlos", "Ferreira", new DateTime(1982, 9, 30), "Masculino", "56789012345", "567890123", "RS", "carlos.ferreira@email.com", "(51) 95432-1098", "(51) 3234-5678", 4, "CARD005", new DateTime(2026, 9, 30), null },
                    { Guid.NewGuid(), "Juliana", "Almeida", new DateTime(1988, 5, 12), "Feminino", "67890123456", "678901234", "PR", "juliana.almeida@email.com", "(41) 94321-0987", null, 5, "CARD006", new DateTime(2025, 11, 30), null },
                    { Guid.NewGuid(), "Roberto", "Pereira", new DateTime(1975, 12, 25), "Masculino", "78901234567", "789012345", "BA", "roberto.pereira@email.com", "(71) 93210-9876", "(71) 3123-4567", 2, "CARD007", new DateTime(2026, 8, 31), null },
                    { Guid.NewGuid(), "Fernanda", "Lima", new DateTime(1992, 4, 8), "Feminino", "89012345678", "890123456", "CE", "fernanda.lima@email.com", "(85) 92109-8765", null, 3, "CARD008", new DateTime(2025, 10, 31), null },
                    { Guid.NewGuid(), "Ricardo", "Souza", new DateTime(1980, 8, 17), "Masculino", "90123456789", "901234567", "DF", "ricardo.souza@email.com", "(61) 91098-7654", "(61) 3012-3456", 1, "CARD009", new DateTime(2026, 5, 31), null },
                    { Guid.NewGuid(), "Patrícia", "Rodrigues", new DateTime(1987, 1, 29), "Feminino", "01234567890", "012345678", "SC", "patricia.rodrigues@email.com", "(48) 90987-6543", null, 4, "CARD010", new DateTime(2025, 7, 31), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Deletar todos os pacientes
            migrationBuilder.Sql("DELETE FROM Pacientes WHERE Id IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10)");

            // Deletar todos os convênios
            migrationBuilder.Sql("DELETE FROM Convenios WHERE Id IN (1, 2, 3, 4, 5)");
        }
    }
}
