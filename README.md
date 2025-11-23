🛡️ WorkSafe .NET API — Advanced Business Development with .NET

A WorkSafe .NET API é uma aplicação Web API desenvolvida com ASP.NET Core, Entity Framework Core e SQL Server, construída para demonstrar uma arquitetura limpa, validações robustas e um CRUD completo para gerenciamento de estações de trabalho (Workstations).

Este projeto faz parte da entrega da disciplina Advanced Business Development with .NET.

✔️ Sumário

Arquitetura do Projeto

Domínio & Regras de Negócio

Aplicação (Serviços & DTOs)

Infraestrutura & Dados (EF Core)

Camada Web API (CRUD + Search)

Tratamento de Erros (ProblemDetails)

Como Executar o Projeto

Endpoints Principais

Exemplos de Uso — CURL

Decisões Arquiteturais

🧩 Arquitetura do Projeto

A solução segue uma arquitetura em camadas:

WorkSafe.Api
├── Domain/
│   └── Entidades e invariantes do negócio
├── Application/
│   ├── Services (casos de uso)
│   └── DTOs / ViewModels
├── Infrastructure/
│   ├── AppDbContext
│   ├── Repositórios (CRUD)
│   └── Migrations (EF Core)
└── Web (Controllers da API)

🏛️ Domínio & Regras de Negócio
Entidade principal: Workstation
Campo	Tipo	Regra
Id	int	Identity
Name	string	Obrigatório
EmployeeName	string	Obrigatório
Department	string	Obrigatório
MonitorDistanceCm	int	Entre 30 e 100 cm
Invariantes aplicadas:

Nome, empregado e departamento não podem ser vazios.

Distância do monitor deve estar entre 30 e 100 centímetros.

Validações automáticas via Data Annotations + ModelState.

⚙️ Aplicação (Serviços & DTOs)

A camada Application contém:

✔ Serviços de aplicação

CreateAsync

UpdateAsync

DeleteAsync

GetByIdAsync

SearchAsync com filtros e paginação (quando disponível)

✔ DTOs / ViewModels

Separação clara entre:

Entrada: WorkstationRequestDTO

Saída: WorkstationResponseDTO

Isso evita expor entidades do domínio diretamente.

🗄️ Infraestrutura & Dados (EF Core)
✔ Banco utilizado

SQL Server

Instância usada: localhost (MSSQLSERVER)

Banco: WorkSafeDb

✔ Connection String (padrão)
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=WorkSafeDb;Trusted_Connection=True;TrustServerCertificate=True"
}

✔ EF Core

Mapeamento via Fluent API / anotações

DbSet<Workstation>

Repositórios concretos contendo CRUD

Histórico completo em Migrations/

🌐 Camada Web API (CRUD + Search)
✔ Endpoints REST com boas práticas

CRUD completo

Rota base: /api/workstations

Respostas com códigos HTTP adequados

Problemas de validação ⇒ HTTP 400

Erros internos ⇒ HTTP 500

✔ Busca com filtros (quando implementada)
GET /api/workstations/search?department=Financeiro&page=1&pageSize=10

❗ Tratamento de Erros (ProblemDetails)

Validações utilizam ModelState, retornando erro padrão:

{
  "title": "Dados inválidos enviados.",
  "status": 400,
  "errors": {
    "Department": [ "The Department field is required." ]
  }
}


Erros inesperados retornam:

{
  "title": "Erro interno no servidor.",
  "status": 500,
  "detail": "Tente novamente mais tarde."
}

🚀 Como Executar o Projeto
1️⃣ Clonar o repositório
git clone https://github.com/Yuri-t0/WorkSafe.net-GS.git
cd WorkSafe.net-GS

2️⃣ Ajustar a connection string (se necessário)

Arquivo: appsettings.json

3️⃣ Aplicar as migrations
dotnet ef database update

4️⃣ Rodar a API
dotnet run

5️⃣ Acessar o Swagger
https://localhost:{PORT}/swagger

📌 Endpoints Principais
GET — listar todas
GET /api/workstations

GET — buscar por id
GET /api/workstations/{id}

POST — criar uma workstation
POST /api/workstations
{
  "name": "Estação A",
  "employeeName": "João Silva",
  "department": "Financeiro",
  "monitorDistanceCm": 60
}

PUT — atualizar
PUT /api/workstations/{id}

DELETE — remover
DELETE /api/workstations/{id}

💻 Exemplos de Uso — cURL
Criar workstation
curl -X POST "https://localhost:{PORT}/api/workstations" ^
  -H "Content-Type: application/json" ^
  -d "{
    \"name\": \"Estação Financeiro 01\",
    \"employeeName\": \"João Silva\",
    \"department\": \"Financeiro\",
    \"monitorDistanceCm\": 60
  }"

Listar
curl "https://localhost:{PORT}/api/workstations"

Buscar por id
curl "https://localhost:{PORT}/api/workstations/1"

🧠 Decisões Arquiteturais

Separação clara Domain → Application → Infrastructure → Web API

DTOs para evitar vazamento de domínio

EF Core para persistência

Migrations versionando o banco

ProblemDetails padronizando erros

Swagger para documentação

Clean Architecture simplificada

SQL Server por compatibilidade com .NET

📦 Entrega Final Atende:

✔ Domínio & invariantes
✔ Casos de uso (serviços)
✔ DTOs + validação + ProblemDetails
✔ EF Core + Migrations
✔ CRUD + Search (quando implementado)
✔ Swagger
✔ README completo
✔ Comandos de instalação
✔ Exemplos cURL
✔ Arquitetura explicada


Yuri Ferreira
RM: 559223

João Santana
RM: 560781
