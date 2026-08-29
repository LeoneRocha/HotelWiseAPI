# Plano de Implementação — Onda 2: HotelWise.Data → Core.SDK

**Versão:** 1.0.0  
**Data:** 2026-08-28  
**Plano Geral:** [HotelWise.Core.SDK.PlanoImplementacao.md](./HotelWise.Core.SDK.PlanoImplementacao.md)  
**Especificação:** [HotelWise.Core.SDK.Especificacao.Data.md](./HotelWise.Core.SDK.Especificacao.Data.md)  
**Pré-requisito:** Onda 1 (Domain) concluída — contratos canônicos e interfaces disponíveis no Core.SDK.

---

## Resumo

| Métrica | Valor |
| :--- | :--- |
| Arquivos a portar | **4** |
| Arquivos mantidos no host | **12** (+ 11 migrations MySQL) |
| Lotes sequenciais | **1** (Lote A1) |
| Estimativa | **1 dia** (1 dev) |

---

## Lote A1 — Repositório Genérico EF Core e Extensões de Persistência

**Dependências:** Onda 1 (Domain) concluída (`IGenericRepository<T>`, `IEntityBase`, `ETypeDataBase`, etc.)  
**Arquivos a portar:** 4  
**Repositórios de domínio a atualizar:** 6

---

### 1. Tarefas Detalhadas

| # | Ação | Arquivo Origem no Host | Destino no Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- | :--- |
| 1.1 | Portar classe canônica | `Repository/Generic/GenericRepositoryBase.cs` | `Infrastructure/GenericRepositoryBase.cs` | `HW_CORE_SDK_REPO` |
| 1.2 | Portar classe estática | `Context/Configure/Helper/ModelBuilderExtensions.cs` | `Extensions/ModelBuilderExtensions.cs` | `HW_CORE_SDK_REPO` |
| 1.3 | Portar classe estática | `Context/Configure/Helper/HelperCharSet.cs` | `Infrastructure/HelperCharSet.cs` | `HW_CORE_SDK_REPO` |
| 1.4 | Portar classe estática | `Context/Configure/Helper/ConfigurationEntitiesHelper.cs` | `Infrastructure/ConfigurationEntitiesHelper.cs` | `HW_CORE_SDK_REPO` |
| 1.5 | Adicionar `ProjectReference` | `HotelWise.Data/HotelWise.Data.csproj` | Referência para `HotelWise.Core.SDK` | — |
| 1.6 | Criar Shims `[Obsolete]` | 4 arquivos originais em `HotelWise.Data` | Casca fina delegando ao Core | `HW_CORE_SDK_REPO` |
| 1.7 | Atualizar herança nos repositórios concretos | 6 repositórios de domínio em `HotelWise.Data` | Herdar de `HotelWise.Core.SDK.Infrastructure.GenericRepositoryBase` | — |
| 1.8 | Validar compilação | Solução inteira (`HotelWiseAPI.sln`) | Build 100% verde | — |
| 1.9 | Implementar suíte de testes | `HotelWise.Core.SDK.Tests/Infrastructure/` | Testes canônicos de persistência | — |

---

### 2. Implementação Canônica no Core.SDK

No arquivo `HotelWise.Core.SDK/Infrastructure/GenericRepositoryBase.cs`:

```csharp
namespace HotelWise.Core.SDK.Infrastructure
{
    using HotelWise.Core.SDK.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

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
            {
                throw new InvalidOperationException("DbContextOptions was not provided.");
            }
            return (TContext)Activator.CreateInstance(typeof(TContext), _options)!;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dataset.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(long id)
        {
            return await _dataset.FindAsync(id);
        }

        public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dataset.Where(predicate).ToListAsync();
        }

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
        {
            return await _dataset.CountAsync();
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dataset.AsNoTracking().AnyAsync(predicate);
        }

        public virtual async Task<List<T>> FetchAsync(int offset, int limit)
        {
            return await _dataset.AsNoTracking().Skip(offset).Take(limit).ToListAsync();
        }
    }

    // Sobrecarga de conveniência utilizando DbContext base
    public abstract class GenericRepositoryBase<T> : GenericRepositoryBase<T, DbContext>
        where T : class
    {
        protected GenericRepositoryBase(DbContext context, DbContextOptions<DbContext>? options = null)
            : base(context, options)
        {
        }
    }
}
```

