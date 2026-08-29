# Plano de Implementação — Onda 4: HotelWise.API (Consumo e Integração)

**Versão:** 1.0.0  
**Data:** 2026-08-28  
**Plano Geral:** [HotelWise.Core.SDK.PlanoImplementacao.md](./HotelWise.Core.SDK.PlanoImplementacao.md)  
**Especificação:** [HotelWise.Core.SDK.Especificacao.API.md](./HotelWise.Core.SDK.Especificacao.API.md)  
**Pré-requisito:** Ondas 1 (Domain), 2 (Data) e 3 (Service) concluídas.

---

## Resumo

| Métrica | Valor |
| :--- | :--- |
| Arquivos a portar para o Core | **0** (API é camada consumidora pura) |
| Arquivos a atualizar no host | **12** |
| Lotes sequenciais | **2** (Lote W1 e Lote W2) |
| Estimativa | **1 dia** (1 dev) |

---

## Lote W1 — Referências, Usings e Correção de Namespaces

**Dependências:** Ondas 1, 2 e 3 concluídas no Core.SDK  
**Arquivos a atualizar:** 12

### 1. Tarefas Detalhadas

| # | Ação | Arquivo Alvo | Detalhe da Modificação |
| :--- | :--- | :--- | :--- |
| 1.1 | Adicionar `ProjectReference` | `HotelWise.API/HotelWise.API.csproj` | `<ProjectReference Include="..\HotelWise.Core.SDK\HotelWise.Core.SDK.csproj" />` |
| 1.2 | Corrigir namespace legado | `Controllers/AppInformationVersionProductController.cs` | Trocar `SmartDigitalPsico.WebAPI.Controllers.v1.SystemDomains` por `HotelWise.API.Controllers` |
| 1.3 | Atualizar usings nos controllers | `Controllers/HotelEndpoints/HotelsController.cs` | Usar `HotelWise.Core.SDK.Common` e `HotelWise.Core.SDK.Security` |
| 1.4 | Atualizar usings nos controllers | `Controllers/HotelEndpoints/RoomsController.cs` | Usar `HotelWise.Core.SDK.Common` e `HotelWise.Core.SDK.Security` |
| 1.5 | Atualizar usings nos controllers | `Controllers/HotelEndpoints/ReservationsController.cs` | Usar `HotelWise.Core.SDK.Common` e `HotelWise.Core.SDK.Security` |
| 1.6 | Atualizar usings nos controllers | `Controllers/HotelEndpoints/RoomAvailabilityController.cs` | Usar `HotelWise.Core.SDK.Common` e `HotelWise.Core.SDK.Security` |
| 1.7 | Atualizar usings nos controllers | `Controllers/AuthController.cs` | Usar `HotelWise.Core.SDK.Common` e `HotelWise.Core.SDK.Security` |
| 1.8 | Atualizar usings nos controllers | `Controllers/Ai/AssistantController.cs` | Usar `HotelWise.Core.SDK.Common` e `HotelWise.Core.SDK.AI.DTO` |
| 1.9 | Atualizar pipeline de middlewares | `Configure/WebApplicationConfigureBuilder.cs` | Consumir middlewares canônicos de `HotelWise.Core.SDK.Infrastructure.Middleware` |
| 1.10 | Atualizar DI de segurança | `Configure/ServiceCollectionConfigureSecurity.cs` | Importar `TokenConfigurationDto` de `HotelWise.Core.SDK.Security` |
| 1.11 | Atualizar DI geral | `Configure/WebApplicationConfigureServiceCollections.cs` | Ajustar referências de tipos do Core |
| 1.12 | Atualizar DI geral | `Configure/ServiceCollectionAddAllDependencies.cs` | Ajustar referências de tipos do Core |
| 1.13 | Validar ausência de namespaces legados | Toda a solução | `grep -r "SmartDigitalPsico" --include="*.cs"` deve retornar 0 |

---

### 2. Mapa de Substituição de Namespaces nos Controllers

```diff
- using HotelWise.Domain.Dto;
+ using HotelWise.Core.SDK.Common;

- using HotelWise.Domain.Helpers;
+ using HotelWise.Core.SDK.Security;
+ using HotelWise.Core.SDK.Logging;

- using HotelWise.Domain.CustomMiddleware;
+ using HotelWise.Core.SDK.Infrastructure.Middleware;
```

---

### 3. Exemplo de Controller Atualizado

No arquivo `HotelWise.API/Controllers/HotelEndpoints/HotelsController.cs`:

```csharp
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Security;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

## Lote W2 — Smoke Test, Validação Funcional e Integração

**Dependências:** Lote W1 concluído com build verde  
**Escopo:** Validação de ponta a ponta da API

### 1. Roteiro de Smoke Tests

| # | Teste | Método HTTP | Rota | Resposta Esperada |
| :--- | :--- | :--- | :--- | :--- |
| 1 | Health Check | `GET` | `/health` | Status `200 OK` |
| 2 | Swagger UI | `GET` | `/swagger/index.html` | Status `200 OK` (UI carrega) |
| 3 | Informações de Versão | `GET` | `/api/appinformationversionproduct/v1/GetAppInformationVersionProduct` | Status `200 OK` com DTO versionado |
| 4 | Autenticação (Login) | `POST` | `/api/auth/v1/login` | Status `200 OK` com `TokenVO` válido |
| 5 | Listagem de Hotéis (Protegido) | `GET` | `/api/hotels/v1` | Status `200 OK` com lista de hotéis |
| 6 | Propagação de CorrelationId | `GET` | Qualquer endpoint com header `X-Correlation-Id` | Resposta retorna o mesmo header no response |

---

### 2. Comandos de Validação e Execução

```powershell
# 1. Build da solução inteira
dotnet build c:\git\HotelWise\HotelWiseAPI\HotelWiseAPI.sln -c Release

# 2. Execução dos testes automatizados
dotnet test c:\git\HotelWise\HotelWiseAPI\HotelWiseAPI.sln

# 3. Verificação de namespace legado remanescente
Get-ChildItem -Path "c:\git\HotelWise\HotelWiseAPI" -Recurse -Filter "*.cs" | 
    Select-String "SmartDigitalPsico" | 
    ForEach-Object { $_.Path }

# 4. Geração do pacote NuGet
dotnet pack c:\git\HotelWise\HotelWiseAPI\HotelWise.Core.SDK\HotelWise.Core.SDK.csproj -c Release
```

---

## 3. Critérios de Aceite da Onda 4

1. ✅ `HotelWise.API.csproj` referencia `HotelWise.Core.SDK`.
2. ✅ Zero referências a namespaces legados (`SmartDigitalPsico.*`) no código-fonte.
3. ✅ Todos os 7 controllers importam tipos canônicos do SDK sem avisos de compilação.
4. ✅ Pipeline de middlewares HTTP orquestra CorrelationId, Logging e Exception Handling a partir do Core.SDK.
5. ✅ Smoke test passa com sucesso em todos os endpoints principais.
6. ✅ `dotnet build HotelWiseAPI.sln` compila com **0 erros** e **0 avisos críticos**.
7. ✅ Pacote NuGet `HotelWise.Core.SDK.nupkg` gerado e pronto para consumo.
