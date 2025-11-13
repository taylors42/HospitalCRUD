import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Crud } from '../crud';
import { Entidade, Convenio, LABELS, MENSAGENS, CreatePacienteDTO, UpdatePacienteDTO } from '../config';

@Component({
  selector: 'app-pacientes',
  imports: [CommonModule, FormsModule],
  templateUrl: './pacientes.html',
  styleUrl: './pacientes.css',
})
export class Pacientes implements OnInit {
  lista: Entidade[] = [];
  convenios: Convenio[] = [];
  registroAtual: Entidade = this.limparRegistro();
  isEditando = false;
  labels = LABELS;
  mensagens = MENSAGENS;
  dataMaxima = new Date().toISOString().split('T')[0];

  constructor(private crudService: Crud) {}

  ngOnInit(): void {
    this.carregarDados();
    this.carregarConvenios();
  }

  carregarDados(): void {
    this.crudService.getAll().subscribe({
      next: (dados) => {
        this.lista = dados;
      },
      error: (erro) => {
        console.error('=== ERRO AO CARREGAR PACIENTES ===');
        console.error('Erro completo:', erro);
        console.error('Status HTTP:', erro.status);
        console.error('Status Text:', erro.statusText);
        console.error('Corpo do erro (erro.error):', erro.error);
        console.error('JSON do erro completo:', JSON.stringify(erro, null, 2));
        if (erro.error) {
          console.error('JSON do erro.error:', JSON.stringify(erro.error, null, 2));
        }
        console.error('===================================');

        let mensagemErro = this.mensagens.erroCarregar;
        let detalhesAdicionais = '';

        // Tenta pegar mensagem da API
        if (erro.error?.mensagem) {
          mensagemErro = erro.error.mensagem;
        } else if (erro.error?.message) {
          mensagemErro = erro.error.message;
        } else if (typeof erro.error === 'string') {
          mensagemErro = erro.error;
        }

        // Adiciona status HTTP
        if (erro.status) {
          detalhesAdicionais = `\n\nStatus HTTP: ${erro.status}`;
          if (erro.statusText) {
            detalhesAdicionais += ` - ${erro.statusText}`;
          }
        }

        alert(mensagemErro + detalhesAdicionais);
      },
    });
  }

  carregarConvenios(): void {
    this.crudService.getAllConvenios().subscribe({
      next: (dados) => {
        console.log('Convênios carregados:', dados);
        this.convenios = dados;
      },
      error: (erro) => {
        console.error('=== ERRO AO CARREGAR CONVÊNIOS ===');
        console.error('Erro completo:', erro);
        console.error('Status HTTP:', erro.status);
        console.error('Status Text:', erro.statusText);
        console.error('Corpo do erro (erro.error):', erro.error);
        console.error('JSON do erro completo:', JSON.stringify(erro, null, 2));
        if (erro.error) {
          console.error('JSON do erro.error:', JSON.stringify(erro.error, null, 2));
        }
        console.error('===================================');

        let mensagemErro = 'Erro ao carregar convênios.';
        let detalhesAdicionais = '';

        // Tenta pegar mensagem da API
        if (erro.error?.mensagem) {
          mensagemErro = erro.error.mensagem;
        } else if (erro.error?.message) {
          mensagemErro = erro.error.message;
        } else if (typeof erro.error === 'string') {
          mensagemErro = erro.error;
        }

        // Adiciona status HTTP
        if (erro.status) {
          detalhesAdicionais = `\n\nStatus HTTP: ${erro.status}`;
          if (erro.statusText) {
            detalhesAdicionais += ` - ${erro.statusText}`;
          }
        }

        alert(mensagemErro + detalhesAdicionais);
      },
    });
  }

