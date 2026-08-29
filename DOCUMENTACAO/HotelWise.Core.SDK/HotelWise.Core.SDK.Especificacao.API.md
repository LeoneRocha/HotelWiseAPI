# Especificação Técnica — HotelWise.Core.SDK (Módulo API)

**Versão:** 1.0.0  
**Data:** 2026-08-28  
**Projeto de Origem:** `HotelWise.API` (`HotelWise.API.csproj`)  
**Projeto de Destino:** `HotelWise.Core.SDK`  
**Documento Principal:** [HotelWise.Core.SDK.Levantamento.md](./HotelWise.Core.SDK.Levantamento.md)

---

## 1. Papel do Módulo na Arquitetura

O projeto `HotelWise.API` é o aplicativo host ASP.NET Core (`Microsoft.NET.Sdk.Web`) executando no framework `.NET 10`. Ele atua primariamente como a camada de apresentação HTTP, orquestrando a inicialização do servidor web, a configuração do pipeline de middlewares, a documentação Swagger/OpenAPI, a autenticação JWT Bearer e a exposição dos endpoints REST da aplicação.

Na migração para o `HotelWise.Core.SDK`, o projeto `HotelWise.API` é **fundamentalmente um consumidor**. Ele não cede controladores de negócio para o Core, mas passa a consumir diretamente os tipos canônicos de DTOs, helpers, segurança, middlewares e contratos agora centralizados no SDK.

---

## 2. Inventário de Componentes de `HotelWise.API`

### 2.1 Controladores da API (Todos Permanecem no Host)

| Controlador | Rota | Responsabilidade | Situação |
| :--- | :--- | :--- | :--- |
| `HotelsController` | `api/hotels/v1` | CRUD de hotéis, indexação vetorial e busca semântica. | **Manter no Host** |
| `RoomsController` | `api/rooms/v1` | CRUD e consulta de quartos e capacidades. | **Manter no Host** |
| `ReservationsController` | `api/reservations/v1` | Operações de reserva de hóspedes. | **Manter no Host** |
| `RoomAvailabilityController` | `api/roomavailability/v1` | Consulta e atualização de disponibilidade e diárias. | **Manter no Host** |
| `AuthController` | `api/auth/v1` | Login, autenticação e emissão de tokens JWT. | **Manter no Host** |
| `AssistantController` | `api/assistant/v1` | Interação conversacional com o agente StayMate. | **Manter no Host** |
| `AppInformationVersionProductController` | `api/appinformationversionproduct/v1` | Exposição de metadados de versão da API. | **Manter no Host** (Ajuste de namespace) |

> **Ajuste de Namespace Legado:** O arquivo `AppInformationVersionProductController.cs` possuía historicamente o namespace `SmartDigitalPsico.WebAPI.Controllers.v1.SystemDomains`. Ele será corrigido para `HotelWise.API.Controllers`.

---

### 2.2 Estruturas de Inicialização e Configuração

| Arquivo / Classe | Caminho | Responsabilidade | Situação |
| :--- | :--- | :--- | :--- |
| `Program.cs` | `Program.cs` | Entry point da aplicação, inicialização de host e catch de falhas fatais. | **Manter no Host** |
| `WebApplicationConfigureBuilder` | `Configure/WebApplicationConfigureBuilder.cs` | Construção de pipeline, middlewares e auto-migration EF. | **Manter no Host** |
| `WebApplicationConfigureServiceCollections` | `Configure/WebApplicationConfigureServiceCollections.cs` | Configuração de CORS, HealthChecks, AppInsights, Swagger e MVC. | **Manter no Host** |
| `ServiceCollectionAddAllDependencies` | `Configure/ServiceCollectionAddAllDependencies.cs` | Registro de pool de DbContext MySQL e injeções de dependência. | **Manter no Host** |
| `ServiceCollectionConfigureSecurity` | `Configure/ServiceCollectionConfigureSecurity.cs` | Configuração de autenticação JWT Bearer e Azure AD. | **Manter no Host** |

---

## 3. Integração e Consumo do `HotelWise.Core.SDK`

### 3.1 Atualização de Referência no Projeto

O arquivo `HotelWise.API.csproj` receberá a referência de projeto para o SDK canônico:

```xml
<ItemGroup>
  <ProjectReference Include="..\HotelWise.Core.SDK\HotelWise.Core.SDK.csproj" />
  <ProjectReference Include="..\HotelWise.Service\HotelWise.Service.csproj" />
</ItemGroup>
```

### 3.2 Atualização de Namespaces e `using`s nos Controladores

Os controladores passam a importar os tipos de resposta (`ServiceResponse`), utilitários de segurança (`SecurityHelperApi`) e DTOs base diretamente do `HotelWise.Core.SDK`:

```csharp
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Security;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers.HotelEndpoints
{
    [Authorize("Bearer")]
    [ApiController]
    [Route("api/[controller]/v1")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly IHotelSearchService _hotelSearchService;

        public HotelsController(IHotelService hotelService, IHotelSearchService hotelSearchService)
        {
            _hotelService = hotelService;
            _hotelSearchService = hotelSearchService;
        }

        private void SetUserIdCurrent()
        {
            long idUser = SecurityHelperApi.GetUserIdApi(User);
            _hotelService.SetUserId(idUser);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<HotelDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            SetUserIdCurrent();
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse<HotelDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] HotelDto hotel)
        {
            SetUserIdCurrent();
            var response = await _hotelService.AddHotelAsync(hotel);
            return Ok(response);
        }
    }
}
```

---

### 3.3 Integração de Middlewares Canônicos

O pipeline HTTP configurado em `WebApplicationConfigureBuilder.Configure` consome os middlewares canônicos migrados para o `HotelWise.Core.SDK.Infrastructure.Middleware`:

```csharp
using HotelWise.Core.SDK.Infrastructure.Middleware;
using HotelWise.Core.SDK.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace HotelWise.API.Configure
{
    public static class WebApplicationConfigureBuilder
    {
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            // Middleware canônico de CorrelationId
            app.UseMiddleware<CorrelationIdMiddleware>();

            // Logging estruturado Serilog
            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    var correlationId = httpContext.Items[CorrelationIdMiddleware.ItemKey]?.ToString()
                        ?? httpContext.TraceIdentifier;
                    diagnosticContext.Set("CorrelationId", correlationId);
                };
            });

            // Middlewares canônicos de tratamento de exceções e log de requisição
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            // Demais etapas de roteamento, autenticação, swagger e endpoints...
        }
    }
}
```

---

## 4. Plano de Verificação e Smoke Testing

Após a conclusão da migração e compilação do `HotelWiseAPI.sln`, a validação do WebAPI seguirá o roteiro:

1. **Build da Solução:**
   ```bash
   dotnet build HotelWiseAPI.sln -c Release
   ```
2. **Execução de Testes Unitários e de Integração:**
   ```bash
   dotnet test HotelWise.Core.SDK.Tests/HotelWise.Core.SDK.Tests.csproj
   ```
3. **Smoke Test de Inicialização:**
   - Inicializar a API localmente ou em contêiner.
   - Validar endpoint de Health Check: `GET /health` $\rightarrow$ Status `200 OK`.
   - Validar endpoint de Versão: `GET /api/appinformationversionproduct/v1/GetAppInformationVersionProduct` $\rightarrow$ Status `200 OK`.
   - Validar UI do Swagger: `GET /swagger/index.html`.
4. **Smoke Test de Autenticação e CRUD:**
   - Efetuar login e obter token JWT: `POST /api/auth/v1/login`.
   - Listar hotéis autenticado com Bearer Token: `GET /api/hotels/v1`.

---

## 5. Checklist de Implementação

- [ ] Adicionar `ProjectReference` para `HotelWise.Core.SDK` em `HotelWise.API.csproj`.
- [ ] Atualizar `using`s em todos os controladores em `Controllers/` para os namespaces do Core.SDK.
- [ ] Atualizar `WebApplicationConfigureBuilder` para consumir middlewares do Core.SDK.
- [ ] Corrigir namespace do `AppInformationVersionProductController.cs`.
- [ ] Validar ausência de warnings de obsolescência (`HW_CORE_SDK_*`) nos controladores.
- [ ] Executar build completo da solução e realizar smoke test dos endpoints principais.
