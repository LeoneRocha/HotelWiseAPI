# Especificação Técnica — HotelWise.Core.SDK (Módulo API)

**Versão:** 2.0.0  
**Data:** 2026-08-28  
**Projeto de Origem:** `HotelWise.API` (`HotelWise.API.csproj`, TFM `net10.0`, SDK `Microsoft.NET.Sdk.Web`)  
**Projeto de Destino:** `HotelWise.Core.SDK`  
**Documento Principal:** [HotelWise.Core.SDK.Levantamento.md](./HotelWise.Core.SDK.Levantamento.md)

---

## 1. Papel do Módulo na Arquitetura

O projeto `HotelWise.API` é o host ASP.NET Core — o ponto de entrada da aplicação. Orquestra inicialização do servidor, pipeline de middlewares, Swagger/OpenAPI, autenticação JWT Bearer e exposição dos endpoints REST.

Na migração para o Core.SDK, o `HotelWise.API` é **fundamentalmente um consumidor**. Ele não cede controllers ou classes de configuração para o Core, mas passa a consumir tipos canônicos do SDK (DTOs, helpers, segurança, middlewares).

### Dependências NuGet relevantes do .csproj

| Pacote | Observação |
| :--- | :--- |
| `Microsoft.ApplicationInsights.AspNetCore` | Telemetria |
| `Microsoft.Extensions.AI` | AI extensibility |
| `Microsoft.Graph` | Integração Microsoft Graph |
| `Microsoft.SemanticKernel`, `.Connectors.MistralAI`, `.Connectors.Ollama` | SK na camada API |
| `Serilog.AspNetCore`, `.Enrichers.Environment`, `.Sinks.*` | Logging |
| `Azure.Identity` | Autenticação Azure |

---

## 2. Inventário Completo — Arquivos .cs (excluindo /obj/)

> Total: **12 arquivos** fonte no projeto. **Todos permanecem no host.**

---

### 2.1 Controladores (7 arquivos)

| Arquivo | Tipo | Rota | Situação |
| :--- | :--- | :--- | :--- |
| `Controllers/HotelEndpoints/HotelsController.cs` | `HotelsController` | `api/hotels/v1` | **Manter** — atualizar `using`s |
| `Controllers/HotelEndpoints/RoomsController.cs` | `RoomsController` | `api/rooms/v1` | **Manter** — atualizar `using`s |
| `Controllers/HotelEndpoints/ReservationsController.cs` | `ReservationsController` | `api/reservations/v1` | **Manter** — atualizar `using`s |
| `Controllers/HotelEndpoints/RoomAvailabilityController.cs` | `RoomAvailabilityController` | `api/roomavailability/v1` | **Manter** — atualizar `using`s |
| `Controllers/AuthController.cs` | `AuthController` | `api/auth/v1` | **Manter** — atualizar `using`s |
| `Controllers/Ai/AssistantController.cs` | `AssistantController` | `api/assistant/v1` | **Manter** — atualizar `using`s |
| `Controllers/AppInformationVersionProductController.cs` | `AppInformationVersionProductController` | `api/appinformationversionproduct/v1` | **Manter** — **corrigir namespace** ¹ |

> ¹ **Bug de namespace legado confirmado no código-fonte:**
> ```csharp
> namespace SmartDigitalPsico.WebAPI.Controllers.v1.SystemDomains
> ```
> Deve ser corrigido para:
> ```csharp
> namespace HotelWise.API.Controllers
> ```
> Além disso, os `using`s atuais apontam para `HotelWise.Domain.Dto` (→ `HotelWise.Core.SDK.Common`) e `HotelWise.Domain.Helpers` (→ `HotelWise.Core.SDK.Logging`).

### 2.2 Configuração de Inicialização (4 arquivos)

| Arquivo | Tipo | Situação |
| :--- | :--- | :--- |
| `Configure/WebApplicationConfigureBuilder.cs` | `WebApplicationConfigureBuilder` | **Manter** — atualizar `using`s para Middlewares do Core |
| `Configure/WebApplicationConfigureServiceCollections.cs` | `WebApplicationConfigureServiceCollections` | **Manter** — atualizar `using`s |
| `Configure/ServiceCollectionAddAllDependencies.cs` | `ServiceCollectionAddAllDependencies` | **Manter** — atualizar `using`s |
| `Configure/ServiceCollectionConfigureSecurity.cs` | `ServiceCollectionConfigureSecurity` | **Manter** — atualizar `using`s (TokenConfigurationDto) |

### 2.3 Entry Point (1 arquivo)

