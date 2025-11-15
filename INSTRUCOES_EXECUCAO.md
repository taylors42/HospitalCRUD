# Guia de Instalação e Execução - Cadastro de Pacientes

Este documento detalha os passos necessários para configurar e executar a aplicação completa (Backend e Frontend) em um ambiente de desenvolvimento local.

## 1. Pré-requisitos

Certifique-se de que você tem os seguintes softwares instalados e configurados em sua máquina:

- **.NET 9 SDK:** Necessário para executar o backend.
  - Verifique a instalação com: `dotnet --version`
- **Node.js 18+ e npm:** Necessário para executar o frontend.
  - Verifique a instalação com: `node --version` e `npm --version`
- **SQL Server:** (Express, Developer, ou outra edição) O banco de dados para persistência dos dados.
- **.NET EF Core Tools:** Ferramenta de linha de comando para gerenciar as migrations do Entity Framework.
  - Instale ou atualize com: `dotnet tool install --global dotnet-ef`

## 2. Configuração do Banco de Dados

O projeto utiliza o SQL Server para persistência de dados.

### Passo 2.1: Criar o Banco de Dados

- Abra seu gerenciador de SQL Server (como o SQL Server Management Studio - SSMS).
- Crie um novo banco de dados vazio. Você pode nomeá-lo, por exemplo, `Hospital`.

### Passo 2.2: Configurar a Connection String

- Abra o arquivo de configuração do backend em: `PacientesAPI/appsettings.json`.
- Localize a seção `ConnectionStrings` e edite a `DefaultConnection` para apontar para o seu servidor e banco de dados recém-criado.

**Exemplo:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR;Database=Hospital;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```
- **Substitua `SEU_SERVIDOR`** pelo nome da sua instância do SQL Server (ex: `localhost`, `SQLEXPRESS`, etc.).
- **Substitua `Hospital`** se você usou um nome diferente para o banco de dados.
- Ajuste as credenciais de conexão (`Trusted_Connection` ou `User Id`/`Password`) conforme a configuração do seu SQL Server.

## 3. Aplicação das Migrations

As migrations do Entity Framework Core são usadas para criar o esquema do banco de dados (tabelas) e popular os dados iniciais (convênios).

- Abra um terminal ou prompt de comando.
- Navegue até a pasta do projeto de banco de dados:
  ```bash
  cd BancoDeDados
  ```
- Execute o seguinte comando para aplicar as migrations:
  ```bash
  dotnet ef database update
  ```
- Ao final do processo, as tabelas `Pacientes` e `Convenios` terão sido criadas no seu banco de dados, e a tabela `Convenios` será populada com 5 registros iniciais.

## 4. Execução do Projeto

Com o banco de dados configurado, você pode iniciar o backend e o frontend. **É recomendado iniciar o backend primeiro.**

### Passo 4.1: Executar o Backend (API)

- Em um novo terminal, navegue até a pasta da API:
  ```bash
  cd PacientesAPI
  ```
- Inicie a aplicação:
  ```bash
  dotnet run
  ```
- A API estará em execução e acessível em `http://localhost:3000`.
- A documentação interativa da API (Swagger) estará disponível em: `http://localhost:3000/swagger`.

### Passo 4.2: Executar o Frontend (Painel Angular)

- Em **outro** terminal, navegue até a pasta do painel:
  ```bash
  cd Painel
  ```
- Instale as dependências (apenas na primeira vez):
  ```bash
  npm install
  ```
- Inicie a aplicação de frontend:
  ```bash
  npm start
  ```
- A aplicação estará acessível no seu navegador em: `http://localhost:4200`.

## 5. Resumo da Ordem de Operações

1.  **Configurar o Ambiente:** Instalar todos os pré-requisitos.
2.  **Configurar o Banco:** Criar o banco de dados no SQL Server e ajustar a `appsettings.json`.
3.  **Aplicar Migrations:** Navegar para a pasta `BancoDeDados` e rodar `dotnet ef database update`.
4.  **Iniciar Backend:** Navegar para a pasta `PacientesAPI` e rodar `dotnet run`.
5.  **Iniciar Frontend:** Navegar para a pasta `Painel` e rodar `npm install` e `npm start`.
6.  **Acessar a Aplicação:** Abrir `http://localhost:4200` no navegador.

Com estes passos, o ambiente de desenvolvimento estará totalmente configurado e a aplicação pronta para uso.
