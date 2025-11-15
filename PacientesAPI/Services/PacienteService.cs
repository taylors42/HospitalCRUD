using Models;
using PacientesAPI.DTOs;
using PacientesAPI.Repository;
using System.Text.RegularExpressions;

namespace PacientesAPI.Services;

public class PacienteService(IPacienteRepository pacienteRepository, IConvenioRepository convenioRepository) : IPacienteService
{
    public IEnumerable<object> ListarPacientesAtivos()
    {
        var pacientes = pacienteRepository.GetPacientesAtivos();
        var convenios = convenioRepository.GetAll().ToList();

        return pacientes.Select(p => new
        {
            nome = p.Nome,
            sobrenome = p.Sobrenome,
            dataDeNascimento = p.DataDeNascimento,
            genero = p.Genero,
            cpf = p.CPF,
            rg = p.RG,
            rguf = p.RGUF,
            email = p.Email,
            celular = p.Celular,
            telefoneFixo = p.TelefoneFixo,
            convenio = convenios.FirstOrDefault(c => c.Id == p.ConvenioId)?.Nome,
            numeroCarteirinha = p.NumeroCarteirinha,
            validadeCarteirinha = p.ValidadeCarteirinha,
            key = p.Key
        }).ToList();
    }

    public (bool sucesso, string mensagem) CriarPaciente(CreatePacienteDTo dto)
    {
        if (dto.CPF is not null)
        {
            if (dto.CPF.Length != 11)
                return (false, "CPF inválido - deve conter 11 dígitos");

            if (!ValidarCpf(dto.CPF))
                return (false, "CPF inválido");
        }

        if (dto.RG is not null && dto.RG.Length != 9)
            return (false, "RG inválido");

        if (ValidarUF(dto.RGUF) is false)
            return (false, "UF do RG inválida");

        if (ValidarEmail(dto.Email) is false)
            return (false, "Email inválido");

        var pacientExists = pacienteRepository.GetAll().Any(p => p.CPF == dto.CPF);
        if (pacientExists)
            return (false, "PACIENTE EXISTENTE");

        var rgExists = pacienteRepository.GetAll().Any(p => p.RG == dto.RG);
        if (rgExists)
            return (false, "RG já cadastrado");

        if (dto.DataDeNascimento >= DateTime.Today)
            return (false, "DATA DE NASCIMENTO FUTURA");

        if (dto.Celular is not null && ValidarTelefone(dto.Celular, true) is false)
            return (false, "Celular inválido - deve conter 11 dígitos (DDD + 9 dígitos)");

        if (dto.TelefoneFixo is not null && ValidarTelefone(dto.TelefoneFixo, false) is false)
            return (false, "Telefone fixo inválido - deve conter 10 dígitos (DDD + 8 dígitos)");

        if (dto.TelefoneFixo is null && dto.Celular is null)
            return (false, "É necessário preencher pelo menos o telefone ou o celular");

        var convenio = convenioRepository.GetById(dto.Convenio);
        if (convenio is null)
            return (false, "Convênio não existente");

        if (string.IsNullOrWhiteSpace(dto.NumeroCarteirinha))
            return (false, "Número da carteirinha é obrigatório");

        if (dto.ValidadeCarteirinha <= DateTime.Now)
            return (false, "Validade da carteirinha é obrigatória ser maior que hoje");

        var paciente = new Paciente
        {
            Nome = dto.Nome,
            Sobrenome = dto.Sobrenome,
            Email = dto.Email,
            DataDeNascimento = dto.DataDeNascimento,
            Genero = dto.Genero,
            CPF = dto.CPF,
            RG = dto.RG,
            RGUF = dto.RGUF,
            Celular = dto.Celular,
            TelefoneFixo = dto.TelefoneFixo,
            ConvenioId = convenio.Id,
            NumeroCarteirinha = dto.NumeroCarteirinha,
            ValidadeCarteirinha = dto.ValidadeCarteirinha
        };

        pacienteRepository.Add(paciente);
        pacienteRepository.SaveChanges();

        return (true, "Paciente adicionado com sucesso");
    }

