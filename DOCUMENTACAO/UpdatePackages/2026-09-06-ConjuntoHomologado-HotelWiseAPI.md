# Conjunto Homologado — Ciclo 2026-09-06 (HotelWiseAPI)

**Data:** 2026-09-06  
**Guia Genérico:** [GuiaGenericoAtualizacaoPacotes.md](./GuiaGenericoAtualizacaoPacotes.md)  
**Guia Específico:** [GuiaAtualizacaoPacotes-HotelWiseAPI.md](./GuiaAtualizacaoPacotes-HotelWiseAPI.md)  
**Exclusões explícitas (travas mantidas):** EF Core 9 / Pomelo 9 · `xunit.runner.visualstudio` 3.1.5 (não migrar para 4.x neste ciclo)

---

## 1. NuGet Aplicado (`Directory.Packages.props`)

### Bloco AI — Semantic Kernel 1.80.0 → 1.80.1

| Pacote | Anterior | Aplicado | Latest |
| ------ | -------- | -------- | ------ |
| Microsoft.SemanticKernel | 1.80.0 | **1.80.1** | 1.80.1 |
| Microsoft.SemanticKernel.Abstractions | 1.80.0 | **1.80.1** | 1.80.1 |
| Microsoft.SemanticKernel.Core | 1.80.0 | **1.80.1** | 1.80.1 |
| Microsoft.SemanticKernel.PromptTemplates.Handlebars | 1.80.0 | **1.80.1** | 1.80.1 |
| Microsoft.SemanticKernel.Agents.Abstractions | 1.80.0 | **1.80.1** | 1.80.1 |
| Microsoft.SemanticKernel.Agents.Core | 1.80.0 | **1.80.1** | 1.80.1 |
| Microsoft.SemanticKernel.Connectors.MistralAI | 1.80.0-alpha | **1.80.1-alpha** | 1.80.1-alpha |
| Microsoft.SemanticKernel.Connectors.Ollama | 1.80.0-alpha | **1.80.1-alpha** | 1.80.1-alpha |
| Microsoft.SemanticKernel.Plugins.Memory | 1.80.0-alpha | **1.80.1-alpha** | 1.80.1-alpha |

### Bloco D — Utilitários e Nuvem

| Pacote | Anterior | Aplicado | Latest |
| ------ | -------- | -------- | ------ |
| Microsoft.Graph | 6.5.0 | **6.6.0** | 6.6.0 |

### Bloco A — Kiota Runtime e Serializers (Pinagem Transitiva & Direta)

| Pacote | Anterior | Aplicado | Latest |
| ------ | -------- | -------- | ------ |
| Microsoft.Kiota.Abstractions | 2.0.0 | **2.1.1** | 2.1.1 |
| Microsoft.Kiota.Authentication.Azure | 2.0.0 | **2.1.1** | 2.1.1 |
| Microsoft.Kiota.Http.HttpClientLibrary | 2.0.0 | **2.1.1** | 2.1.1 |
| Microsoft.Kiota.Serialization.Form | 2.0.0 | **2.1.1** | 2.1.1 |
| Microsoft.Kiota.Serialization.Json | 2.0.0 | **2.1.1** | 2.1.1 |
| Microsoft.Kiota.Serialization.Multipart | 2.0.0 | **2.1.1** | 2.1.1 |
| Microsoft.Kiota.Serialization.Text | 2.0.0 | **2.1.1** | 2.1.1 |

### Não atualizado (travas / já no latest útil)

| Pacote / família | Versão | Motivo |
| ---------------- | ------ | ------ |
| Microsoft.EntityFrameworkCore.* | 9.0.18 | Trava Pomelo 9 ↔ EF 9 |
| Pomelo.EntityFrameworkCore.MySql | 9.0.0 | Sem release oficial 10.x |
| Microsoft.AspNetCore.Authentication.JwtBearer (CPM) | 10.0.11 | Hosts e bibliotecas net10.0 alinhados; 0 avisos no Consolidate do VS |
| SmartCoreHub.Core.SDK | 20260906.516.0 | Consumo NuGet; alinhado no latest |
| Markdig / QuestPDF / Identity.Web / AutoMapper / Serilog | latest atual no CPM | Sem delta aplicável |

---

## 2. Validações Executadas

```text
dotnet list HotelWiseAPI.sln package --outdated
  -> delta aplicável: SemanticKernel 1.80.1 + Graph 6.6.0 + Kiota 2.1.1
  -> majors bloqueadas: EF 10, xUnit runner 4

dotnet list HotelWiseAPI.sln package --vulnerable --include-transitive
  -> 0 vulnerabilidades

dotnet restore HotelWiseAPI.sln -> 0 erros
dotnet build HotelWiseAPI.sln -c Release -> 0 erros
dotnet pack GroqApiLibrary/GroqApiLibrary.csproj -c Release -> 0 erros (multi-TFM net8.0;net10.0)
dotnet pack HotelWise.Core.SDK/HotelWise.Core.SDK.csproj -c Release -> 0 erros (multi-TFM net8.0;net10.0;netstandard2.0;netstandard2.1)

dotnet test HotelWiseAPI.sln -c Release --no-build -> 264 aprovados / 0 falhas:
  - HotelWise.Domain.Tests:       27
  - HotelWise.Data.Tests:         20
  - HotelWise.Service.Tests:      83
  - HotelWise.API.Tests:          55
  - HotelWise.Core.SDK.Tests:     79
```

---

## 3. Notas Operacionais

1. **Consolidação JwtBearer saneada (Consolidate 0):** A dependência `Microsoft.AspNetCore.Authentication.JwtBearer` foi removida de `HotelWise.Core.SDK` (seguindo a arquitetura canônica de `SmartCoreHub.Core.SDK`, onde o SDK multi-TFM provê abstrações e não acopla middlewares de runtime Web). Todos os projetos consumidores (`HotelWise.API`, `HotelWise.Service`, `HotelWise.Data`, `HotelWise.Domain`) permanecem 100% consolidados e alinhados na versão `10.0.11` via CPM, zerando o aviso de consolidação na IDE.
2. **Kiota e Graph:** A sincronização de `Microsoft.Graph 6.6.0` com `Microsoft.Kiota.* 2.1.1` garante integridade completa da árvore de comunicação HTTP e serialização.
3. **Semantic Kernel 1.80.1:** Bumping uniforme de abstrações, agentes, core e conectores (`1.80.1` e `1.80.1-alpha`).
4. **Próximo ciclo:** EF Core 10 quando Pomelo 10 oficial for lançado; avaliar `xunit.runner.visualstudio` 4.x em ciclo dedicado.
