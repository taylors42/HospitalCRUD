using Models;

namespace PacientesAPI.Repository;

public interface IConvenioRepository
{
    IEnumerable<Convenio> GetAll();
    Convenio? GetById(int id);
}