    public (bool sucesso, string mensagem) AtualizarPaciente(Guid key, UpdatePacienteDTo dto)
    {
        var paciente = pacienteRepository.GetByKey(key);
        if (paciente is null)
            return (false, "Paciente não existe");

        paciente.Nome = dto.Nome ?? paciente.Nome;
        paciente.Sobrenome = dto.Sobrenome ?? paciente.Sobrenome;

        if (dto.DataDeNascimento is not null && dto.DataDeNascimento >= DateTime.Today)
            return (false, "Data de Nascimento Inválida");

        paciente.DataDeNascimento = dto.DataDeNascimento ?? paciente.DataDeNascimento;

        if (dto.Email is not null && ValidarEmail(dto.Email) is false)
            return (false, "Email Inválido");

        paciente.Email = dto.Email ?? paciente.Email;
        paciente.Genero = dto.Genero ?? paciente.Genero;

        if (dto.Celular is not null && ValidarTelefone(dto.Celular, true) is false)
            return (false, "Celular inválido - deve conter 11 dígitos (DDD + 9 dígitos)");

        if (dto.TelefoneFixo is not null && ValidarTelefone(dto.TelefoneFixo, false) is false)
            return (false, "Telefone fixo inválido - deve conter 10 dígitos (DDD + 8 dígitos)");

        var novoCelular = dto.Celular ?? paciente.Celular;
        var novoTelefoneFixo = dto.TelefoneFixo ?? paciente.TelefoneFixo;

        if (novoCelular is null && novoTelefoneFixo is null)
            return (false, "É necessário preencher pelo menos o telefone ou o celular");

        paciente.Celular = novoCelular;
        paciente.TelefoneFixo = novoTelefoneFixo;

        if (dto.NumeroCarteirinha is not null)
            paciente.NumeroCarteirinha = dto.NumeroCarteirinha;

        if (dto.ValidadeCarteirinha is not null && dto.ValidadeCarteirinha <= DateTime.Now)
            return (false, "Validade da carteirinha deve estar no formato MM/AAAA");

        paciente.ValidadeCarteirinha = dto.ValidadeCarteirinha ?? paciente.ValidadeCarteirinha;

        if (dto.Convenio is not null)
        {
            var convenio = convenioRepository.GetAll().FirstOrDefault(c => c.Nome == dto.Convenio);
            if (convenio is null)
                return (false, "Convênio inexistente");

            paciente.ConvenioId = convenio.Id;
        }

        pacienteRepository.Update(paciente);
        pacienteRepository.SaveChanges();

        return (true, "Paciente Atualizado com sucesso");
    }

    public (bool sucesso, string mensagem) ExcluirPaciente(Guid key)
    {
        var paciente = pacienteRepository.GetByKey(key);
        if (paciente is null)
            return (false, "Paciente não existente");

        if (paciente.DataDeExclusao is not null)
            return (false, "Paciente já excluído");

        pacienteRepository.Delete(paciente);
        pacienteRepository.SaveChanges();

        return (true, "Excluído com sucesso");
    }

    #region Validações
    private static bool ValidarUF(string? uf)
    {
        if (string.IsNullOrEmpty(uf))
            return false;

        var ufsValidas = new[] { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA",
                                  "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN",
                                  "RS", "RO", "RR", "SC", "SP", "SE", "TO" };

        return ufsValidas.Contains(uf.ToUpper());
    }

    private static bool ValidarTelefone(string? telefone, bool isCelular)
    {
        if (string.IsNullOrEmpty(telefone))
            return true;

        telefone = new string(telefone.Where(char.IsDigit).ToArray());

        if (isCelular)
            return telefone.Length == 11;
        else
            return telefone.Length == 10;
    }

    private static bool ValidarCpf(string cpf)
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

    private static bool ValidarEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
    #endregion
}
