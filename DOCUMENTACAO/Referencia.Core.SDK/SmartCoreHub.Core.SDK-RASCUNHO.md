# SmartCoreHub.Core.SDK — Especificação

> **Banner — rascunho obsoleto:** este documento é o **draft inicial** e **contradiz** o estado atual (pastas `CORE/` vs `backend/Core/`, API pública imaginada, “Core puro”). Use como artefato histórico apenas. Fonte de verdade: [Especificacao.md](./SmartCoreHub.Core.SDK-Especificacao.md), [MigracaoGenericos.md](./SmartCoreHub.Core.SDK-MigracaoGenericos.md) e [`README.md`](../../../../backend/Core/SmartCoreHub.Core.SDK/README.md) do pacote.
>
> **Complemento (2026-07-15):** extrações pendentes pós-migração executadas — ver [Extracao-Pendencias.md](./SmartCoreHub.Core.SDK-Extracao-Pendencias.md).

Status: Draft (obsoleto)

Este documento especifica o projeto `SmartCoreHub.Core.SDK`, uma Class Library .NET alojada em `CORE/SmartCoreHub.Core.SDK` destinada a centralizar classes genéricas, helpers e contratos reutilizáveis.

## 1. Objetivo

- Fornecer uma biblioteca modular e versionada via NuGet com primitivas de `Domain`, infraestrutura genérica, serviços utilitários e helpers.
- Reduzir duplicação entre projetos internos e ofertar contratos estáveis para consumidores.
- Seguir as mesmas convenções de empacotamento e publicação adotadas por `SmartCoreHub.Localization.SDK`.

## 2. Escopo

- Inclusões: `EntityBase`, `ValueObject`, `DomainEvent`, `Result<T>`, `BaseRepository<T>`, `UnitOfWork`, `GenericService<T>`, `ValidationService`, `NotificationService`, `ConfigHelper`, `StringHelper`, `DateTimeHelper`, `LoggerHelper`.
- Exclusões: códigos específicos de feature, integrações proprietárias de outros projetos, ou migração/remoção de código existente.

## 3. Estrutura do projeto

- `CORE/SmartCoreHub.Core.SDK/` — projeto Class Library.
  - `src/Domain/` — `EntityBase`, `ValueObject`, `DomainEvent`.
  - `src/Infrastructure/` — `BaseRepository<T>`, `UnitOfWork`, `LoggerHelper`.
  - `src/Services/` — `GenericService<T>`, `ValidationService`, `NotificationService`.
  - `src/Helpers/` — `ConfigHelper`, `StringHelper`, `DateTimeHelper`, `Result<T>`.

- `CORE/SmartCoreHub.Core.SDK.Tests/` — projeto de testes unitários (xUnit ou NUnit), com cobertura focal em helpers, serviços genéricos e infra.

## 4. API pública e contratos

- Namespaces públicos devem ser estáveis e versionados semanticamente:
  - `SmartCoreHub.Core.SDK.Domain`
  - `SmartCoreHub.Core.SDK.Infrastructure`
  - `SmartCoreHub.Core.SDK.Services`
  - `SmartCoreHub.Core.SDK.Helpers`

- Contratos mínimos:
  - `EntityBase` — `Guid Id` (ou `long Id` conforme padrão do repositório), `DateTime CreatedAt`, `DateTime? UpdatedAt`, `bool IsActive`.
  - `ValueObject` — `protected abstract IEnumerable<object> GetEqualityComponents()` + `Equals/GetHashCode` implementados.
  - `Result<T>` — `bool IsSuccess`, `T Value`, `IEnumerable<string> Errors`, `ErrorCode` opcional.

## 5. Infraestrutura

- `BaseRepository<T>`: operações assíncronas `AddAsync`, `UpdateAsync`, `DeleteAsync`, `GetByIdAsync`, `ListAsync` (aceita paginação/filtragem mínima). Deve ser facilmente adaptável a EF Core ou Dapper por implementação concreta.
- `UnitOfWork`: `BeginTransactionAsync`, `CommitAsync`, `RollbackAsync` e integração com provedores de persistência.
- `LoggerHelper`: métodos para enriquecer logs com `CorrelationId`, `Context` e evitar exposição de secrets.

