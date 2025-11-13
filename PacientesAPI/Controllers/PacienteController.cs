using BancoDeDados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using PacientesAPI.DTOs;
using System.Text.RegularExpressions;

namespace PacientesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PacienteController(PacientesDbContext contexto) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var listaPacientes = contexto
            .Pacientes
            .Where(p => p.DataDeExclusao == null)
            .AsNoTracking()
            .Select(p => new 
            {
                p.Nome,
                p.Sobrenome,
                p.DataDeNascimento,
                p.Genero,
                p.CPF,
                p.RG,
                p.RGUF,
                p.Email,
                p.Celular,
                p.TelefoneFixo,
                Convenio = contexto.Convenios.FirstOrDefault(c => c.Id == p.ConvenioId)!.Nome,
                p.ValidadeCarteirinha,
                p.Key
            })
            .ToList();

        return StatusCode(200, listaPacientes);
    }

    [HttpPost]
    public IActionResult Post([FromBody] CreatePacienteDTo paciente)
    {
        try
        {
            if (paciente.CPF is not null)
            {
                if (paciente.CPF.Length != 11)
                {
                    return StatusCode(
                        400,
                        new { mensagem = "CPF inválido - deve conter 11 dígitos" }
                    );
                }

                if (ValidarCpf(paciente.CPF) is false)
                {
                    return StatusCode(
                        400,
                        new { mensagem = "CPF inválido" }
                    );
                }
            }

            if (paciente.RG is not null && paciente.RG.Length != 9)
            {
                return StatusCode(
                    400,
                    new { mensagem = "RG inválido" }
                );
            }

            if (ValidarUF(paciente.RGUF) is false)
            {
                return StatusCode(
                    400,
                    new { mensagem = "UF do RG inválida" }
                );
            }

            if (ValidarEmail(paciente.Email) is false)
            {
                return StatusCode(
                    400,
                    new { mensagem = "Email inválido" }
                );
            }

            var pacientExists = contexto
                .Pacientes
                .Any(p => p.CPF == paciente.CPF);

            if (pacientExists)
            {
                return StatusCode(
                    400,
                    new { mensagem = "PACIENTE EXISTENTE" }
                );
            }

            var rgExists = contexto
                .Pacientes
                .Any(p => p.RG == paciente.RG);

            if (rgExists)
            {
                return StatusCode(
                    400,
                    new { mensagem = "RG já cadastrado" }
                );
            }

            if (paciente.DataDeNascimento >= DateTime.Today)
            {
                return StatusCode(
                    400,
                    new { mensagem = "DATA DE NASCIMENTO FUTURA" }
                );
            }

            if (paciente.Celular is not null && ValidarTelefone(paciente.Celular, true) is false)
            {
                return StatusCode(
                    400,
                    new { mensagem = "Celular inválido - deve conter 11 dígitos (DDD + 9 dígitos)" }
                );
            }

            if (paciente.TelefoneFixo is not null && !ValidarTelefone(paciente.TelefoneFixo, false))
            {
                return StatusCode(
                    400,
                    new { mensagem = "Telefone fixo inválido - deve conter 10 dígitos (DDD + 8 dígitos)" }
                );
            }

            var telefoneExiste = paciente.TelefoneFixo is null;
            var celularExiste = paciente.Celular is null;
            if (telefoneExiste && celularExiste)
            {
                return StatusCode(
                    400,
                    new { mensagem = "É necessário preencher pelo menos o telefone ou o celular" }
                );
            }

            var convenio = contexto
                .Convenios
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == paciente.Convenio);

            if (convenio is null)
            {
                return StatusCode(
                    400,
                    new { mensagem = "Convênio não existente" }
                );
            }

            var pacienteCompleto = new Paciente
            {
                Nome = paciente.Nome,
                Sobrenome = paciente.Sobrenome,
                Email = paciente.Email,
                DataDeNascimento = paciente.DataDeNascimento,
                Genero = paciente.Genero,
                CPF = paciente.CPF,
                RG = paciente.RG,
                RGUF = paciente.RGUF,
                Celular = paciente.Celular,
                TelefoneFixo = paciente.TelefoneFixo,
                ConvenioId = convenio.Id,
                NumeroCarteirinha = new Random().Next().ToString(),
                ValidadeCarteirinha = DateTime.Now.AddYears(2)
            };

            contexto.Add(pacienteCompleto);
            contexto.SaveChanges();

            return StatusCode(
                200, 
                new { mensagem = "Paciente adicionado com sucesso" } 
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500, 
                "Erro interno no servidor"
            );
        }
    }

    [HttpDelete("{key}")]
    public IActionResult Delete(Guid key)
    {
        try
        {
            var paciente = contexto
                .Pacientes
                .FirstOrDefault(p => p.Key == key);
            if (paciente is null)
            {
                return StatusCode(
                    404, 
                    new { mensagem = "Paciente não existente" }
                );
            }

            if (paciente.DataDeExclusao is not null)
            {
                return StatusCode(
                    404,
                    new { mensagem = "Paciente já excluído" }
                );
            }

            paciente.DataDeExclusao = DateTime.Today;

            contexto.SaveChanges();

            return StatusCode(
                200,
                new { mensagem = "Excluído com sucesso" }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new { mensagem = "Erro interno no servidor" }
            );
        }
    }

    [HttpPatch("{key}")]
    public IActionResult Patch(Guid key, [FromBody] UpdatePacienteDTo dto)
    {
        try
        {
            var paciente = contexto
                .Pacientes
                .FirstOrDefault(p => p.Key == key);

            if (paciente is null)
            {
                return StatusCode(
                    400, 
                    new { mensagem = "Paciente não existe" }
                );
            }

            paciente.Nome = dto.Nome ?? paciente.Nome;
            paciente.Sobrenome = dto.Sobrenome ?? paciente.Sobrenome;

            if (dto.DataDeNascimento is not null && dto.DataDeNascimento >= DateTime.Today)
            {
                return StatusCode(
                    400,
                    new { mensagem = "Data de Nascimento Inválida" }
                ) ;
            }

            paciente.DataDeNascimento = dto.DataDeNascimento ?? paciente.DataDeNascimento;

            if (dto.Email is not null && ValidarEmail(dto.Email) is false)
            {
                return StatusCode(
                    400,
                    new { mensagem = "Email Inválido" }
                );
            }

            paciente.Email = dto.Email ?? paciente.Email;

            paciente.Genero = dto.Genero ?? paciente.Genero;

            if (dto.Celular is not null && !ValidarTelefone(dto.Celular, true))
            {
                return StatusCode(
                    400,
                    new { mensagem = "Celular inválido - deve conter 11 dígitos (DDD + 9 dígitos)" }
                );
            }

            if (dto.TelefoneFixo is not null && !ValidarTelefone(dto.TelefoneFixo, false))
            {
                return StatusCode(
                    400,
                    new { mensagem = "Telefone fixo inválido - deve conter 10 dígitos (DDD + 8 dígitos)" }
                );
            }

            var novoCelular = dto.Celular ?? paciente.Celular;
            var novoTelefoneFixo = dto.TelefoneFixo ?? paciente.TelefoneFixo;

            if (novoCelular is null && novoTelefoneFixo is null)
            {
                return StatusCode(
                    400,
                    new { mensagem = "É necessário preencher pelo menos o telefone ou o celular" }
                );
            }

            paciente.Celular = novoCelular;
            paciente.TelefoneFixo = novoTelefoneFixo;

            if (dto.Convenio is not null)
            {
                var convenio = contexto
                    .Convenios
                    .FirstOrDefault(c => c.Nome == dto.Convenio);

                if (convenio is null)
                {
                    return StatusCode(
                        400,
                        new { mensagem = "Convênio inexistente" }
                    );
                }

                if (paciente.ConvenioId != convenio.Id)
                {
                    paciente.ConvenioId = convenio.Id;
                    paciente.NumeroCarteirinha = string.Concat(convenio.Nome.ToUpper(), new Random().Next().ToString().PadLeft(6, '0').AsSpan(0, 6));
                    paciente.ValidadeCarteirinha = DateTime.Now.AddYears(2);
                }
            }

            contexto.SaveChanges();

            return StatusCode(
                200, 
                new { mensagem = "Paciente Atualizado com sucesso" } 
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno no servidor");
        }
    }

    #region Regras
    public static bool ValidarUF(string? uf)
    {
        if (string.IsNullOrEmpty(uf))
            return false;

        var ufsValidas = new[] { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA",
                                  "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN",
                                  "RS", "RO", "RR", "SC", "SP", "SE", "TO" };

        return ufsValidas.Contains(uf.ToUpper());
    }

    public static bool ValidarTelefone(string? telefone, bool isCelular)
    {
        if (string.IsNullOrEmpty(telefone))
            return true; // Opcional, validação de obrigatoriedade é feita antes

        // Remove caracteres não numéricos
        telefone = new string(telefone.Where(char.IsDigit).ToArray());

        // Celular: 11 dígitos (DDD + 9 + 8 dígitos)
        // Fixo: 10 dígitos (DDD + 8 dígitos)
        if (isCelular)
            return telefone.Length == 11;
        else
            return telefone.Length == 10;
    }

    public static bool ValidarCpf(string cpf)
    {
        cpf = cpf.Replace(".", "").Replace("-", "");

        if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
            return false;

        int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        string digito = resto.ToString();
        tempCpf += digito;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito += resto.ToString();

        return cpf.EndsWith(digito);
    }

    public static bool ValidarEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    #endregion
}