  salvar(): void {
    if (this.isEditando && this.registroAtual.key) {
      const { key, ...resto } = this.registroAtual;

      console.log('registroAtual completo:', this.registroAtual);
      console.log('resto:', resto);

      // Garante que a data esteja no formato correto YYYY-MM-DD
      const dataFormatada = resto.dataDeNascimento ?
        resto.dataDeNascimento.split('T')[0] : resto.dataDeNascimento;

      const dadosAtualizacao: UpdatePacienteDTO = {
        nome: resto.nome,
        sobrenome: resto.sobrenome,
        dataDeNascimento: dataFormatada,
        genero: resto.genero,
        email: resto.email,
        convenio: (resto.convenio && resto.convenio !== '') ? String(resto.convenio) : undefined,
        celular: (resto.celular && resto.celular.trim() !== '') ? resto.celular.replace(/\D/g, '') : undefined,
        telefoneFixo: (resto.telefoneFixo && resto.telefoneFixo.trim() !== '') ? resto.telefoneFixo.replace(/\D/g, '') : undefined
      };

      console.log('dadosAtualizacao:', dadosAtualizacao);
      // Nota: RG, RGUF e CPF não são enviados no update pois não podem ser alterados
      this.crudService.update(key, dadosAtualizacao).subscribe({
        next: (resposta: any) => {
          console.log('Resposta atualização:', resposta);
          alert(resposta?.mensagem || this.mensagens.atualizado);
          this.carregarDados();
          this.cancelar();
        },
        error: (erro) => {
          console.error('=== ERRO AO ATUALIZAR PACIENTE ===');
          console.error('Erro completo:', erro);
          console.error('Status HTTP:', erro.status);
          console.error('Status Text:', erro.statusText);
          console.error('Corpo do erro (erro.error):', erro.error);
          console.error('Tipo do erro.error:', typeof erro.error);
          console.error('JSON do erro completo:', JSON.stringify(erro, null, 2));
          if (erro.error) {
            console.error('JSON do erro.error:', JSON.stringify(erro.error, null, 2));
          }
          console.error('===================================');

          let mensagemErro = '';
          let detalhesAdicionais = '';

          // Verifica se é erro de RG duplicado
          if (typeof erro.error === 'string' && erro.error.includes('IX_Pacientes_RG')) {
            mensagemErro = 'Já existe um paciente cadastrado com este RG. Por favor, verifique o número digitado.';
          }
          // Verifica se é erro de CPF duplicado
          else if (typeof erro.error === 'string' && erro.error.includes('IX_Pacientes_CPF')) {
            mensagemErro = 'Já existe um paciente cadastrado com este CPF. Por favor, verifique o número digitado.';
          }
          // Verifica se é erro de Email duplicado
          else if (typeof erro.error === 'string' && erro.error.includes('IX_Pacientes_Email')) {
            mensagemErro = 'Já existe um paciente cadastrado com este Email. Por favor, verifique o email digitado.';
          }
          // Tenta pegar mensagem da API de várias formas possíveis
          else if (erro.error?.mensagem) {
            mensagemErro = erro.error.mensagem;
          } else if (erro.error?.message) {
            mensagemErro = erro.error.message;
          } else if (erro.error?.title) {
            mensagemErro = erro.error.title;
          } else if (typeof erro.error === 'string') {
            mensagemErro = erro.error;
          } else if (erro.message) {
            mensagemErro = erro.message;
          } else {
            mensagemErro = this.mensagens.erroAtualizar;
          }

          // Adiciona detalhes do status HTTP
          if (erro.status) {
            detalhesAdicionais = `\n\nStatus HTTP: ${erro.status}`;
            if (erro.statusText) {
              detalhesAdicionais += ` - ${erro.statusText}`;
            }
          }

          // Adiciona detalhes extras se existirem
          if (erro.error?.errors) {
            detalhesAdicionais += '\n\nDetalhes: ' + JSON.stringify(erro.error.errors, null, 2);
          }

          // Se a mensagem ainda estiver vazia, mostra o objeto todo
          if (!mensagemErro || mensagemErro === this.mensagens.erroAtualizar) {
            if (erro.error && typeof erro.error === 'object') {
              detalhesAdicionais += '\n\nResposta da API:\n' + JSON.stringify(erro.error, null, 2);
            }
          }

          alert(mensagemErro + detalhesAdicionais);
          this.carregarDados();
        },
      });
    } else {
      const dadosCriacao: CreatePacienteDTO = {
        nome: this.registroAtual.nome,
        sobrenome: this.registroAtual.sobrenome,
        dataDeNascimento: this.registroAtual.dataDeNascimento,
        genero: this.registroAtual.genero,
        rg: this.registroAtual.rg.replace(/\D/g, ''),
        rguf: this.registroAtual.rguf,
        email: this.registroAtual.email,
        convenio: String(this.registroAtual.convenio),
        ...(this.registroAtual.cpf && { cpf: this.registroAtual.cpf.replace(/\D/g, '') }),
        ...(this.registroAtual.celular && { celular: this.registroAtual.celular.replace(/\D/g, '') }),
        ...(this.registroAtual.telefoneFixo && { telefoneFixo: this.registroAtual.telefoneFixo.replace(/\D/g, '') })
      };
      this.crudService.create(dadosCriacao).subscribe({
        next: (resposta: any) => {
          console.log('Resposta criação:', resposta);
          alert(resposta?.mensagem || this.mensagens.criado);
          this.carregarDados();
          this.cancelar();
        },
        error: (erro) => {
          console.error('=== ERRO AO CRIAR PACIENTE ===');
          console.error('Erro completo:', erro);
          console.error('Status HTTP:', erro.status);
          console.error('Status Text:', erro.statusText);
          console.error('Corpo do erro (erro.error):', erro.error);
          console.error('Tipo do erro.error:', typeof erro.error);
          console.error('JSON do erro completo:', JSON.stringify(erro, null, 2));
          if (erro.error) {
            console.error('JSON do erro.error:', JSON.stringify(erro.error, null, 2));
          }
          console.error('===================================');

          let mensagemErro = '';
          let detalhesAdicionais = '';

          // Verifica se é erro de RG duplicado
          if (typeof erro.error === 'string' && erro.error.includes('IX_Pacientes_RG')) {
            mensagemErro = 'Já existe um paciente cadastrado com este RG. Por favor, verifique o número digitado.';
          }
          // Verifica se é erro de CPF duplicado
          else if (typeof erro.error === 'string' && erro.error.includes('IX_Pacientes_CPF')) {
            mensagemErro = 'Já existe um paciente cadastrado com este CPF. Por favor, verifique o número digitado.';
          }
          // Verifica se é erro de Email duplicado
          else if (typeof erro.error === 'string' && erro.error.includes('IX_Pacientes_Email')) {
            mensagemErro = 'Já existe um paciente cadastrado com este Email. Por favor, verifique o email digitado.';
          }
          // Tenta pegar mensagem da API de várias formas possíveis
          else if (erro.error?.mensagem) {
            mensagemErro = erro.error.mensagem;
          } else if (erro.error?.message) {
            mensagemErro = erro.error.message;
          } else if (erro.error?.title) {
            mensagemErro = erro.error.title;
          } else if (typeof erro.error === 'string') {
            mensagemErro = erro.error;
          } else if (erro.message) {
            mensagemErro = erro.message;
          } else {
            mensagemErro = this.mensagens.erroCriar;
          }

          // Adiciona detalhes do status HTTP
          if (erro.status) {
            detalhesAdicionais = `\n\nStatus HTTP: ${erro.status}`;
            if (erro.statusText) {
              detalhesAdicionais += ` - ${erro.statusText}`;
            }
          }

          // Adiciona detalhes extras se existirem
          if (erro.error?.errors) {
            detalhesAdicionais += '\n\nDetalhes: ' + JSON.stringify(erro.error.errors, null, 2);
          }

          // Se a mensagem ainda estiver vazia, mostra o objeto todo
          if (!mensagemErro || mensagemErro === this.mensagens.erroCriar) {
            if (erro.error && typeof erro.error === 'object') {
              detalhesAdicionais += '\n\nResposta da API:\n' + JSON.stringify(erro.error, null, 2);
            }
          }

          alert(mensagemErro + detalhesAdicionais);
          this.carregarDados();
        },
      });
    }
  }

