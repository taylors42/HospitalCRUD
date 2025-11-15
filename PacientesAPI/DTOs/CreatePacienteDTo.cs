using System.ComponentModel.DataAnnotations;

public sealed class CreatePacienteDTo
{
    [Required]
    public string Nome { get; set; }
    [Required]
    public string Sobrenome { get; set; }
    [Required]
    public DateTime DataDeNascimento { get; set; }
    [Required]
    public string Genero { get; set; }
    public string? CPF { get; set; }
    [Required]
    public string RG { get; set; }
    [Required]
    public string RGUF { get; set; }
    [Required]
    public string Email { get; set; }
    public string? Celular { get; set; }
    public string? TelefoneFixo { get; set; }
    [Required]
    public int Convenio { get; set; }
    [Required]
    public string NumeroCarteirinha { get; set; }
    [Required]
    public DateTime ValidadeCarteirinha { get; set; }
}
