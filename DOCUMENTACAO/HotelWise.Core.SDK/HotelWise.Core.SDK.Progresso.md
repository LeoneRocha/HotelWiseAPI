# Progresso da Implementação — HotelWise.Core.SDK

**Versão:** 1.0.0  
**Data:** 2026-08-28  
**Status Geral:** 🟡 Documentação Consolidada — Pronto para Início da Execução  
**Pacote Alvo:** `HotelWise.Core.SDK` (NuGet Único)  
**TFM:** `net10.0` (host) · `net10.0;net8.0;netstandard2.1;netstandard2.0` (Core.SDK)

---

## 1. Documentos Vinculados

| Categoria | Documento | Status |
| :--- | :--- | :---: |
| **Visão Geral** | [HotelWise.Core.SDK.Levantamento.md](./HotelWise.Core.SDK.Levantamento.md) | ✅ Consolidado |
| **Plano Geral** | [HotelWise.Core.SDK.PlanoImplementacao.md](./HotelWise.Core.SDK.PlanoImplementacao.md) | ✅ Consolidado |
| **Domain** | [HotelWise.Core.SDK.Especificacao.Domain.md](./HotelWise.Core.SDK.Especificacao.Domain.md) · [PlanoImplementacao.Domain.md](./HotelWise.Core.SDK.PlanoImplementacao.Domain.md) | ✅ Consolidado |
| **Data** | [HotelWise.Core.SDK.Especificacao.Data.md](./HotelWise.Core.SDK.Especificacao.Data.md) · [PlanoImplementacao.Data.md](./HotelWise.Core.SDK.PlanoImplementacao.Data.md) | ✅ Consolidado |
| **Service** | [HotelWise.Core.SDK.Especificacao.Service.md](./HotelWise.Core.SDK.Especificacao.Service.md) · [PlanoImplementacao.Service.md](./HotelWise.Core.SDK.PlanoImplementacao.Service.md) | ✅ Consolidado |
| **API** | [HotelWise.Core.SDK.Especificacao.API.md](./HotelWise.Core.SDK.Especificacao.API.md) · [PlanoImplementacao.API.md](./HotelWise.Core.SDK.PlanoImplementacao.API.md) | ✅ Consolidado |

---

## 2. Checklist Global de Execução por Onda e Projeto

| Fase / Onda | Projeto Alvo | Escopo | Total Tipos | Status | Progresso |
| :--- | :--- | :--- | :---: | :---: | :---: |
| **Fase 0** | `HotelWise.Core.SDK` + `.Tests` | Scaffold dos projetos `.csproj`, solution, `LICENSE`, `README`, `GlobalUsings` | — | Pendente | 0% |
| **Onda 1 (D1–D7)** | `HotelWise.Domain` | Entidades base, DTOs, helpers, middlewares, adaptadores e configs IA | **92** | Pendente | 0% |
| **Onda 2 (A1)** | `HotelWise.Data` | `GenericRepositoryBase<T, TContext>`, extensões EF e adaptação dos repos | **4** | Pendente | 0% |
| **Onda 3 (S1–S2)** | `HotelWise.Service` | `GenericEntityServiceBase`, `TokenService`, fábricas de IA e DI extensions | **11** | Pendente | 0% |
| **Onda 4 (W1–W2)** | `HotelWise.API` | `ProjectReference`, usings nos controllers, fix namespace legado, smoke tests | **12** | Pendente | 0% |
| **Consolidação** | Solução Completa | Cobertura de testes $\ge 90\%$, `dotnet pack`, auditoria de warnings | — | Pendente | 0% |

**Progresso Global Estimado:** **0%** (Fase de Planejamento e Especificação 100% Concluída)

---

## 3. Detalhamento dos Lotes de Execução

### Fase 0 — Scaffold e Estrutura Base
- [ ] Criar `HotelWise.Core.SDK/HotelWise.Core.SDK.csproj` com multi-targeting (`net10.0;net8.0;netstandard2.1;netstandard2.0`)
- [ ] Criar `HotelWise.Core.SDK.Tests/HotelWise.Core.SDK.Tests.csproj` com xUnit, Moq, FluentAssertions e Coverlet
- [ ] Criar `GlobalUsings.cs`, `README.md` e `LICENSE`
- [ ] Adicionar projetos à `HotelWiseAPI.sln`
- [ ] Referenciar `GroqApiLibrary` no `HotelWise.Core.SDK.csproj`
- [ ] Validar compilação limpa da solution

