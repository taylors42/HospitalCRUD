using Models;

namespace PacientesAPI.Services;

public interface IConvenioService
{
    IEnumerable<Convenio> ListarConvenios();
}
