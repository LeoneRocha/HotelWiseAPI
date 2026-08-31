# HotelWise.Core.SDK → casca sobre SmartCoreHub.Core.SDK

**Status:** preparação (Etapa 1 concluída; casca **ainda não** ativada)  
**Atualizado:** 2026-08-31 (mensagens `[Obsolete]` alinhadas às camadas Common/Domain/Infrastructure/Service)

Os tipos já migrados neste pacote estão marcados com `[Obsolete]`:
- citam a **camada** SCH (`Common` / `Domain` / `Infrastructure` / `Service`);
- citam o **pacote NuGet** `SmartCoreHub.Core.SDK` e o **FQN** do tipo destino;
- avisam que, **após publicar o NuGet**, `HotelWise.Core.SDK` vira **só casca** (`PackageReference` + wrappers) e **delega a `SmartCoreHub.Core.SDK`** (não a si mesmo).

`DiagnosticId = "HW_MIGRATED"` entra na **casca** (Etapa 3) — hoje omitido por compatibilidade `netstandard2.0`.



## Layout SCH (camadas)

No `SmartCoreHub.Core.SDK`, o material migrado vive sob:

- `Common/` — Attributes, DTOs/constants HW, `Security/`
- `Domain/` — `Abstractions/`, `AI/` (contratos), `Helpers/` (+ `Ported/`), `Extensions/`
- `Infrastructure/` — `AI/Adapters`, ThirdParty, `Data/ModelBuilderExtensions`, Middleware
- `Service/` — `AI/` (runtime), DI, `Services/Generic`, `Validation/`, `API/Helpers/Ported`

FQNs nas mensagens `[Obsolete]` já apontam esses namespaces.

## Placeholders — preencher após publicação

| Campo | Valor atual / TODO |
| :--- | :--- |
| **PackageId canônico** | `SmartCoreHub.Core.SDK` |
| **Versão mínima alvo** | `__TODO_NUGET_VERSION__` *(artifact local Etapa 1: `2.0.0`; Etapa 3 planejada: `3.0.0`)* |
| **Feed NuGet** | `__TODO_NUGET_FEED_URL__` *(nuget.org / Azure Artifacts / feed interno)* |
| **Artifact local (pré-publish)** | `SmartCoreHub/.../artifacts/SmartCoreHub.Core.SDK.2.0.0.nupkg` |
| **DiagnosticId depreciação** | `HW_MIGRATED` *(aplicar na casca Etapa 3; hoje só na mensagem/docs)* |
| **Docs unificação** | `repos/SmartCoreHub/Documentation/CoreFinal/` (`PROGRESSO.md` Etapa 3, `sdk-migration.md` §10) |

## Checklist para ativar a casca

1. Publicar `SmartCoreHub.Core.SDK` no feed e preencher os placeholders acima.
2. No `HotelWise.Core.SDK.csproj`, **descomentar** o `PackageReference` para `SmartCoreHub.Core.SDK` (bloco `TODO CASCA`).
3. Remover (ou esvaziar) a implementação dos tipos migrados e substituir por thin-wrappers, por exemplo:

```csharp
namespace HotelWise.Core.SDK.Abstractions;

[Obsolete(
    "Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericService. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.",
    DiagnosticId = "HW_MIGRATED")]
public interface IGenericService<TDto> : SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericService<TDto>
    where TDto : class
{
}
```

4. Manter no HW apenas o que **não** foi portado (ex.: `EntityBase`, `TokenService`, middlewares Correlation/RequestLogging) até Etapa 3 / reuse explícito.
5. Validar: `HotelWise.Core.SDK.Tests` 79/79; hosts compilam com CS0618 (não bloqueante).

## O que já está feito

- Port aditivo no `SmartCoreHub.Core.SDK` (Etapa 1).
- `[Obsolete]` nos **97** tipos migrados, com mensagem citando o pacote NuGet `SmartCoreHub.Core.SDK` e o FQN de destino (casca + `DiagnosticId = HW_MIGRATED` na Etapa 3).
- Este arquivo + bloco comentado no `.csproj` prontos para preencher após publish.