### Onda 1 — HotelWise.Domain (92 tipos)
- [ ] **Lote D1 (Fundamentos):** `EntityBase`, `EntityBaseWithNameEmail`, `IEntityBase*`, `IGenericRepository<T>`, `IGenericService<TDto>`, `IServiceResponse` (9 arquivos)
- [ ] **Lote D2 (DTOs & Constantes):** `ServiceResponse<T>`, `ErrorResponse`, `EntityDtoBase`, `SecurityDto`, `TokenConfigurationDto`, `TokenVO`, `ETypeDataBase`, constantes, `AppWarningException` (21 arquivos)
- [ ] **Lote D3 (Helpers):** `DataHelper`, `CultureDateTimeHelper`, `TimeFormatter`, `MarkdownHelper`, `HtmlHelper`, `HelperValidation`, `SecurityHelper`, `SecurityHelperApi`, `ServiceCollectionHelper`, `EnumExtensions`, `ConfigurationAppSettingsHelper`, `LogAppHelper` (12 arquivos)
- [ ] **Lote D4 (Middlewares):** `CorrelationIdMiddleware`, `GlobalExceptionMiddleware`, `RequestLoggingMiddleware` (3 arquivos)
- [ ] **Lote D5 (IA Abstrações & Enums):** Interfaces de IA/SK/AppConfig e enums de IA (17 arquivos)
- [ ] **Lote D6 (IA Adapters & Configs):** 5 adapters LLM/VectorStore, 18 configs RAG, `PromptMessageVO`, `DataVectorBase`, `AskAssistantResponse/Request` (26 arquivos)
- [ ] **Lote D7 (IA Validators & Helpers):** Validadores de prompts, `EmbeddingHelper`, `ChatSessionHelper`, `TokenCounterHelper` (6 arquivos)
- [ ] Shims `[Obsolete]` com `DiagnosticId = "HW_CORE_SDK_*"` em todos os 92 arquivos do host

### Onda 2 — HotelWise.Data (4 tipos)
- [ ] **Lote A1:** `GenericRepositoryBase<T, TContext>`, `ModelBuilderExtensions`, `HelperCharSet`, `ConfigurationEntitiesHelper` (4 arquivos)
- [ ] Adicionar `ProjectReference` no `HotelWise.Data.csproj`
- [ ] Shims `[Obsolete]` nos 4 arquivos originais do host
- [ ] Atualizar herança nos 6 repositórios concretos de domínio
- [ ] Implementar suíte `GenericRepositoryBaseTests`

### Onda 3 — HotelWise.Service (11 tipos)
- [ ] **Lote S1:** `GenericEntityServiceBase<T, TDto>`, `GenericVectorStoreServiceBase`, `TokenService` (3 arquivos)
- [ ] **Lote S2:** `AIInferenceAdapterFactory`, `AIInferenceService`, `VectorStoreAdapterFactory`, `SemanticKernelProviderConfigure`, `ConfigureServicesAI`, `ServiceCollectionConfigureCors`, `ServiceCollectionConfigureAppSettings`, `ServiceCollectionConfigureAutoMapper` (8 arquivos)
- [ ] Adicionar `ProjectReference` no `HotelWise.Service.csproj`
- [ ] Shims `[Obsolete]` nos 11 arquivos originais do host
- [ ] Atualizar herança nos 14 serviços concretos mantidos no host
- [ ] Implementar suíte `GenericEntityServiceBaseTests`, `TokenServiceTests`, `AIInferenceAdapterFactoryTests`

### Onda 4 — HotelWise.API (Consumo e Integração)
- [ ] **Lote W1:** Adicionar `ProjectReference`, atualizar `using`s nos 7 controllers, corrigir namespace legado em `AppInformationVersionProductController.cs`, atualizar pipeline de middlewares
- [ ] **Lote W2:** Executar smoke tests (`/health`, `/swagger`, endpoints de Hotéis, Quartos, Reservas, Autenticação)
- [ ] Validar 0 ocorrências de `SmartDigitalPsico` no código-fonte

### Consolidação Final
- [ ] Cobertura de linhas $\ge 90\%$ e branches $\ge 85\%$ no `HotelWise.Core.SDK.Tests`
- [ ] `dotnet build HotelWiseAPI.sln` com **0 erros** e **0 avisos críticos**
- [ ] `dotnet pack HotelWise.Core.SDK.csproj` gerando `.nupkg` com símbolos `.snupkg` e documentação XML

---

## 4. Evidências de Validação por Fase

| Fase | Build Solution | Testes Automatizados | Cobertura SDK | Shims Obsolete | Namespace Legado | Observações |
| :---: | :---: | :---: | :---: | :---: | :---: | :--- |
| **0** | — | — | — | — | — | Scaffold inicial |
| **1** | — | — | — | — | — | Domain portado |
| **2** | — | — | — | — | — | Data portado |
| **3** | — | — | — | — | — | Service portado |
| **4** | — | — | — | — | — | API integrada |
| **Final** | — | — | — | — | — | Pacote NuGet validado |
