using Microsoft.AspNetCore.Mvc;
using PacientesAPI.Services;

namespace PacientesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ConveniosController(IConvenioService convenioService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => StatusCode(200, convenioService.ListarConvenios());
}