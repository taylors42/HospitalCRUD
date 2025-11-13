using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BancoDeDados;

public class PacientesDbContextFactory : IDesignTimeDbContextFactory<PacientesDbContext>
{
    public PacientesDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PacientesDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=.;Database=Hospital;Integrated Security=True;TrustServerCertificate=True"
        );

        return new PacientesDbContext(optionsBuilder.Options);
    }
}
