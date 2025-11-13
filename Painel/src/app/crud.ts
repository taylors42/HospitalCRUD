import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Entidade,
  CreatePacienteDTO,
  UpdatePacienteDTO,
  Convenio,
  getPacienteApiUrl,
  getConvenioApiUrl
} from './config';

@Injectable({
  providedIn: 'root',
})
export class Crud {
  private pacienteApiUrl = getPacienteApiUrl();
  private convenioApiUrl = getConvenioApiUrl();

  constructor(private http: HttpClient) {}

  create(dados: CreatePacienteDTO): Observable<Entidade> {
    const dto = {
      Nome: dados.nome,
      Sobrenome: dados.sobrenome,
      DataDeNascimento: dados.dataDeNascimento,
      Genero: dados.genero,
      RG: dados.rg,
      RGUF: dados.rguf,
      Email: dados.email,
      Convenio: parseInt(dados.convenio, 10),
      ...(dados.cpf && { CPF: dados.cpf }),
      ...(dados.celular && { Celular: dados.celular }),
      ...(dados.telefoneFixo && { TelefoneFixo: dados.telefoneFixo })
    };

    // A API .NET espera os campos diretamente no corpo da requisição, não um objeto "dto"
    const body = dto;
    console.log(JSON.stringify(body, null, 2));
    return this.http.post<Entidade>(this.pacienteApiUrl, body);
  }

  getAll(): Observable<Entidade[]> {
    return this.http.get<Entidade[]>(this.pacienteApiUrl);
  }

  update(key: string, dados: UpdatePacienteDTO): Observable<Entidade> {
    console.log('=== CRUD UPDATE - Dados recebidos ===');
    console.log('dados.convenio:', dados.convenio, '(tipo:', typeof dados.convenio, ')');

    // Monta o DTO interno com campos obrigatórios
    const dto: any = {
      Nome: dados.nome,
      Sobrenome: dados.sobrenome,
      DataDeNascimento: dados.dataDeNascimento,
      Genero: dados.genero,
      Email: dados.email
    };

    // Adiciona Convenio - SEMPRE como número
    if (dados.convenio) {
      let convenioId: number | undefined;

      if (typeof dados.convenio === 'object' && dados.convenio !== null && 'id' in dados.convenio) {
        // Se for um objeto com uma propriedade 'id' (ex: {id: 1, nome: 'Convenio A'})
        convenioId = (dados.convenio as Convenio).id;
      } else {
        // Se for uma string ou número representando o ID
        const convenioStr = String(dados.convenio).trim();
        if (convenioStr !== '' && convenioStr !== '0') {
          const parsedId = parseInt(convenioStr, 10);
          if (!isNaN(parsedId) && parsedId > 0) {
            convenioId = parsedId;
          }
        }
      }

      if (convenioId !== undefined) {
        dto.Convenio = convenioId;
        console.log('Convenio adicionado ao DTO:', dto.Convenio, '(tipo:', typeof dto.Convenio, ')');
      } else {
        console.log('Não foi possível determinar o ID do convênio a partir de:', dados.convenio);
      }
    } else {
      console.log('dados.convenio está vazio/undefined, NÃO será enviado');
    }

    // Adiciona Celular e TelefoneFixo, permitindo strings vazias para que a API possa validar.
    dto.Celular = dados.celular;
    dto.TelefoneFixo = dados.telefoneFixo;

    // A API .NET espera o DTO dentro de um objeto "dto"
    const body = { dto };

    console.log('=== Body FINAL enviado para API (update) ===');
    console.log(JSON.stringify(body, null, 2));
    console.log('============================================');

    return this.http.patch<Entidade>(`${this.pacienteApiUrl}/${key}`, body);
  }

  delete(key: string): Observable<any> {
    return this.http.delete<any>(`${this.pacienteApiUrl}/${key}`);
  }

  getAllConvenios(): Observable<Convenio[]> {
    return this.http.get<Convenio[]>(this.convenioApiUrl);
  }
}