## 6. Services

- `GenericService<T>`: orquestra regras comuns + chamadas ao repositório. Deve depender de interfaces (`IRepository<T>`, `IUnitOfWork`, `ISmartCoreHubMapper`) e não de implementações concretas.
- `ValidationService`: validações reutilizáveis (p.ex. `EnsureNotNullOrWhitespace`, `EnsureMaxLength`, `ValidateResourceKeyPrefix` — reutilizar regras existentes no repo quando aplicável).
- `NotificationService`: pub/sub local simples; interface para integrar com message bus externo se necessário.

## 7. Helpers

- `ConfigHelper`: helpers para `IConfiguration` binding com validação de valores obrigatórios e `Get<T>` com fallback seguro.
- `StringHelper`: normalização, trim/normalize, `IsNullOrWhiteSpaceWithLimit`.
- `DateTimeHelper`: UTC helpers, start/end-of-day, truncation e comparação com tolerância.

## 8. Testes

- Framework recomendado: `xUnit` (consistente com a maioria dos projetos atuais; permitir `NUnit` se preferido pela equipe).
- Cobertura mínima: criar testes unitários para todos os helpers, para `Result<T>` e para um cenário de `GenericService<T>` com repositório em-memory.
- Convenção de nomes: `Method_Scenario_ExpectedResult` e comentários em português explicando o objetivo do teste.

## 9. Publicação e CI/CD

- Empacotar como `PackageId = SmartCoreHub.Core.SDK` (ou outra convenção aprovada pela equipe).
- Incluir metadados no `.csproj`: `Authors`, `RepositoryUrl`, `PackageLicenseExpression`, `Company`.
- Reaproveitar pipeline de build/publish usado por `SmartCoreHub.Localization.SDK` (ver `Documentation/Features/SDK/` para exemplos de pipeline). O pipeline deve executar: build, test, pack, sign (se aplicável), push para feed interno.

## 10. Segurança e boas práticas

- Não colocar segredos no repositório. Usar variáveis de ambiente e feeds privados para publicação.
- Não expor dados sensíveis nos logs; usar `LoggerHelper` para padronizar e filtrar campos sensíveis.

## 11. Backwards compatibility

- Interfaces públicas só podem ter breaking changes em major releases.
- Documentar migrações de API no changelog do pacote.

## 12. Rascunho de tarefas iniciais

1. Criar projetos `.csproj` em `CORE/SmartCoreHub.Core.SDK` e `CORE/SmartCoreHub.Core.SDK.Tests`.
2. Implementar `EntityBase`, `ValueObject`, `Result<T>` e `StringHelper` com testes.
3. Implementar `BaseRepository<T>` e um provider in-memory para testes.
4. Criar pipeline CI sem publicação automática (apenas build+test) inicialmente.

---

Referências:

- `Documentation/Features/SDK/SmartCoreHub.Localization.SDK-Requisitos.md` (para convenções de publicação e pipeline)
- `PROJECT_GUIDELINES.md` (regras gerais do repositório)


# SmartCoreHub.Core.SDK — Especificação Completa

Status: Draft

## Objetivo

Criar uma nova Class Library chamada `SmartCoreHub.Core.SDK` responsável por centralizar componentes reutilizáveis entre todos os projetos SmartCoreHub.

IMPORTANTE:

* Não mover código existente.
* Não alterar projetos existentes.
* Não remover implementações atuais.
* Criar uma nova SDK independente.
* Todo código deve ser genérico e reutilizável.
* O objetivo é reduzir duplicação futura.
* O projeto deve ser empacotado como NuGet seguindo exatamente o mesmo padrão utilizado em `SmartCoreHub.Localization.SDK`.

Antes de iniciar a implementação:

* Ler obrigatoriamente `PROJECT_GUIDELINES.md`.
* Analisar a estrutura e configuração do projeto `SmartCoreHub.Localization.SDK`.
* Replicar padrões de:

  * Package metadata
  * Versionamento
  * Build
  * Testes
  * CI/CD
  * Publicação NuGet
  * Estrutura de solução

---

# Estrutura da Solução

```text
CORE/
 ├─ SmartCoreHub.Core.SDK/
 │   ├─ Domain/
 │   ├─ Infrastructure/
 │   ├─ Services/
 │   ├─ Helpers/
 │   ├─ Extensions/
 │   ├─ Exceptions/
 │   ├─ Validation/
 │   ├─ Logging/
 │   ├─ Security/
 │   ├─ Mapping/
 │   ├─ Configuration/
 │   ├─ Constants/
 │   ├─ Collections/
 │   ├─ Caching/
 │   ├─ Events/
 │   ├─ Specifications/
 │   └─ Abstractions/
 │
 └─ SmartCoreHub.Core.SDK.Tests/
```

---

# Domain

## EntityBase

```csharp
public abstract class EntityBase
{
    public Guid Id { get; protected set; }

    public DateTime CreatedAt { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }

    public bool IsActive { get; protected set; }
}
```

## AuditableEntity

Adicionar:

```csharp
CreatedBy
UpdatedBy
DeletedBy
DeletedAt
```

## ValueObject

Implementação completa de igualdade.

## DomainEvent

Classe base para eventos de domínio.

## AggregateRoot

Classe base para DDD.

## IEntity

Interface genérica.

## IAggregateRoot

Interface para agregados.

---

# Result Pattern

## Result

## Result<T>

## PaginatedResult<T>

## ValidationResult

## Error

```csharp
Code
Message
Metadata
```

Inspirado em FluentResults e ErrorOr.

---

# Exceptions

Criar exceções padronizadas:

```csharp
BusinessException
ValidationException
NotFoundException
ConflictException
UnauthorizedException
ForbiddenException
ConfigurationException
ExternalServiceException
```

---

# Infrastructure

## IRepository<T>

Interface genérica.

## BaseRepository<T>

Operações CRUD.

## IReadRepository<T>

Separação CQRS.

## IUnitOfWork

Transações.

## InMemoryRepository

Somente para testes.

---

# Services

## GenericCrudService<T>

Serviço genérico.

## ValidationService

Validações reutilizáveis.

## NotificationService

Notificações internas.

## RetryService

Políticas de retry.

## CacheService

Abstração de cache.

---

# Specifications

Implementar padrão Specification:

```csharp
ISpecification<T>
BaseSpecification<T>
```

Permitir:

* filtros
* includes
* ordenação
* paginação

---

# Events

## IEvent

## IDomainEvent

## IEventPublisher

## IEventHandler

## EventDispatcher

Implementação local.

Preparar para integração futura com:

* RabbitMQ
* Azure Service Bus
* Kafka

---

# Validation

Criar helpers:

```csharp
Guard
Ensure
ValidationHelper
```

Exemplos:

```csharp
Guard.NotNull()
Guard.NotEmpty()
Guard.NotNegative()
Guard.NotDefault()
```

---

# Helpers

## StringHelper

* Normalize
* RemoveAccents
* Slugify
* ToSnakeCase
* ToCamelCase
* ToPascalCase
* Truncate
* MaskDocument

## DateTimeHelper

* UTC
* TimeZone
* StartOfDay
* EndOfDay
* AgeCalculation
* BusinessDays

## NumberHelper

* Percentage
* Currency
* Decimal precision

## EnumHelper

* Description
* DisplayName
* Parsing

## JsonHelper

* Serialize
* Deserialize

## ReflectionHelper

* Property discovery
* Attribute discovery

## CollectionHelper

* Batch
* Chunk
* DistinctBy

---

# Extensions

Criar extensões para:

## StringExtensions

## DateTimeExtensions

## GuidExtensions

## EnumerableExtensions

## ServiceCollectionExtensions

## ConfigurationExtensions

---

# Logging

## ILoggerAdapter

