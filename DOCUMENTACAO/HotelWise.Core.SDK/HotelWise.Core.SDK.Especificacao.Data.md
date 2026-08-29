# Especificação Técnica — HotelWise.Core.SDK (Módulo Data)

**Versão:** 2.0.0  
**Data:** 2026-08-28  
**Projeto de Origem:** `HotelWise.Data` (`HotelWise.Data.csproj`, TFM `net10.0`)  
**Projeto de Destino:** `HotelWise.Core.SDK`  
**Documento Principal:** [HotelWise.Core.SDK.Levantamento.md](./HotelWise.Core.SDK.Levantamento.md)

---

## 1. Papel do Módulo na Arquitetura

O projeto `HotelWise.Data` é a camada de persistência da solução, utilizando **Entity Framework Core** com o provedor **Pomelo.EntityFrameworkCore.MySql**. Referencia `HotelWise.Domain`.

O papel desta especificação é definir a extração dos padrões genéricos de acesso a dados para o `HotelWise.Core.SDK`, mantendo no host apenas o DbContext concreto, migrations, mapeamentos fluentes, mock data e repositórios de domínio.

### Dependências NuGet relevantes do .csproj

| Pacote | Impacto no Core.SDK |
| :--- | :--- |
| `Microsoft.EntityFrameworkCore` | Dependência condicional por TFM no Core |
| `Microsoft.EntityFrameworkCore.Relational` | Idem |
| `Pomelo.EntityFrameworkCore.MySql` | **Apenas no host** — provedor de banco concreto |
| `Bogus` | **Apenas no host** — geração de mock data |
| `Azure.Data.Tables`, `Azure.Identity` | Cloud abstractions |

---

## 2. Inventário Completo — Arquivos .cs (excluindo /obj/ e migrations)

> Total: **16 arquivos** fonte (excluindo 21 migrations e 3 arquivos obj).

---

### 2.1 Portar + Obsoletar → Core.SDK (4 arquivos)

| Arquivo | Tipo | Destino Core.SDK | Namespace Canônico | DiagnosticId |
| :--- | :--- | :--- | :--- | :--- |
| `Repository/Generic/GenericRepositoryBase.cs` | `GenericRepositoryBase<T, TContext>` | `Infrastructure/` | `HotelWise.Core.SDK.Infrastructure` | `HW_CORE_SDK_REPO` |
| `Context/Configure/Helper/ModelBuilderExtensions.cs` | `ModelBuilderExtensions` | `Extensions/` | `HotelWise.Core.SDK.Extensions` | `HW_CORE_SDK_REPO` |
| `Context/Configure/Helper/HelperCharSet.cs` | `HelperCharSet` | `Infrastructure/` | `HotelWise.Core.SDK.Infrastructure` | `HW_CORE_SDK_REPO` |
| `Context/Configure/Helper/ConfigurationEntitiesHelper.cs` | `ConfigurationEntitiesHelper` | `Infrastructure/` | `HotelWise.Core.SDK.Infrastructure` | `HW_CORE_SDK_REPO` |

---

### 2.2 Manter no Host (12 arquivos)

#### DbContext concreto (1 arquivo)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `Context/HotelWiseDbContextMysql.cs` | `HotelWiseDbContextMysql` | DbContext concreto de produção com DbSets de `Hotel`, `Room`, `Reservation`, `RoomAvailability`, `User`, `ChatSessionHistory` |

