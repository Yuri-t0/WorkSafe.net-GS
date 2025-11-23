# 🛡️ WorkSafe .NET API – Advanced Business Development with .NET

A **WorkSafe .NET API** é uma aplicação Web API desenvolvida em **ASP.NET Core** com **EF Core** e **SQL Server**, voltada para o **cadastro e gestão de estações de trabalho** (workstations) e informações ergonômicas básicas.

O objetivo do projeto é demonstrar, de forma organizada e orientada a camadas:

- Modelagem de domínio e regras de negócio
- Serviços de aplicação claros (casos de uso)
- Persistência com EF Core (mapeamentos + migrations)
- Exposição de uma Web API REST com validações, ProblemDetails e Swagger

---

## 🧩 1. Domínio & Arquitetura

### Entidade principal

A entidade principal do domínio é:

- **Workstation**
  - `Id` (int) – chave primária, gerada pelo banco
  - `Name` (string) – nome da estação de trabalho (obrigatório)
  - `EmployeeName` (string) – colaborador associado (obrigatório)
  - `Department` (string) – departamento/setor (obrigatório)
  - `MonitorDistanceCm` (int) – distância do monitor em centímetros  
    - Regra de negócio: **deve estar entre 30 e 100 cm**
  - Outros campos/flags podem ser adicionados conforme a evolução do domínio.

### Invariantes e regras de negócio

As principais regras de negócio garantidas via validação de modelo e anotações:

- **Campos obrigatórios**
  - `Name`, `EmployeeName` e `Department` são obrigatórios.
- **Regra ergonômica**
  - `MonitorDistanceCm` deve estar entre **30 e 100**. Valores fora dessa faixa geram erro de validação (400).

Essas invariantes são aplicadas na entidade/DTOs e reforçadas na camada de aplicação.

### Arquitetura em camadas

O projeto está organizado nas seguintes pastas:

- **Domain/**
  - Entidades de domínio (`Workstation`) e regras centrais.
- **Application/**
  - **Services** de aplicação para orquestrar os casos de uso (CRUD, busca, etc.).
  - **DTOs / ViewModels** para entrada e saída de dados.
- **Infrastructure/**
  - Configuração do **EF Core**, `DbContext` e **repositórios concretos**.
  - Mapeamentos de entidades e migrations.
- **Controllers/**
  - Camada Web API, que expõe os endpoints REST.
- **Migrations/**
  - Histórico das migrações do EF Core para criação/alteração do banco.

---

## ⚙️ 2. Aplicação (Serviços e DTOs)

A camada **Application** concentra a lógica de aplicação, separando o domínio da Web API:

- **Serviços de aplicação (`WorkstationService`, etc.)**
  - `CreateAsync` – cria uma nova workstation a partir de um DTO de entrada.
  - `UpdateAsync` – atualiza dados de uma workstation existente.
  - `DeleteAsync` – remove uma workstation por Id.
  - `GetByIdAsync` – busca por Id.
  - **Opcional / recomendável:** métodos de **busca paginada com filtro** (ex.: por departamento, por nome, etc.).

- **DTOs / ViewModels**
  - DTOs de **entrada** para criação/edição (sem expor detalhes internos do domínio).
  - DTOs de **saída** com os dados formatados para a API.

Essa separação garante que a Web API não dependa diretamente das entidades de domínio e facilita evolução e testes.

---

## ❗ 3. Tratamento de erros & validações

A API utiliza o pipeline padrão do ASP.NET Core com validação de modelos:

- Quando o corpo da requisição envia dados inválidos (campos obrigatórios vazios, ranges inválidos, etc.), o framework retorna:
  - **HTTP 400 (Bad Request)** com um objeto no formato **`ProblemDetails`**, contendo:
    - `title` – mensagem amigável (ex.: `"Dados inválidos enviados."`)
    - `status` – código HTTP
    - `errors` – dicionário com os campos e mensagens (ex.: `"MonitorDistanceCm deve estar entre 30 e 100."`)

- Erros não tratados são mapeados para **HTTP 500 (Internal Server Error)** com uma mensagem genérica:
  - `"Erro interno no servidor. Tente novamente mais tarde."`

> Isso atende ao requisito de **validações + ProblemDetails** e evita vazar detalhes de implementação para o cliente.

---

## 🗄️ 4. Infra & Dados (EF Core)

### Banco de dados

- Banco: **SQL Server**
- Instância padrão utilizada: `localhost` (MSSQLSERVER padrão)
- Nome do banco: **`WorkSafeDb`**

A connection string pode ser ajustada em:

```json
// appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=WorkSafeDb;Trusted_Connection=True;TrustServerCertificate=True"
}
Se estiver usando outra instância ou usuário/senha, ajuste aqui conforme sua máquina.

EF Core & Repositórios
AppDbContext configurado na camada Infrastructure, expondo o DbSet<Workstation>.

Mapeamentos via OnModelCreating e/ou EntityTypeConfiguration.

Migrations criadas via dotnet ef migrations add.

CRUD implementado via serviços + repositórios concretos na Infrastructure.

🌐 5. Camada Web (Web API)
A API segue o padrão de controllers com boas práticas REST.

Principais endpoints
Prefixo base (exemplo): /api/workstations
Substitua a porta pela que aparecer no console ao rodar a API.

GET /api/workstations
Retorna a lista de workstations (pode ser paginada/filtrada).

GET /api/workstations/{id}
Busca uma workstation por Id.

POST /api/workstations
Cria uma nova workstation.

Exemplo de corpo:

json
Copy code
{
  "name": "Estação Financeiro 01",
  "employeeName": "João Silva",
  "department": "Financeiro",
  "monitorDistanceCm": 60
}
PUT /api/workstations/{id}
Atualiza uma workstation existente.

DELETE /api/workstations/{id}
Remove uma workstation.

Endpoint de busca (search) com filtros e paginação (recomendado)
Opcionalmente (e recomendado pelo enunciado), pode existir algo como:

GET /api/workstations/search?department=Financeiro&page=1&pageSize=10&orderBy=name

Retornando um objeto paginado com:

itens da página

total de registros

informações de próxima/anterior página

links HATEOAS (self, next, previous).

📚 6. Swagger / Documentação da API
Ao rodar o projeto no perfil de desenvolvimento, o Swagger UI é habilitado automaticamente.

URL típica (ajuste a porta conforme seu ambiente):

https://localhost:7043/swagger

ou

http://localhost:5043/swagger

No Swagger você consegue:

Ver a lista de endpoints

Ver os modelos (schemas)

Executar requisições de teste (GET/POST/PUT/DELETE)

Validar respostas e códigos HTTP

🏃 7. Como rodar o projeto localmente
Pré-requisitos
.NET SDK 8.0+

SQL Server (instância local MSSQLSERVER ou outra de sua preferência)

Git