Abstração independente.

## LoggerHelper

Adicionar:

```csharp
CorrelationId
RequestId
TraceId
```

Mascarar automaticamente:

* Senhas
* Tokens
* ConnectionStrings
* Secrets

---

# Security

## HashHelper

SHA256

## EncryptionHelper

AES

## TokenHelper

JWT helpers genéricos.

## SecureRandomHelper

---

# Configuration

## ConfigurationHelper

Leitura segura de:

```csharp
IConfiguration
Environment Variables
Options Pattern
```

## EnvironmentHelper

```csharp
IsDevelopment
IsProduction
IsTest
```

---

# Mapping

Criar abstração:

```csharp
ISmartCoreHubMapper
```

Compatível com:

* AutoMapper
* Mapster

Sem dependência direta.

---

# Caching

## ICacheProvider

## MemoryCacheProvider

Preparar para:

* Redis
* Distributed Cache

---

# Constants

Centralizar:

```csharp
DateFormats
RegexPatterns
ErrorCodes
ClaimTypes
Headers
```

---

# Testes

Criar projeto:

```text
SmartCoreHub.Core.SDK.Tests
```

Utilizar:

* xUnit
* FluentAssertions
* Moq

Cobertura mínima:

* Result
* Helpers
* Extensions
* Validation
* Repository
* Specification
* Events

Meta:

* mínimo 90% de cobertura

---

# NuGet

Replicar exatamente a configuração utilizada por SmartCoreHub.Localization.SDK:

* PackageId
* Authors
* Company
* RepositoryUrl
* PackageTags
* SourceLink
* Symbols
* XML Docs
* README
* Changelog

---

# Requisitos Arquiteturais

* SOLID
* Clean Architecture
* DDD Friendly
* CQRS Friendly
* Async First
* Nullable Enabled
* Implicit Usings Enabled
* XML Documentation
* Sonar Friendly
* Thread Safe quando aplicável

---

# Tarefas Iniciais

1. Ler PROJECT_GUIDELINES.md.
2. Analisar SmartCoreHub.Localization.SDK.
3. Criar solução SmartCoreHub.Core.SDK.
4. Criar projeto de testes.
5. Configurar empacotamento NuGet.
6. Implementar Domain Foundation.
7. Implementar Result Pattern.
8. Implementar Helpers.
9. Implementar Validation.
10. Implementar Extensions.
11. Implementar Infrastructure Base.
12. Implementar testes.
13. Validar build e pack.
14. Gerar documentação XML.
15. Gerar README inicial.
 



 Esta é uma excelente iniciativa. Para que o **Cursor IA** (ou qualquer LLM de engenharia de software) crie exatamente o que você precisa, sem alucinar ou gerar códigos desconexos, a especificação precisa ser **ultra-estruturada, pragmática e focar nas interfaces e padrões arquiteturais**.

