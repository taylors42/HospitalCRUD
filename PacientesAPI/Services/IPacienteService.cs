using Models;
using PacientesAPI.DTOs;

namespace PacientesAPI.Services;

public interface IPacienteService
{
    IEnumerable<object> ListarPacientesAtivos();
    (bool sucesso, string mensagem) CriarPaciente(CreatePacienteDTo dto);
    (bool sucesso, string mensagem) AtualizarPaciente(Guid key, UpdatePacienteDTo dto);
    (bool sucesso, string mensagem) ExcluirPaciente(Guid key);
}