  editar(registro: Entidade): void {
    this.registroAtual = { ...registro };

    // Formatar data de nascimento para o formato YYYY-MM-DD
    if (this.registroAtual.dataDeNascimento) {
      // Remove a parte do horário se existir (ex: "2000-01-15T00:00:00" -> "2000-01-15")
      this.registroAtual.dataDeNascimento = this.registroAtual.dataDeNascimento.split('T')[0];
    }

    // Garantir que o convênio seja sempre o ID correto
    if (this.registroAtual.convenio) {
      // Tenta encontrar o convênio tanto pelo nome quanto pelo ID
      const convenioEncontrado = this.convenios.find(conv => {
        // Verifica se corresponde ao nome do convênio
        if (conv.nome.toLowerCase() === String(this.registroAtual.convenio).toLowerCase()) {
          return true;
        }
        // Verifica se corresponde ao ID do convênio
        if (String(conv.id) === String(this.registroAtual.convenio)) {
          return true;
        }
        return false;
      });

      if (convenioEncontrado) {
        // Sempre define como string do ID
        this.registroAtual.convenio = String(convenioEncontrado.id);
      } else {
        console.warn('Convênio não encontrado na lista:', this.registroAtual.convenio);
      }
    }

    this.isEditando = true;
  }

  deletar(key: string | undefined): void {
    if (!key) return;

    if (confirm(this.mensagens.confirmarDelete)) {
      this.crudService.delete(key).subscribe({
        next: (resposta) => {
          alert(resposta.mensagem);
          this.carregarDados();
        },
        error: (erro) => {
          console.error('=== ERRO AO DELETAR PACIENTE ===');
          console.error('Erro completo:', erro);
          console.error('Status HTTP:', erro.status);
          console.error('Status Text:', erro.statusText);
          console.error('Corpo do erro (erro.error):', erro.error);
          console.error('Tipo do erro.error:', typeof erro.error);
          console.error('JSON do erro completo:', JSON.stringify(erro, null, 2));
          if (erro.error) {
            console.error('JSON do erro.error:', JSON.stringify(erro.error, null, 2));
          }
          console.error('===================================');

          let mensagemErro = '';
          let detalhesAdicionais = '';

          // Tenta pegar mensagem da API de várias formas possíveis
          if (erro.error?.mensagem) {
            mensagemErro = erro.error.mensagem;
          } else if (erro.error?.message) {
            mensagemErro = erro.error.message;
          } else if (erro.error?.title) {
            mensagemErro = erro.error.title;
          } else if (typeof erro.error === 'string') {
            mensagemErro = erro.error;
          } else if (erro.message) {
            mensagemErro = erro.message;
          } else {
            mensagemErro = this.mensagens.erroDeletar;
          }

          // Adiciona detalhes do status HTTP
          if (erro.status) {
            detalhesAdicionais = `\n\nStatus HTTP: ${erro.status}`;
            if (erro.statusText) {
              detalhesAdicionais += ` - ${erro.statusText}`;
            }
          }

          // Adiciona detalhes extras se existirem
          if (erro.error?.errors) {
            detalhesAdicionais += '\n\nDetalhes: ' + JSON.stringify(erro.error.errors, null, 2);
          }

          // Se a mensagem ainda estiver vazia, mostra o objeto todo
          if (!mensagemErro || mensagemErro === this.mensagens.erroDeletar) {
            if (erro.error && typeof erro.error === 'object') {
              detalhesAdicionais += '\n\nResposta da API:\n' + JSON.stringify(erro.error, null, 2);
            }
          }

          alert(mensagemErro + detalhesAdicionais);
          this.carregarDados();
        }
      });
    }
  }