Abaixo, transformei e melhorei o seu rascunho em um **Prompt/Especificação de Engenharia Pronto para o Cursor**. Removi ambiguidades, unifiquei as duas listas que você enviou, adicionei exemplos de código idiomáticos modernos (C# 10+) e estruturei o documento para que a IA trabalhe em módulos lógicos.

---

# 📑 SmartCoreHub.Core.SDK — Especificação Técnica de Engenharia

**Status:** Pronto para Implementação (Cursor IA)

**Contexto:** Criação de uma nova Class Library utilitária e genérica (.NET 8/9), empacotável via NuGet.

> ⚠️ **PREMISSA CRÍTICA (NÃO NEGOCIÁVEL):**
> * **NÃO** mova, altere ou remova código de nenhum projeto existente na Solution.
> * Trata-se de um projeto **100% NOVO** e isolado em `CORE/SmartCoreHub.Core.SDK`.
> * Antes de escrever código, o Cursor deve analisar o arquivo `PROJECT_GUIDELINES.md` e o `.csproj` de `SmartCoreHub.Localization.SDK` para clonar estritamente as configurações de metadados, implicit usings, nullable contexts e empacotamento NuGet.
> 
> 

---

## 1. Estrutura de Arquivos e Namespaces

O projeto deve seguir estritamente a convenção de nomenclatura `SmartCoreHub.Core.SDK.[Módulo]`. A estrutura de pastas no disco deve mapear os namespaces:

```text
CORE/
 ├─ SmartCoreHub.Core.SDK/
 │   ├─ SmartCoreHub.Core.SDK.csproj
 │   ├─ Abstractions/          # Interfaces base globais
 │   ├─ Domain/                # Primitivas DDD (Entity, ValueObject, AggregateRoot)
 │   ├─ Infrastructure/        # Contratos de Persistência e Transação
 │   ├─ Services/              # Abstrações de Orquestração, Cache e Retry
 │   ├─ Validation/            # Guard Clauses e Fluent Validation Helpers
 │   ├─ Helpers/               # Classes utilitárias estáticas (String, DateTime, etc)
 │   ├─ Extensions/            # Métodos de extensão (.NET primitives & DI)
 │   ├─ Common/                # Result Pattern, Errors, Exceptions, Constants
 │   └─ Logging/               # Abstrações e Enriquecedores de Log
 │
 └─ SmartCoreHub.Core.SDK.Tests/
     ├─ SmartCoreHub.Core.SDK.Tests.csproj
     └─ [Módulo]/              # Testes espelhando a estrutura do projeto principal

```

---

## 2. Modelos de Implementação de Referência (Design Blueprints)

Para garantir que o Cursor utilize padrões modernos e performáticos, utilize os designs abaixo como guia de geração:

### 2.1. Common (Result Pattern & Errors)

Substituir o lançamento de exceções de fluxo por um Result Pattern fortemente tipado e performático.

```csharp
namespace SmartCoreHub.Core.SDK.Common;

public record Error(string Code, string Message, Dictionary<string, object>? Metadata = null);

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<Error> Errors { get; }

    protected Result(bool isSuccess, IEnumerable<Error>? errors = null)
    {
        IsSuccess = isSuccess;
        Errors = errors?.ToList().AsReadOnly() ?? Array.Empty<Error>();
    }

    public static Result Success() => new(true);
    public static Result Failure(Error error) => new(false, new[] { error });
    public static Result Failure(IEnumerable<Error> errors) => new(false, errors);
}

public class Result<T> : Result
{
    private readonly T? _value;
    public T Value => IsSuccess ? _value! : throw new InvalidOperationException("Não é possível acessar o valor de um resultado falho.");

    private Result(T? value, bool isSuccess, IEnumerable<Error>? errors = null) : base(isSuccess, errors)
    {
        _value = value;
    }

    public static Result<T> Success(T value) => new(value, true);
    public static new Result<T> Failure(Error error) => new(default, false, new[] { error });
    public static new Result<T> Failure(IEnumerable<Error> errors) => new(default, false, errors);
}

```

### 2.2. Domain Primitives (DDD)

Suporte completo a DDD com tipos imutáveis e controle de ciclo de vida.

```csharp
namespace SmartCoreHub.Core.SDK.Domain;

public interface IEntity { Guid Id { get; } }

public abstract class EntityBase : IEntity, IEquatable<EntityBase>
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsActive { get; protected set; } = true;

    public void UpdateModifiedDate() => UpdatedAt = DateTime.UtcNow;
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;

    public bool Equals(EntityBase? other) => other is not null && Id.Equals(other.Id);
    public override bool Equals(object? obj) => obj is EntityBase other && Equals(other);
    public override int GetHashCode() => Id.GetHashCode();
}

public abstract class AuditableEntity : EntityBase
{
    public string CreatedBy { get; protected set; } = string.Empty;
    public string? UpdatedBy { get; protected set; }
    public string? DeletedBy { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }

    public void SetCreation(string user) => CreatedBy = user;
    public void SetModification(string user) { UpdatedBy = user; UpdateModifiedDate(); }
    public void SoftDelete(string user) { DeletedBy = user; DeletedAt = DateTime.UtcNow; IsActive = false; }
}

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;
        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }
    public override int GetHashCode() => GetEqualityComponents().Select(x => x?.GetHashCode() ?? 0).Aggregate((x, y) => x ^ y);
}

```

### 2.3. Validation (Guard Clauses)

Componente defensivo para evitar dados inconsistentes logo na entrada de construtores ou métodos.

```csharp
namespace SmartCoreHub.Core.SDK.Validation;

public static class Guard
{
    public static void AgainstNull([NotNull] object? argument, string argumentName)
    {
        if (argument is null)
            throw new ArgumentNullException(argumentName, $"{argumentName} não pode ser nulo.");
    }

    public static void AgainstEmptyString(string argument, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(argument))
            throw new ArgumentException($"{argumentName} não pode ser vazio ou conter apenas espaços.", argumentName);
    }

    public static void AgainstNegative(decimal argument, string argumentName)
    {
        if (argument < 0)
            throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} não pode ser negativo.");
    }
}

```

---

## 3. Especificação Detalhada dos Módulos Utilizários

O Cursor deve implementar as seguintes classes utilitárias e métodos específicos:

### 3.1 Helpers & Extensions

* **`StringHelper` & `StringExtensions**`:
* `NormalizeText()`: Remove acentos e caracteres especiais.
* `Slugify()`: Transforma strings em slugs de URL (ex: `Olá Mundo!` ➡️ `ola-mundo`).
* `ToSnakeCase()`, `ToCamelCase()`, `ToPascalCase()`.
* `MaskDocument()`: Mascara CPF (`***.###.###-`) ou CNPJ (`.***.###/####-`).


* **`DateTimeHelper` & `DateTimeExtensions**`:
* `ToUserTimeZone(DateTime utcTime, string timeZoneId)`.
* `StartOfDay()`, `EndOfDay()`.
* `CalculateAge(DateTime birthDate)`.
* `AddBusinessDays(DateTime start, int days)`: Ignora sábados e domingos.



### 3.2 Infrastructure (Persistência Genérica)

* Interfaces puras desvinculadas de ORM (EF/Dapper):
* `IRepository<T> where T : EntityBase` contendo assinaturas assíncronas padrão (`GetByIdAsync`, `ListAsync`, `AddAsync`, `UpdateAsync`, `DeleteAsync`).
* `IUnitOfWork` contendo `Task<int> CommitAsync(CancellationToken cancellationToken = default)`.


* **`InMemoryRepository<T>`**: Implementação baseada em `ConcurrentDictionary<Guid, T>` confinada exclusivamente ao projeto de **Testes Unitários** para mock rápido de repositórios.

### 3.3 Logging Padrão Enterprise

* **`ILoggerAdapter<T>`**: Wrapper sobre `Microsoft.Extensions.Logging.ILogger`.
* Inclusão automática nos escopos de log (`BeginScope`) de: `CorrelationId`, `RequestId` e `TraceId`.
* Sanitização de Logs: Interceptador genérico baseado em Attributes (`[SensitiveData]`) para substituir valores de propriedades como *passwords, tokens, credit cards* pela string `[REDACTED]` antes da serialização ou escrita do log.

### 3.4 Abstrações de Infra (Mappers & Caching)

* **`ISmartCoreHubMapper`**: Interface genérica contendo `TDestination Map<TSource, TDestination>(TSource source)`. Sem dependência de bibliotecas terceiras no Core SDK (A aplicação final injetará a implementação concreta via Mapster ou AutoMapper).
* **`ICacheProvider`**: Interface assíncrona pura para operações `GetAsync<T>`, `SetAsync<T>`, `RemoveAsync`. Fornecer uma implementação padrão `MemoryCacheProvider` interna utilizando `IMemoryCache`.

---

## 4. Estratégia de Testes Unitários

O projeto `SmartCoreHub.Core.SDK.Tests` deve ser criado utilizando:

* **Framework:** `xUnit`
* **Biblioteca de Asserts:** `FluentAssertions`
* **Isolamento:** `Moq` ou `NSubstitute`

### Diretrizes de Cobertura (Meta: >= 90%)

1. **Helpers e Extensions:** Testar exaustivamente com matrizes de dados de entrada (`[Theory]` e `[InlineData]`).
2. **Result Pattern:** Testar comportamentos de sucesso, falha e segurança ao tentar acessar `.Value` em falhas.
3. **Guard Clauses:** Validar se as exceções corretas são disparadas sob condições inválidas.
4. **Specifications:** Testar filtros lógicos aplicados sobre coleções em memória (`IQueryable`).

**Padrão de Nomeação das Classes de Teste:**
`NomeDoMetodo_CenarioDeTeste_ComportamentoEsperado`

*Exemplo:* `Slugify_StringWithAccentsAndSymbols_ReturnsCleanKebabCase`

---

## 5. Configuração do `.csproj` e NuGet (Configuração Espelho)

O arquivo `SmartCoreHub.Core.SDK.csproj` deve clonar exatamente as propriedades do SDK de localização da empresa. Certifique-se de configurar:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFrameworks>net8.0;net9.0</TargetFrameworks>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>

  <PropertyGroup>
    <PackageId>SmartCoreHub.Core.SDK</PackageId>
    <Authors>SuaEmpresa / Core Team</Authors>
    <Company>SmartCoreHub</Company>
    <Description>Biblioteca centralizadora de primitivas de domínio, helpers e utilitários reaproveitáveis da arquitetura SmartCoreHub.</Description>
    <PackageTags>core;sdk;shared;helpers;ddd;result-pattern</PackageTags>
    <RepositoryUrl>https://github.com/seu-repositorio/smartcorehub-backend</RepositoryUrl>
    <PublishRepositoryUrl>true</PublishRepositoryUrl>
    <EmbedUntrackedSources>true</EmbedUntrackedSources>
    <IncludeSymbols>true</IncludeSymbols>
    <SymbolPackageFormat>snupkg</SymbolPackageFormat>
  </PropertyGroup>

</Project>

```

---

## 6. Roteiro de Execução Incremental para o Cursor IA

*Instrução para o Cursor: Execute as etapas uma por uma, validando o build e criando os testes unitários da etapa correspondente antes de avançar.*

* [ ] **Passo 1:** Analisar `PROJECT_GUIDELINES.md` e as configurações do projeto `SmartCoreHub.Localization.SDK`.
* [ ] **Passo 2:** Criar a estrutura de diretórios e os dois projetos `.csproj` vazios com as configurações descritas na Seção 5.
* [ ] **Passo 3:** Implementar o módulo `Common` (`Result`, `Error`, `Exceptions`) + Testes Unitários.
* [ ] **Passo 4:** Implementar o módulo `Domain` (`EntityBase`, `ValueObject`, `AuditableEntity`) + Testes Unitários.
* [ ] **Passo 5:** Implementar `Validation` (`Guard`) e os `Helpers` com testes exaustivos via `[Theory]`.
* [ ] **Passo 6:** Criar as abstrações e interfaces de `Infrastructure`, `Services`, `Logging` e `Mapping`.
* [ ] **Passo 7:** Validar a geração de documentação XML de compilação e garantir que o comando `dotnet pack` execute com sucesso sem warnings arquiteturais.

---

### 💡 Por que essa especificação ficou melhor?

1. **Redução de Escopo Ambíguo:** O rascunho anterior tentava listar tudo o que existe no ecossistema de software de uma vez. Aqui, as responsabilidades estão bem delimitadas (ex: O Mapper e o Cache são apenas *interfaces/abstrações*, impedindo a IA de trazer dependências pesadas de terceiros para dentro do seu SDK Core).
2. **Código de Exemplo Moderno:** Apresentar Records e sintaxe C# moderna dita o tom estético e de performance que você espera do código gerado pela IA.
3. **Instruções Imperativas:** O uso de termos fortes como *"NÃO NEGOCIÁVEL"* e *"Espelho"* impede desvios de padrão pelo Cursor.