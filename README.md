# Sistema de Cadastro de Pacientes

Sistema completo de cadastro e gerenciamento de pacientes hospitalares, desenvolvido com ASP.NET Core e Angular 19.

## Visão Geral

Este projeto implementa um sistema CRUD (Create, Read, Update, Delete) para gerenciamento de pacientes hospitalares, incluindo validações completas, máscaras de entrada, e integração com sistema de convênios médicos.

## Tecnologias Utilizadas

### Backend
- **.NET 8.0** (ASP.NET Core)
- **Entity Framework Core** (InMemory Database)
- **RESTful API**

### Frontend
- **Angular 19** (Standalone Components)
- **TypeScript**
- **RxJS**
- **CSS3** (Design minimalista e responsivo)

## Funcionalidades

- Cadastro completo de pacientes com validações
- Edição de dados cadastrais
- Listagem de pacientes ativos
- Exclusão lógica de pacientes
- Gerenciamento de convênios médicos
- Geração automática de número de carteirinha
- Validação de CPF (opcional mas validado se preenchido)
- Máscaras automáticas para CPF, RG, telefones
- Interface responsiva e acessível

## Estrutura do Projeto

```
CadastroPacientes/
├── PacientesAPI/              # Backend ASP.NET Core
│   └── Controllers/           # Controllers da API
├── Painel/                    # Frontend Angular 19
│   └── src/app/
│       ├── components/        # Componentes Angular
│       ├── services/          # Serviços HTTP
│       ├── models/            # Interfaces TypeScript
│       └── utils/             # Utilitários (máscaras)
├── Models/                    # Modelos de dados
│   ├── Paciente.cs
│   └── Convenio.cs
└── BancoDeDados/              # Scripts SQL
```

## Pré-requisitos

- .NET 8.0 SDK ou superior
- Node.js 18+ e npm
- Visual Studio 2022 ou Visual Studio Code (opcional)

## Como Executar

### 1. Backend (API)

```bash
cd PacientesAPI
dotnet run
```

A API estará disponível em: `http://localhost:5000`

### 2. Frontend (Angular)

```bash
cd Painel
npm install
npm start
```

O painel estará disponível em: `http://localhost:4200`

## Endpoints da API

- `GET /Paciente` - Lista todos os pacientes ativos
- `POST /Paciente` - Cria novo paciente
- `PATCH /Paciente/{key}` - Atualiza paciente
- `DELETE /Paciente/{key}` - Exclui paciente (exclusão lógica)
- `GET /Convenio` - Lista todos os convênios

## Validações Implementadas

### Backend
- Validação de CPF válido e único
- Validação de RG
- Validação de email válido
- Validação de pelo menos um telefone (celular ou fixo)
- Validação de data de nascimento não futura
- Prevenção de cadastros duplicados por CPF

### Frontend
- Validações em tempo real
- Feedback visual de erros
- Máscaras automáticas para formatação
- Campos obrigatórios claramente indicados

## Design

O sistema segue princípios de design minimalista:
- Paleta de cores neutra e profissional
- Tipografia limpa e legível
- Interface responsiva para todos os dispositivos
- Feedback visual claro para todas as ações

## Banco de Dados

**Importante**: O sistema atualmente usa banco de dados em memória (InMemory Database) para fins de demonstração. Os dados são perdidos quando a aplicação é reiniciada.

Para uso em produção, configure SQL Server conforme especificação do projeto.

## Documentação Adicional

- [Instruções Detalhadas de Execução](INSTRUCOES_EXECUCAO.md)
- [Documentação do Frontend](FRONTEND_README.md)

## Conformidade com Requisitos

- Todos os campos obrigatórios implementados
- Geração automática de ID e carteirinha
- Seleção de gênero (Masculino, Feminino, Outro)
- CPF opcional mas validado
- Estados brasileiros (UF) para RG
- Integração com convênios médicos
- Exclusão lógica com campo DataDeExclusao
- Máscaras em campos de entrada
- Interface responsiva e usável

## Melhorias Futuras

- Migração para SQL Server
- Implementação de autenticação e autorização
- Paginação na listagem de pacientes
- Filtros e busca avançada
- Testes unitários e de integração
- Dockerização da aplicação
- Documentação Swagger da API

## Autor

Desenvolvido como parte de um desafio técnico de cadastro de pacientes.

## Licença

Este projeto é fornecido como está, para fins de avaliação técnica.
