using System.ComponentModel.DataAnnotations;

namespace PacientesAPI.DTOs;

public sealed class UpdatePacienteDTo
{
    public string? Nome { get; set; }
    public string? Sobrenome { get; set; }
    public DateTime? DataDeNascimento { get; set; }
    public string? Genero { get; set; }
    public string? Email { get; set; }
    public string? Celular { get; set; }
    public string? TelefoneFixo { get; set; }
    public string? Convenio { get; set; }
}
