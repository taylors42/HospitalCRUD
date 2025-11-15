using Models;
using PacientesAPI.Repository;

namespace PacientesAPI.Services;

public class ConvenioService(IConvenioRepository convenioRepository) : IConvenioService
{
    public IEnumerable<Convenio> ListarConvenios() =>
        convenioRepository.GetAll();
}
