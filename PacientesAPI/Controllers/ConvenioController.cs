using BancoDeDados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PacientesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ConveniosController(PacientesDbContext contexto) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var listaConvenios = contexto
            .Convenios
            .AsNoTracking()
            .ToList();

        return StatusCode(200, listaConvenios);
    }
}
