using Microsoft.AspNetCore.Mvc;
using PacientesAPI.DTOs;
using PacientesAPI.Services;

namespace PacientesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PacienteController(IPacienteService pacienteService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        StatusCode(200, pacienteService.ListarPacientesAtivos());

    [HttpPost]
    public IActionResult Post([FromBody] CreatePacienteDTo paciente)
    {
        try
        {
            var (sucesso, mensagem) = pacienteService.CriarPaciente(paciente);

            if (sucesso is false)
                return StatusCode(400, new { mensagem });

            return StatusCode(200, new { mensagem });
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno no servidor");
        }
    }

    [HttpPatch("{key}")]
    public IActionResult Patch(Guid key, [FromBody] UpdatePacienteDTo dto)
    {
        try
        {
            var (sucesso, mensagem) = pacienteService.AtualizarPaciente(key, dto);

            if (sucesso is false)
                return StatusCode(400, new { mensagem });

            return StatusCode(200, new { mensagem });
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno no servidor");
        }
    }

    [HttpDelete("{key}")]
    public IActionResult Delete(Guid key)
    {
        try
        {
            var (sucesso, mensagem) = pacienteService.ExcluirPaciente(key);

            if (sucesso is false)
                return StatusCode(404, new { mensagem });

            return StatusCode(200, new { mensagem });
        }
        catch (Exception)
        {
            return StatusCode(500, new { mensagem = "Erro interno no servidor" });
        }
    }
}