#### Mapeamentos Fluent API (6 arquivos)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `Context/Configure/Entity/ChatSessionHistoryConfiguration.cs` | `ChatSessionHistoryConfiguration` | Mapeamento EF de `ChatSessionHistory` |
| `Context/Configure/Entity/HotelModelConfigurations/HotelConfiguration.cs` | `HotelConfiguration` | Mapeamento EF de `Hotel` |
| `Context/Configure/Entity/HotelModelConfigurations/ReservationConfiguration.cs` | `ReservationConfiguration` | Mapeamento EF de `Reservation` |
| `Context/Configure/Entity/HotelModelConfigurations/RoomAvailabilityConfiguration.cs` | `RoomAvailabilityConfiguration` | Mapeamento EF de `RoomAvailability` |
| `Context/Configure/Entity/HotelModelConfigurations/RoomConfiguration.cs` | `RoomConfiguration` | Mapeamento EF de `Room` |
| `Context/Configure/Entity/UserConfiguration.cs` | `UserConfiguration` | Mapeamento EF de `User` |

#### Mock Data / Seed (3 arquivos)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `Context/Configure/Mock/HotelsMockData.cs` | `HotelsMockData` | Seed de hotéis para desenvolvimento |
| `Context/Configure/Mock/RoomsMockData.cs` | `RoomsMockData` | Seed de quartos para desenvolvimento |
| `Context/Configure/Mock/UserMockData.cs` | `UserMockData` | Seed de usuários para desenvolvimento |

#### Repositórios de Domínio (6 arquivos)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `Repository/HotelRepositories/HotelRepository.cs` | `HotelRepository` | Persistência de hotéis |
| `Repository/HotelRepositories/RoomRepository.cs` | `RoomRepository` | Persistência de quartos |
| `Repository/HotelRepositories/ReservationRepository.cs` | `ReservationRepository` | Persistência de reservas |
| `Repository/HotelRepositories/RoomAvailabilityRepository.cs` | `RoomAvailabilityRepository` | Persistência de disponibilidade |
| `Repository/UserRepository.cs` | `UserRepository` | Persistência e autenticação de usuários |
| `Repository/ChatSessionHistoryRepository.cs` | `ChatSessionHistoryRepository` | Persistência de sessões de chat |

#### Migrations MySQL (armazenadas, não contabilizadas individualmente)

Pasta `Migrations/MySql/` contém **11 migrations** (InitialCreate → SeedRoomPosDotNet10 → FixUserMockSeedPosDotNet10) + `HotelWiseDbContextMysqlModelSnapshot.cs`. Todas permanecem intocadas no host.

---

## 3. Detalhamento Técnico — `GenericRepositoryBase<T, TContext>`

### Implementação Atual

Localizado em `HotelWise.Data.Repository.Generic.GenericRepositoryBase<T, TContext>`:

```csharp
namespace HotelWise.Core.SDK.Infrastructure
{
    using HotelWise.Core.SDK.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;

    public abstract class GenericRepositoryBase<T, TContext> : IGenericRepository<T>
        where T : class
        where TContext : DbContext
    {
        protected readonly TContext _context;
        protected readonly DbSet<T> _dataset;
        private readonly DbContextOptions<TContext>? _options;

        protected GenericRepositoryBase(TContext context, DbContextOptions<TContext>? options = null)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dataset = _context.Set<T>();
            _options = options;
        }

        protected TContext CreateContext()
        {
            if (_options == null)
                throw new InvalidOperationException("DbContextOptions was not provided.");
            return (TContext)Activator.CreateInstance(typeof(TContext), _options)!;
        }

        public virtual async Task<List<T>> GetAllAsync()
            => await _dataset.AsNoTracking().ToListAsync();

        public virtual async Task<T?> GetByIdAsync(long id)
            => await _dataset.FindAsync(id);

        public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dataset.Where(predicate).ToListAsync();

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dataset.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dataset.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dataset.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dataset.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(long id)
        {
            var entity = await _dataset.FindAsync(id);
            if (entity != null)
            {
                _dataset.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<int> CountAsync()
            => await _dataset.CountAsync();

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
            => await _dataset.AsNoTracking().AnyAsync(predicate);

        public virtual async Task<List<T>> FetchAsync(int offset, int limit)
            => await _dataset.AsNoTracking().Skip(offset).Take(limit).ToListAsync();
    }

    // Sobrecarga de conveniência com DbContext base
    public abstract class GenericRepositoryBase<T> : GenericRepositoryBase<T, DbContext>
        where T : class
    {
        protected GenericRepositoryBase(DbContext context, DbContextOptions<DbContext>? options = null)
            : base(context, options) { }
    }
}
```

