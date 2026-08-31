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

## Informações do Pacote Publicado

| Campo | Valor |
| :--- | :--- |
| **PackageId canônico** | `SmartCoreHub.Core.SDK` |
| **Versão Publicada** | `20260831.2105.0` |
| **Feed NuGet Oficial** | [https://www.nuget.org/packages/SmartCoreHub.Core.SDK/](https://www.nuget.org/packages/SmartCoreHub.Core.SDK/) |
| **Comando de Instalação** | `dotnet add package SmartCoreHub.Core.SDK --version 20260831.2105.0` |
| **DiagnosticId depreciação** | `HW_MIGRATED` |
| **Docs unificação** | `SmartCoreHub/Documentation/CoreFinal/implementacao-hotelwise-core-sdk.md` |

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
