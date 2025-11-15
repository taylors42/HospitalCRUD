using Models;

namespace PacientesAPI.Repository;

public interface IPacienteRepository
{
    IEnumerable<Paciente> GetAll();
    Paciente? GetByKey(Guid key);
    IEnumerable<Paciente> GetPacientesAtivos();
    void Add(Paciente paciente);
    void Update(Paciente paciente);
    void Delete(Paciente paciente);
    void SaveChanges();
}
