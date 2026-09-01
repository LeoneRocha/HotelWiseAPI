# Conjunto Homologado — Ciclo 2026-08-31 (HotelWiseAPI)

**Data:** 2026-08-31  
**Guia Genérico:** [GuiaGenericoAtualizacaoPacotes.md](./GuiaGenericoAtualizacaoPacotes.md)  
**Guia Específico:** [GuiaAtualizacaoPacotes-HotelWiseAPI.md](./GuiaAtualizacaoPacotes-HotelWiseAPI.md)  
**Escopo:** `HotelWiseAPI/Directory.Packages.props` (hosts + casca `HotelWise.Core.SDK` via CPM compartilhado)  
**Exclusões explícitas (travas mantidas):** EF Core 9 / Pomelo 9 · `xunit.runner.visualstudio` 3.1.5 (não migrar para 4.x neste ciclo) · JwtBearer `VersionOverride` 8.0.16 no TFM `net8.0` da casca  

---

## 1. NuGet Aplicado (`Directory.Packages.props`)

### Bloco AI — Semantic Kernel 1.78.0 → 1.80.0

| Pacote | Anterior | Aplicado | Latest |
| ------ | -------- | -------- | ------ |
| Microsoft.SemanticKernel | 1.78.0 | **1.80.0** | 1.80.0 |
| Microsoft.SemanticKernel.Abstractions | 1.78.0 | **1.80.0** | 1.80.0 |
| Microsoft.SemanticKernel.Core | 1.78.0 | **1.80.0** | 1.80.0 |
| Microsoft.SemanticKernel.PromptTemplates.Handlebars | 1.78.0 | **1.80.0** | 1.80.0 |
| Microsoft.SemanticKernel.Agents.Abstractions | 1.78.0 | **1.80.0** | 1.80.0 |
| Microsoft.SemanticKernel.Agents.Core | 1.78.0 | **1.80.0** | 1.80.0 |
| Microsoft.SemanticKernel.Connectors.MistralAI | 1.78.0-alpha | **1.80.0-alpha** | 1.80.0-alpha |
| Microsoft.SemanticKernel.Connectors.Ollama | 1.78.0-alpha | **1.80.0-alpha** | 1.80.0-alpha |
| Microsoft.SemanticKernel.Plugins.Memory | 1.78.0-alpha | **1.80.0-alpha** | 1.80.0-alpha |

### Bloco E — Testes

| Pacote | Anterior | Aplicado | Latest |
| ------ | -------- | -------- | ------ |
| FluentAssertions | 8.3.0 | **8.10.0** | 8.10.0 |

### Não atualizado (travas / já no latest útil)

| Pacote / família | Versão | Motivo |
| ---------------- | ------ | ------ |
| Microsoft.EntityFrameworkCore.* | 9.0.18 | Trava Pomelo 9 ↔ EF 9 |
| Pomelo.EntityFrameworkCore.MySql | 9.0.0 | Sem release oficial 10.x |
| xunit.runner.visualstudio | 3.1.5 | Major 4.x fora deste ciclo |
| Microsoft.AspNetCore.Authentication.JwtBearer (CPM) | 10.0.11 | Hosts net10.0 alinhados |
| JwtBearer `VersionOverride` (casca `net8.0`) | 8.0.16 | Multi-target deliberado; Consolidate no VS é esperado |
| SmartCoreHub.Core.SDK | 20260831.2105.0 | Consumo NuGet; bump só após nova publicação SCH |
| Markdig / QuestPDF / Identity.Web / Graph | latest atual no CPM | Sem delta aplicável |

---

## 2. Validações Executadas

```text
dotnet list HotelWiseAPI.sln package --outdated
  -> delta aplicável: SemanticKernel 1.80.0 + FluentAssertions 8.10.0
  -> majors bloqueadas: EF 10, xUnit runner 4

dotnet list HotelWiseAPI.sln package --vulnerable --include-transitive
  -> 0 vulnerabilidades

dotnet restore HotelWiseAPI.sln -> 0 erros
dotnet build HotelWiseAPI.sln -c Release -> 0 erros
dotnet build HotelWise.Core.SDK/HotelWise.Core.SDK.csproj -c Release -> 0 erros (multi-TFM)

dotnet test HotelWiseAPI.sln -c Release --no-build -> 264 aprovados / 0 falhas:
  - HotelWise.Domain.Tests:       27
  - HotelWise.Data.Tests:         20
  - HotelWise.Service.Tests:      83
  - HotelWise.API.Tests:          55
  - HotelWise.Core.SDK.Tests:     79
```

---

## 3. Notas operacionais

1. **Consolidate JwtBearer:** o aviso no VS entre `10.0.11` (hosts) e `8.0.16` (`VersionOverride` no `HotelWise.Core.SDK` para `net8.0`) é **intencional** — não consolidar para 10.x no TFM net8.0.
2. **Alinhamento SCH:** o backend SmartCoreHub já homologou SK 1.80.0 no mesmo dia (`2026-08-31-ConjuntoHomologado.md` no repo SCH). Após republicar `SmartCoreHub.Core.SDK`, atualizar o `PackageVersion` no CPM do HotelWise.
3. **Próximo ciclo:** EF Core 10 quando Pomelo 10 oficial; avaliar `xunit.runner.visualstudio` 4.x em ciclo dedicado.