| Arquivo | Tipo | Situação |
| :--- | :--- | :--- |
| `Program.cs` | — | **Manter** — sem alteração de `using`s necessária |

---

## 3. Integração e Consumo do Core.SDK

### 3.1 Atualização de Referência no Projeto

```xml
<ItemGroup>
  <ProjectReference Include="..\HotelWise.Core.SDK\HotelWise.Core.SDK.csproj" />
  <ProjectReference Include="..\HotelWise.Service\HotelWise.Service.csproj" />
</ItemGroup>
```

### 3.2 Mapa de `using`s a Atualizar

| `using` antigo | `using` novo (Core.SDK) | Arquivos afetados |
| :--- | :--- | :--- |
| `HotelWise.Domain.Dto` | `HotelWise.Core.SDK.Common` | Controllers (ServiceResponse, ErrorResponse, AppInformationVersionProductDto) |
| `HotelWise.Domain.Helpers` | `HotelWise.Core.SDK.Logging` | `AppInformationVersionProductController` (LogAppHelper) |
| `HotelWise.Domain.Helpers` | `HotelWise.Core.SDK.Security` | Controllers (SecurityHelperApi) |
| `HotelWise.Domain.CustomMiddleware` | `HotelWise.Core.SDK.Infrastructure.Middleware` | `WebApplicationConfigureBuilder` |
| `HotelWise.Domain.Dto.AppConfig` | `HotelWise.Core.SDK.Security` | `ServiceCollectionConfigureSecurity` (TokenConfigurationDto) |

### 3.3 Exemplo: Controller Atualizado

```csharp
using HotelWise.Core.SDK.Common;     // ServiceResponse<T>
using HotelWise.Core.SDK.Security;    // SecurityHelperApi
using HotelWise.Domain.Dto.Enitty;    // permanece (DTOs de domínio)
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
    }
}
```

### 3.4 Exemplo: Pipeline de Middlewares

```csharp
using HotelWise.Core.SDK.Infrastructure.Middleware;
using HotelWise.Core.SDK.Logging;

namespace HotelWise.API.Configure
{
    public static class WebApplicationConfigureBuilder
    {
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    var correlationId = httpContext.Items[CorrelationIdMiddleware.ItemKey]?.ToString()
                        ?? httpContext.TraceIdentifier;
                    diagnosticContext.Set("CorrelationId", correlationId);
                };
            });

            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            // ... roteamento, auth, swagger, endpoints
        }
    }
}
```

### 3.5 Correção do Namespace Legado

```diff
- namespace SmartDigitalPsico.WebAPI.Controllers.v1.SystemDomains
+ namespace HotelWise.API.Controllers

- using HotelWise.Domain.Dto;
- using HotelWise.Domain.Helpers;
+ using HotelWise.Core.SDK.Common;
+ using HotelWise.Core.SDK.Logging;
```

---

## 4. Plano de Verificação e Smoke Testing

### 4.1 Build da Solução
```bash
dotnet build HotelWiseAPI.sln -c Release
```

### 4.2 Testes
```bash
dotnet test HotelWise.Core.SDK.Tests/HotelWise.Core.SDK.Tests.csproj
```

### 4.3 Smoke Test de Inicialização
- **Health Check:** `GET /health` → 200 OK
- **Versão:** `GET /api/appinformationversionproduct/v1/GetAppInformationVersionProduct` → 200 OK
- **Swagger:** `GET /swagger/index.html` → carrega sem erros

### 4.4 Smoke Test de Autenticação e CRUD
- **Login:** `POST /api/auth/v1/login` → JWT token
- **Hotéis (autenticado):** `GET /api/hotels/v1` → 200 OK com Bearer

### 4.5 Validação de Namespace
```bash
# Deve retornar 0 ocorrências em arquivos .cs
grep -r "SmartDigitalPsico" --include="*.cs" HotelWise.API/
```

---

## 5. Checklist de Implementação

- [ ] Adicionar `ProjectReference` para `HotelWise.Core.SDK` em `HotelWise.API.csproj`
- [ ] Atualizar `using`s em 7 controllers (ver mapa §3.2)
- [ ] Atualizar `using`s em 4 classes de configuração
- [ ] Corrigir namespace de `AppInformationVersionProductController.cs`:  
      `SmartDigitalPsico.WebAPI.Controllers.v1.SystemDomains` → `HotelWise.API.Controllers`
- [ ] Validar ausência de warnings `HW_CORE_SDK_*` nos controllers
- [ ] Build completo da solução
- [ ] Smoke test: health, swagger, auth, CRUD
- [ ] `grep -r "SmartDigitalPsico"` = 0 ocorrências em `.cs`
