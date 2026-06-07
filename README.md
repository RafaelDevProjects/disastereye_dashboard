# 🛰️ DisasterEye — Space Monitoring System

**Global Solution · FIAP 2025 · C# Development · Turma 3ESPR**

---

## Sobre o Projeto

O **DisasterEye** é um sistema web distribuído para gerenciamento de tecnologias espaciais aplicadas ao monitoramento de desastres naturais, integrando dados da **API EONET da NASA**.

---

## Arquitetura

```
DisasterEye/
├── DisasterEye.AppHost/          # .NET Aspire — Orquestrador
├── DisasterEye.ApiService/       # API REST (Back-End)
│   ├── Controllers/              # TecnologiasController, AuthController, CategoriasController
│   ├── Models/                   # Tecnologia, CategoriaImpacto, Usuario
│   ├── Data/                     # AppDbContext (EF Core + Oracle)
│   ├── Repositories/             # Padrão Repository com Interfaces
│   └── DTOs/                     # Data Transfer Objects
├── DisasterEye.Web/              # Front-End ASP.NET Core MVC (net8.0)
│   ├── Controllers/              # HomeController, AccountController, TecnologiasController
│   ├── Models/                   # ViewModels
│   ├── Services/                 # ApiService (HttpClient — sem acesso direto ao banco)
│   └── Views/                    # Razor Views com Bootstrap 5
├── script_banco_dados_oracle.sql # Script Oracle completo
├── StartDisasterEye.ps1          # Script orquestrador
└── equipes.txt                   # Integrantes
```

---

## Requisitos Atendidos (PDF)

| Requisito | Implementação |
|---|---|
| .NET Aspire | `DisasterEye.AppHost` orquestra API + Web |
| Oracle + EF Core | `Oracle.EntityFrameworkCore` 8.21.121 |
| Padrão Repository | `ITecnologiaRepository` / `IUsuarioRepository` + implementações |
| BCrypt | `BCrypt.Net-Next` em `UsuarioRepository.CreateAsync()` |
| `/api/tecnologias/stats` | Endpoint de estatísticas para o dashboard |
| HttpClient desacoplado | Web nunca acessa o banco diretamente |
| Dashboard Bootstrap 5 | Cards, barras de progresso, Chart.js, tabela responsiva |
| Cookie Auth + Claims | Perfil `Administrador`/`Pesquisador` injetado via Claims |
| Rotas protegidas | `[Authorize(Policy = "ApenasAdmin")]` em Create/Edit/Delete |

---

## Como Rodar

### Pré-requisitos
- .NET 8 SDK

### 1. Configurar a connection string

Edite o arquivo `DisasterEye.ApiService/appsettings.json` e substitua os dados de acesso:

```json
"ConnectionStrings": {
  "OracleConnection": "User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL;"
}
```

> O banco de dados já está criado e configurado no servidor Oracle da FIAP. Não é necessário criar tabelas ou executar scripts.

### 2. Executar o projeto

Abra **2 terminais** na pasta raiz do projeto:

**Terminal 1 — API (Back-End):**
```powershell
cd DisasterEye.ApiService
dotnet run
```
Aguarde: `Now listening on: http://localhost:5001`

**Terminal 2 — Web (Front-End):**
```powershell
cd DisasterEye.Web
dotnet run
```
Aguarde: `Now listening on: http://localhost:5002`

**Acesse:** http://localhost:5002

---

### Alternativa — Script orquestrador

Na raiz do projeto, execute o script que sobe os dois serviços automaticamente e abre o navegador:

```powershell
.\StartDisasterEye.ps1
```

---

## Credenciais Padrão

| Campo | Valor |
|---|---|
| URL | http://localhost:5002 |
| E-mail | `admin@disastereye.com` |
| Senha | `Admin@123` |
| Perfil | `Administrador` |

---

## Endpoints da API

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/tecnologias` | Lista todas |
| GET | `/api/tecnologias/{id}` | Detalhe |
| GET | `/api/tecnologias/stats` | **Dashboard stats** |
| POST | `/api/tecnologias` | Criar |
| PUT | `/api/tecnologias/{id}` | Atualizar |
| DELETE | `/api/tecnologias/{id}` | Excluir |
| GET | `/api/categorias` | Categorias |
| POST | `/api/auth/login` | Login |
| POST | `/api/auth/register` | Registro |

Swagger: `http://localhost:5001/swagger`

---

## Commits Semânticos Sugeridos

```
feat: estrutura inicial do projeto com .NET Aspire e Oracle
feat: modelos de domínio (Tecnologia, CategoriaImpacto, Usuario)
feat: AppDbContext com EF Core Oracle e seed inicial
feat: padrão Repository com interfaces (ITecnologiaRepository, IUsuarioRepository)
feat: endpoints CRUD da API de tecnologias
feat: endpoint stats para dashboard (/api/tecnologias/stats)
feat: autenticação BCrypt no UsuarioRepository
feat: autenticação por cookies com claims no Web MVC
feat: dashboard com cards, barras de progresso e gráfico Chart.js
feat: controle de acesso por perfil via Claims (Administrador/Pesquisador)
```