### Helpers EF Core

- **`ModelBuilderExtensions.cs`**: Convenções globais de nomenclatura, filtros, conversões DateTime → UTC, aplicação em massa de configurações fluentes.
- **`HelperCharSet.cs`**: CharSet padrão (`utf8mb4`), Collation, constantes de charset agnósticas.
- **`ConfigurationEntitiesHelper.cs`**: Convenções de chave primária/estrangeira, aplicação padronizada de mapeamentos.

---

## 4. Padrão de Shim no Host

```csharp
namespace HotelWise.Data.Repository.Generic
{
    using Microsoft.EntityFrameworkCore;

    // ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Infrastructure.GenericRepositoryBase<T, TContext>.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_REPO")]
    public abstract class GenericRepositoryBase<T, TContext>
        : HotelWise.Core.SDK.Infrastructure.GenericRepositoryBase<T, TContext>
        where T : class
        where TContext : DbContext
    {
        protected GenericRepositoryBase(TContext context, DbContextOptions<TContext> options)
            : base(context, options) { }
    }
}
```

---

## 5. Adaptação dos Repositórios de Domínio

Os repositórios concretos atualizam herança para usar o Core diretamente:

```csharp
using HotelWise.Core.SDK.Infrastructure;   // GenericRepositoryBase<T, TContext>
using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository.HotelRepositories
{
    public class HotelRepository
        : GenericRepositoryBase<Hotel, HotelWiseDbContextMysql>, IHotelRepository
    {
        public HotelRepository(
            HotelWiseDbContextMysql context,
            DbContextOptions<HotelWiseDbContextMysql> options)
            : base(context, options) { }

        // Métodos específicos de domínio...
    }
}
```

---

## 6. Plano de Testes (`HotelWise.Core.SDK.Tests`)

| Suite | Foco | Estratégia |
| :--- | :--- | :--- |
| `GenericRepositoryBaseTests.cs` | CRUD completo (`Add`, `AddRange`, `GetById`, `GetAll`, `Find`, `Update`, `UpdateRange`, `Delete`, `Count`, `Exists`, `Fetch`) | SQLite in-memory ou `Microsoft.EntityFrameworkCore.InMemory` |
| `GenericRepositoryBaseTests.cs` | Tratamento de `null` → `ArgumentNullException` | Caso negativo |
| `GenericRepositoryBaseTests.cs` | `CreateContext` sem options → `InvalidOperationException` | Caso negativo |
| `ModelBuilderExtensionsTests.cs` | Validação de aplicação de convenções | `ModelBuilder` mockado |
| `HelperCharSetTests.cs` | Constantes de charset/collation | Comparação de strings |

---

## 7. Checklist de Implementação

- [ ] Criar classe canônica `GenericRepositoryBase<T, TContext>` + `GenericRepositoryBase<T>` em `HotelWise.Core.SDK/Infrastructure/`
- [ ] Criar `ModelBuilderExtensions.cs` em `HotelWise.Core.SDK/Extensions/`
- [ ] Criar `HelperCharSet.cs` e `ConfigurationEntitiesHelper.cs` em `HotelWise.Core.SDK/Infrastructure/`
- [ ] Adicionar `[Obsolete]` e shim fino em 4 arquivos de `HotelWise.Data`
- [ ] Adicionar `ProjectReference` para `HotelWise.Core.SDK` em `HotelWise.Data.csproj`
- [ ] Atualizar `using`s nos 6 repositórios de domínio
- [ ] Implementar suíte de testes de persistência em `HotelWise.Core.SDK.Tests`
- [ ] `dotnet build HotelWise.Data/HotelWise.Data.csproj` verde
