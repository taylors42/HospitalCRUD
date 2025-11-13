using System.ComponentModel.DataAnnotations;

namespace Models;

public sealed class Paciente
{
    [Key]
    public int Id { get; set; }
    public Guid Key { get; set; } = Guid.NewGuid();
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public DateTime DataDeNascimento { get; set; }
    [AllowedValues("Masculino", "Feminino", "Outro")]
    public string Genero { get; set; }
    public string? CPF { get; set; }
    public string RG { get; set; }
    public string RGUF { get; set; } = string.Empty;
    public string Email { get; set; }
    public string? Celular { get; set; }
    public string? TelefoneFixo { get; set; }
    public int ConvenioId { get; set; }
    public string NumeroCarteirinha { get; set; }
    public DateTime ValidadeCarteirinha { get; set; }
    public DateTime? DataDeExclusao { get; set; }
}
