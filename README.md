# HotelWise API

Backend ASP.NET Core da plataforma **HotelWise**: gestão de hotéis, quartos, disponibilidade e reservas, com busca semântica e assistente de IA (chatbot).

Solução: `HotelWiseAPI.sln` · Target: **.NET 10** · Persistência: **MySQL** (EF Core 9 + Pomelo 9)

---

## Sobre

Projeto piloto que integra domínio hoteleiro com IA:

- Cadastro e consulta de hotéis, rooms, availabilities e reservas
- Busca humanizada via **Semantic Kernel** + vector store (**Qdrant** / InMemory)
- Assistente conversacional (Mistral, Ollama e outros providers configuráveis)
- Autenticação JWT local e suporte a **Microsoft Entra ID** (`Microsoft.Identity.Web`)
- API documentada com **Swagger / OpenAPI**

Frontend companion: [HotelWiseUI](https://github.com/LeoneRocha/HotelWiseUI)

---

## Funcionalidades

| Área | Capacidade |
| ---- | ---------- |
| **Hotéis** | CRUD, geração assistida por IA (`GET .../Hotels/v1/generate`), indexação em vector store |
| **Quartos** | CRUD por hotel, tipos/status, noites mínimas |
| **Disponibilidade** | Disponibilidades e preços por quarto (incl. operações em lote) |
| **Reservas** | Criação e gestão de reservas |
| **Auth** | Login JWT (`admin` / `admin123` no seed), roles Admin |
| **Assistente IA** | Chat / RAG via Semantic Kernel + Kernel Memory |
| **Busca semântica** | Embeddings + Qdrant (ou InMemory) para ranking de hotéis |
| **Observabilidade** | Serilog (Console/File), Correlation ID (`X-Correlation-ID`), exception middleware, `GET /health`, Application Insights opcional |
| **Ops** | Migrations EF no startup, Swagger, CI Azure DevOps, análise SonarCloud |

### Endpoints principais (`/api/.../v1`)

| Controller | Rota base | Exemplos |
| ---------- | --------- | -------- |
| Auth | `api/Auth/v1` | Authenticate |
| Hotels | `api/Hotels/v1` | CRUD, tags, generate, semanticsearch, addvector |
| Rooms | `api/Rooms/v1` | CRUD, listagem por hotel |
| RoomAvailability | `api/RoomAvailability/v1` | availabilities / batch |
| Reservations | `api/Reservations/v1` | CRUD, cancel, por room |
| Assistant | `api/Assistant/v1` | `POST ask` |
| App info | `api/AppInformationVersionProduct/v1` | Versão do produto |
| Health | `/health` | Liveness |

Swagger (raiz redireciona): `/swagger`

---

## Arquitetura da solução

```text
HotelWiseAPI.sln
├── HotelWise.API          # Host Web API, DI, Swagger, middleware, health
├── HotelWise.Service      # Casos de uso, AI adapters, AutoMapper wiring
├── HotelWise.Data         # EF Core, Pomelo MySQL, migrations, seeds
├── HotelWise.Domain       # Entidades, DTOs, interfaces, Semantic Kernel adapters
├── GroqApiLibrary         # Cliente Groq (packable: net8.0;net10.0)
└── HotelWise.ConsolePOC   # POC Ollama
```

**CPM:** versões NuGet centralizadas em [`Directory.Packages.props`](Directory.Packages.props)  
**SDK:** pin em [`global.json`](global.json) (`10.0.301`, `rollForward: latestFeature`)

---

## Stack tecnológica

### Plataforma
- C# / **.NET 10** (ASP.NET Core 10.0.x)
- Swashbuckle (OpenAPI 2.x / Swagger UI)
- Serilog + enrichers (Environment / MachineName)
- Application Insights (se `ApplicationInsights:ConnectionString` ou `APPLICATIONINSIGHTS_CONNECTION_STRING`)

### Persistência
- **Entity Framework Core 9.0.18**
- **Pomelo.EntityFrameworkCore.MySql 9.0.0** (última estável oficial; EF 10 aguarda Pomelo 10)
- MySQL 8.x
- Seeds: User `admin`, Hotel Example, Room de exemplo

### Segurança
- JWT Bearer
- Microsoft.Identity.Web (Entra ID)
- FluentValidation

### IA & busca
- Microsoft Semantic Kernel 1.78
- Kernel Memory
- Conectores: Mistral AI, Ollama, Qdrant, InMemory
- OllamaSharp / Mistral.SDK
- Microsoft.Extensions.AI / VectorData

### Azure & utilitários
- Azure Identity, Blob, Queue, Tables
- Microsoft Graph
- AutoMapper, Polly, Bogus
- Documentos: OpenXml, PDFsharp, QuestPDF, Markdig, HtmlAgilityPack

### DevOps & qualidade
- Azure DevOps Pipelines
- SonarCloud
- GitHub
- Docker (`QdrantDockerFile/`, stacks locais em `IA_Local/`)

---

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download) (recomendado 10.0.301+)
- MySQL 8.x acessível (`ConnectionStrings:DBConnectionMySQL`)
- (Opcional) [Qdrant](https://qdrant.tech/) se `VectorStoreType=Qdrant`
- (Opcional) Ollama / chaves Mistral / Groq conforme `ApplicationIAConfig` em `appsettings`

---

## Como executar

```powershell
cd HotelWiseAPI
dotnet restore HotelWiseAPI.sln
dotnet build HotelWiseAPI.sln -c Release
dotnet run --project HotelWise.API/HotelWise.API.csproj
```

Ambiente Development usa `appsettings.Development.json`.  
No startup a API aplica migrations pendentes (`Database.Migrate()`).

### Credenciais seed (dev)

| Login | Senha | Role |
| ----- | ----- | ---- |
| `admin` | `admin123` | Admin |

### Health

```text
GET /health
```

---

## Entity Framework (MySQL)

Contexto: `HotelWiseDbContextMysql`  
Projeto: `HotelWise.Data` · Startup: `HotelWise.API`

```powershell
cd HotelWiseAPI
$env:ASPNETCORE_ENVIRONMENT = "Development"

dotnet ef migrations list `
  --project HotelWise.Data/HotelWise.Data.csproj `
  --startup-project HotelWise.API/HotelWise.API.csproj `
  --context HotelWiseDbContextMysql

dotnet ef migrations add <Nome> `
  --project HotelWise.Data/HotelWise.Data.csproj `
  --startup-project HotelWise.API/HotelWise.API.csproj `
  --context HotelWiseDbContextMysql `
  --output-dir Migrations/MySql

dotnet ef database update `
  --project HotelWise.Data/HotelWise.Data.csproj `
  --startup-project HotelWise.API/HotelWise.API.csproj `
  --context HotelWiseDbContextMysql
```

---

## Observabilidade

- Header **`X-Correlation-ID`**: aceito na entrada e devolvido na resposta; propagado nos logs
- **Serilog request logging** + middleware de request (sem logar Authorization)
- **Global exception middleware**: stack no log; JSON com `correlationId` / `traceId`
- **Application Insights**: ativo somente com connection string configurada

Documentação da migração .NET 10: pasta [`DOCUMENTACAO/`](DOCUMENTACAO/)

---

## Links

| Recurso | URL |
| ------- | --- |
| API (Swagger) | https://hotelwiseapi.azurewebsites.net/swagger/index.html |
| Frontend | https://hotelwiseui.azurewebsites.net/ |
| Repo API | https://github.com/LeoneRocha/HotelWiseAPI |
| Repo UI | https://github.com/LeoneRocha/HotelWiseUI |
| Azure DevOps | https://lionscorp.visualstudio.com/VariousStudies/_build |
| SonarCloud API | https://sonarcloud.io/summary/new_code?id=lionscorp_hotelwiseapi&branch=master |
| SonarCloud UI | https://sonarcloud.io/summary/new_code?id=lionscorp_hotelwiseui&branch=master |

---

## Pack (GroqApiLibrary)

```powershell
dotnet pack GroqApiLibrary/GroqApiLibrary.csproj -c Release -o ./artifacts/nupkg
```

O pacote inclui `lib/net8.0` e `lib/net10.0`.

---

## Notas

- Frontend (React / Vite / TypeScript) vive no repositório **HotelWiseUI**, não nesta solução.
- Não há suíte de testes automatizados na API neste ciclo (gap conhecido); validação via smoke + Swagger.
- Bloco EF permanece em **9.x** até existir Pomelo 10 oficial no NuGet.
