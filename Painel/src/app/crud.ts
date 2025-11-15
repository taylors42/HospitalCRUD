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
      NumeroCarteirinha: dados.numeroCarteirinha,
      ValidadeCarteirinha: dados.validadeCarteirinha,
      ...(dados.cpf && { CPF: dados.cpf }),
      ...(dados.celular && { Celular: dados.celular }),
      ...(dados.telefoneFixo && { TelefoneFixo: dados.telefoneFixo })
    };

    return this.http.post<Entidade>(this.pacienteApiUrl, dto);
  }

  getAll(): Observable<Entidade[]> {
    return this.http.get<Entidade[]>(this.pacienteApiUrl);
  }

  update(key: string, dados: UpdatePacienteDTO): Observable<Entidade> {
    const dto: any = {
      Nome: dados.nome,
      Sobrenome: dados.sobrenome,
      DataDeNascimento: dados.dataDeNascimento,
      Genero: dados.genero,
      Email: dados.email
    };

    if (dados.convenio) {
      let convenioId: number | undefined;

      if (typeof dados.convenio === 'object' && dados.convenio !== null && 'id' in dados.convenio) {
        convenioId = (dados.convenio as Convenio).id;
      } else {
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
      }
    }

    dto.Celular = dados.celular;
    dto.TelefoneFixo = dados.telefoneFixo;

    if (dados.numeroCarteirinha) {
      dto.NumeroCarteirinha = dados.numeroCarteirinha;
    }
    if (dados.validadeCarteirinha) {
      dto.ValidadeCarteirinha = dados.validadeCarteirinha;
    }

    const body = { dto };

    return this.http.patch<Entidade>(`${this.pacienteApiUrl}/${key}`, body);
  }

  delete(key: string): Observable<any> {
    return this.http.delete<any>(`${this.pacienteApiUrl}/${key}`);
  }

  getAllConvenios(): Observable<Convenio[]> {
    return this.http.get<Convenio[]>(this.convenioApiUrl);
  }
}
