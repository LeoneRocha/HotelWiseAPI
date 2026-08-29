# Progresso da Implementação — HotelWise.Core.SDK

**Versão:** 2.2.0  
**Data:** 2026-08-29  
**Status Geral:** 🟢 Consolidação concluída — migração completa  
**Pacote Alvo:** `HotelWise.Core.SDK` (NuGet Único)  
**TFM:** `net10.0` (host) · `net10.0;net8.0;netstandard2.1;netstandard2.0` (Core.SDK)

---

## 1. Documentos Vinculados

| Categoria | Documento | Status |
| :--- | :--- | :---: |
| **Visão Geral** | [HotelWise.Core.SDK.Levantamento.md](./HotelWise.Core.SDK.Levantamento.md) | ✅ Consolidado |
| **Guia de Referência** | [HotelWise.Core.SDK.Guide.md](./HotelWise.Core.SDK.Guide.md) | ✅ Publicado |
| **Plano Geral** | [HotelWise.Core.SDK.PlanoImplementacao.md](./HotelWise.Core.SDK.PlanoImplementacao.md) | ✅ Consolidado |
| **Domain** | [HotelWise.Core.SDK.Especificacao.Domain.md](./HotelWise.Core.SDK.Especificacao.Domain.md) · [PlanoImplementacao.Domain.md](./HotelWise.Core.SDK.PlanoImplementacao.Domain.md) | ✅ Consolidado |
| **Data** | [HotelWise.Core.SDK.Especificacao.Data.md](./HotelWise.Core.SDK.Especificacao.Data.md) · [PlanoImplementacao.Data.md](./HotelWise.Core.SDK.PlanoImplementacao.Data.md) | ✅ Consolidado |
| **Service** | [HotelWise.Core.SDK.Especificacao.Service.md](./HotelWise.Core.SDK.Especificacao.Service.md) · [PlanoImplementacao.Service.md](./HotelWise.Core.SDK.PlanoImplementacao.Service.md) | ✅ Consolidado |
| **API** | [HotelWise.Core.SDK.Especificacao.API.md](./HotelWise.Core.SDK.Especificacao.API.md) · [PlanoImplementacao.API.md](./HotelWise.Core.SDK.PlanoImplementacao.API.md) | ✅ Consolidado |

---

## 2. Checklist Global de Execução por Onda e Projeto

| Fase / Onda | Projeto Alvo | Escopo | Total Tipos | Status | Progresso |
| :--- | :--- | :--- | :---: | :---: | :---: |
| **Fase 0** | `HotelWise.Core.SDK` + `.Tests` | Scaffold | — | ✅ Concluído | 100% |
| **Onda 1 (D1–D7)** | `HotelWise.Domain` | 92 tipos | **92** | ✅ Concluído | 100% |
| **Onda 2 (A1)** | `HotelWise.Data` | 4 tipos | **4** | ✅ Concluído | 100% |
| **Onda 3 (S1–S2)** | `HotelWise.Service` | 11 tipos | **11** | ✅ Concluído | 100% |
| **Onda 4 (W1–W2)** | `HotelWise.API` | usings + smoke | **12** | ✅ Concluído | 100% |
| **Consolidação** | Solução Completa | cobertura ≥ 90%, pack, CI | — | ✅ Concluído | 100% |

**Progresso Global Estimado:** **100%**

---

## 3. Detalhamento — Consolidação

### Auditoria final
- [x] `dotnet build HotelWiseAPI.sln -c Release` — 0 erros
- [x] `dotnet test HotelWise.Core.SDK.Tests` — **79/79** passando
- [x] `SmartDigitalPsico` em `*.cs` = **0**
- [x] Shims host `HW_CORE_SDK_*` (Domain/Data/Service): **108** arquivos
- [x] Referências API a `HotelWise.Core.SDK`: presentes (controllers/middlewares/helpers)

### Cobertura ≥ 90% (unit-testável)
- [x] Coverlet via `--collect:"XPlat Code Coverage"` + `HotelWise.Core.SDK.Tests/coverlet.runsettings`
- [x] **Line coverage: 92.62%** (740/799 linhas incluídas; `line-rate=0.9261`)
- [x] Exclusões Coverlet (rede/SK live): `AI/Adapters/*`, `SemanticKernelProviderConfigure`, `ApplicationIAConfig`
- [x] Testes gap: `Consolidation/ConsolidationCoverageTests.cs` + `ConsolidationCoverageGapsTests.cs`

### Pack NuGet
- [x] `NU5104` adicionado ao `NoWarn` (deps SK alpha); versão do pacote permanece **1.0.0**
- [x] Pack Release: `artifacts/core-sdk/HotelWise.Core.SDK.1.0.0.nupkg` + `.snupkg` + XML docs

### CI mínimo
- [x] Workflow: `.github/workflows/core-sdk.yml` (restore → build Core+Tests → test Coverlet → pack)
- [x] Triggers: push/PR em `HotelWise.Core.SDK/**` e `HotelWise.Core.SDK.Tests/**`

### Consolidação
- [x] Cobertura ≥ 90%, pack/NU5104, CI, Progresso 100%

---

## 4. Evidências de Validação

| Fase | Build | Testes | Observações |
| :---: | :---: | :---: | :--- |
| **0–3** | ✅ | ✅ 56 | Domain + Data + Service |
| **4 W1–W2** | ✅ 0 erros / 0 avisos | ✅ 56/56 | API consome Core; pack OK |
| **Consolidação** | ✅ | ✅ 79/79 | Coverlet **92.62%**; pack `1.0.0`; CI `core-sdk.yml` |

### Roteiro smoke HTTP (manual / ambiente com MySQL)
1. `GET /health` → 200  
2. `GET /swagger/index.html` → 200  
3. `GET /api/appinformationversionproduct/v1/GetAppInformationVersionProduct` → 200  
4. `POST /api/auth/v1/login` (ou Authenticate) → token  
5. `GET /api/hotels/v1` Bearer → 200  
6. Header `X-Correlation-Id` ecoado na resposta  
