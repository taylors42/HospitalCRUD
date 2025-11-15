using BancoDeDados;
using Microsoft.EntityFrameworkCore;
using Models;

namespace PacientesAPI.Repository;

public sealed class ConvenioRepository(PacientesDbContext context) : IConvenioRepository
{
    public IEnumerable<Convenio> GetAll() =>
        context.Convenios.AsNoTracking().ToList();

    public Convenio? GetById(int id) => 
        context.Convenios.AsNoTracking().FirstOrDefault(c => c.Id == id);
}