---

### 3. Padrão de Shim no Host (`HotelWise.Data`)

No arquivo `HotelWise.Data/Repository/Generic/GenericRepositoryBase.cs`:

```csharp
namespace HotelWise.Data.Repository.Generic
{
    using Microsoft.EntityFrameworkCore;
    using CoreRepo = HotelWise.Core.SDK.Infrastructure;

    // ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Infrastructure.GenericRepositoryBase<T, TContext>.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_REPO")]
    public abstract class GenericRepositoryBase<T, TContext> : CoreRepo.GenericRepositoryBase<T, TContext>
        where T : class
        where TContext : DbContext
    {
        protected GenericRepositoryBase(TContext context, DbContextOptions<TContext> options)
            : base(context, options)
        {
        }
    }
}
```

---

### 4. Atualização dos Repositórios Concretos no Host

Os 6 repositórios de domínio passam a herdar diretamente do Core.SDK:

1. `Repository/HotelRepositories/HotelRepository.cs`
2. `Repository/HotelRepositories/RoomRepository.cs`
3. `Repository/HotelRepositories/ReservationRepository.cs`
4. `Repository/HotelRepositories/RoomAvailabilityRepository.cs`
5. `Repository/UserRepository.cs`
6. `Repository/ChatSessionHistoryRepository.cs`

**Exemplo de adaptação:**
```csharp
using HotelWise.Core.SDK.Infrastructure;
using HotelWise.Data.Context;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository.HotelRepositories
{
    public class HotelRepository : GenericRepositoryBase<Hotel, HotelWiseDbContextMysql>, IHotelRepository
    {
        public HotelRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options) 
            : base(context, options)
        {
        }

        // Métodos específicos de domínio preservados integralmente...
    }
}
```

---

### 5. Itens Mantidos Intactos no Host

- **`HotelWiseDbContextMysql.cs`:** DbContext concreto com os DbSets de domínio.
- **`Context/Configure/Entity/*.cs`:** 6 configurações Fluent API (`HotelConfiguration`, `RoomConfiguration`, etc.).
- **`Context/Configure/Mock/*.cs`:** 3 arquivos de mock data (`HotelsMockData`, `RoomsMockData`, `UserMockData`).
- **`Migrations/MySql/*.cs`:** Todas as 11 migrations e o snapshot de modelo.

---

## 6. Testes Canônicos (`HotelWise.Core.SDK.Tests`)

| Arquivo de Teste | Escopo | Metodologia |
| :--- | :--- | :--- |
| `GenericRepositoryBaseTests.cs` | Operações CRUD completas (`Add`, `AddRange`, `GetById`, `GetAll`, `Find`, `Update`, `UpdateRange`, `Delete`, `Count`, `Exists`, `Fetch`) | EF Core InMemory / SQLite InMemory |
| `GenericRepositoryBaseTests.cs` | Validação de exceções e argumentos nulos | Casos negativos (`ArgumentNullException`, `InvalidOperationException`) |
| `ModelBuilderExtensionsTests.cs` | Aplicação de convenções no `ModelBuilder` | Teste com DbContext de teste |
| `HelperCharSetTests.cs` | Strings de charset e collation | Testes unitários com assertions |
| `ConfigurationEntitiesHelperTests.cs` | Utilitários de configuração de entidade | Testes unitários |

---

## 7. Critérios de Aceite da Onda 2

1. ✅ `GenericRepositoryBase<T, TContext>` e helpers de persistência residem em `HotelWise.Core.SDK`.
2. ✅ Shims `[Obsolete]` com `DiagnosticId = "HW_CORE_SDK_REPO"` adicionados aos 4 arquivos originais do host.
3. ✅ Todos os 6 repositórios de domínio herdam com sucesso da base canônica do SDK.
4. ✅ `dotnet build HotelWise.Data/HotelWise.Data.csproj` compila com **0 erros**.
5. ✅ Suíte de testes `GenericRepositoryBaseTests` atinge cobertura $\ge 90\%$.
6. ✅ Gate para Onda 3 (Service) liberado.