  cancelar(): void {
    this.registroAtual = this.limparRegistro();
    this.isEditando = false;
  }

  private limparRegistro(): Entidade {
    return {
      nome: '',
      sobrenome: '',
      dataDeNascimento: '',
      genero: '',
      cpf: '',
      rg: '',
      rguf: '',
      email: '',
      celular: '',
      telefoneFixo: '',
      convenio: '',
    };
  }

  validarCPF(cpf: string | undefined): boolean {
    if (!cpf) return true; // CPF é opcional

    const cpfLimpo = cpf.replace(/\D/g, '');

    if (cpfLimpo.length !== 11) return false;

    // Verifica se todos os dígitos são iguais
    if (/^(\d)\1{10}$/.test(cpfLimpo)) return false;

    // Validação dos dígitos verificadores
    let soma = 0;
    for (let i = 0; i < 9; i++) {
      soma += parseInt(cpfLimpo.charAt(i)) * (10 - i);
    }
    let resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    if (resto !== parseInt(cpfLimpo.charAt(9))) return false;

    soma = 0;
    for (let i = 0; i < 10; i++) {
      soma += parseInt(cpfLimpo.charAt(i)) * (11 - i);
    }
    resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    if (resto !== parseInt(cpfLimpo.charAt(10))) return false;

    return true;
  }

  validarRG(rg: string): boolean {
    if (!rg) return false;

    const rgLimpo = rg.replace(/\D/g, '');

    // RG deve ter entre 7 e 9 dígitos
    return rgLimpo.length >= 7 && rgLimpo.length <= 9;
  }

  verificarCPF(): void {
    if (this.registroAtual.cpf && !this.validarCPF(this.registroAtual.cpf)) {
      alert('CPF inválido! Verifique o número digitado.');
      this.registroAtual.cpf = '';
    }
  }

  verificarRG(): void {
    if (this.registroAtual.rg && !this.validarRG(this.registroAtual.rg)) {
      alert('RG inválido! Deve conter entre 7 e 9 dígitos.');
      this.registroAtual.rg = '';
    }
  }
}
