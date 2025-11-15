using BancoDeDados;
using Microsoft.EntityFrameworkCore;
using Models;

namespace PacientesAPI.Repository;

public sealed class PacienteRepository(PacientesDbContext context) : IPacienteRepository
{
    public IEnumerable<Paciente> GetAll() => 
        context.Pacientes.AsNoTracking().ToList();

    public Paciente? GetByKey(Guid key) => 
        context.Pacientes.AsNoTracking().FirstOrDefault(p => p.Key == key);

    public IEnumerable<Paciente> GetPacientesAtivos() =>
        context.Pacientes
            .Where(p => p.DataDeExclusao == null)
            .AsNoTracking()
            .ToList();

    public void Add(Paciente paciente) =>
        context.Pacientes.Add(paciente);

    public void Update(Paciente paciente) =>
        context.Pacientes.Update(paciente);

    public void Delete(Paciente paciente)
    {
        paciente.DataDeExclusao = DateTime.Now;
        context.Pacientes.Update(paciente);
    }

    public void SaveChanges() => context.SaveChanges();
}
