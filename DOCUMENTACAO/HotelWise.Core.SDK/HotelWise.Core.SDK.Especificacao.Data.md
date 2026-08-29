# Especificação Técnica — HotelWise.Core.SDK (Módulo Data)

**Versão:** 1.0.0  
**Data:** 2026-08-28  
**Projeto de Origem:** `HotelWise.Data` (`HotelWise.Data.csproj`)  
**Projeto de Destino:** `HotelWise.Core.SDK`  
**Documento Principal:** [HotelWise.Core.SDK.Levantamento.md](./HotelWise.Core.SDK.Levantamento.md)

---

## 1. Papel do Módulo na Arquitetura

O projeto `HotelWise.Data` é a camada de persistência e acesso a dados relacionais da solução HotelWiseAPI, utilizando **Entity Framework Core 9** com o provedor **Pomelo.EntityFrameworkCore.MySql**.

O papel desta especificação é definir a extração dos padrões genéricos de acesso a dados (`GenericRepositoryBase<T, TContext>`) e utilitários de configuração de modelo EF Core (`ModelBuilderExtensions`, `HelperCharSet`, `ConfigurationEntitiesHelper`) para o núcleo `HotelWise.Core.SDK`, mantendo no projeto host `HotelWise.Data` apenas os elementos estritamente vinculados ao banco de dados concreto da aplicação (DbContext, migrations, mapeamentos fluentes e repositórios de domínio).

---

## 2. Inventário de Tipos de `HotelWise.Data`

### 2.1 Tipos para Portar + Obsoletar (Canônicos no Core.SDK)

| Tipo Original | Caminho no Host | Caminho no Core.SDK | Namespace Canônico | DiagnosticId |
| :--- | :--- | :--- | :--- | :--- |
| `GenericRepositoryBase<T, TContext>` | `Repository/Generic/GenericRepositoryBase.cs` | `Infrastructure/GenericRepositoryBase.cs` | `HotelWise.Core.SDK.Infrastructure` | `HW_CORE_SDK_REPO` |
| `ModelBuilderExtensions` | `Context/Configure/Helper/ModelBuilderExtensions.cs` | `Extensions/ModelBuilderExtensions.cs` | `HotelWise.Core.SDK.Extensions` | `HW_CORE_SDK_REPO` |
| `HelperCharSet` | `Context/Configure/Helper/HelperCharSet.cs` | `Infrastructure/HelperCharSet.cs` | `HotelWise.Core.SDK.Infrastructure` | `HW_CORE_SDK_REPO` |
| `ConfigurationEntitiesHelper` | `Context/Configure/Helper/ConfigurationEntitiesHelper.cs` | `Infrastructure/ConfigurationEntitiesHelper.cs` | `HotelWise.Core.SDK.Infrastructure` | `HW_CORE_SDK_REPO` |

### 2.2 Tipos para Manter no Host (`HotelWise.Data`)

| Tipo / Pasta | Caminho no Host | Motivo da Permanência |
| :--- | :--- | :--- |
| `HotelWiseDbContextMysql` | `Context/HotelWiseDbContextMysql.cs` | DbContext concreto de produção com DbSets específicos da aplicação. |
| Mapeamentos Fluent API (`*Configuration.cs`) | `Context/Configure/Entity/*` | Mapeamento de tabelas específicas (`Hotel`, `Room`, `Reservation`, `User`, etc.). |
| Mocks de Seed (`HotelsMockData`, etc.) | `Context/Configure/Mock/*` | Dados de seed específicos para ambiente de desenvolvimento/testes do produto. |
| Migrations MySQL (`Migrations/MySql/*`) | `Migrations/MySql/*` | Histórico e snapshots de schema do banco de dados MySQL de produção. |
| `HotelRepository` | `Repository/HotelRepositories/HotelRepository.cs` | Regras de persistência de hotéis (herda a base genérica do Core). |
| `RoomRepository` | `Repository/HotelRepositories/RoomRepository.cs` | Consultas e comandos de quartos. |
| `ReservationRepository` | `Repository/HotelRepositories/ReservationRepository.cs` | Consultas e comandos de reservas de hotel. |
| `RoomAvailabilityRepository` | `Repository/HotelRepositories/RoomAvailabilityRepository.cs` | Consultas e comandos de disponibilidade de quartos. |
| `UserRepository` | `Repository/UserRepository.cs` | Consultas e autenticação de usuários. |
| `ChatSessionHistoryRepository` | `Repository/ChatSessionHistoryRepository.cs` | Histórico de sessões de chat com IA. |

---

## 3. Detalhamento Técnico das Extrações

### 3.1 `GenericRepositoryBase<T, TContext>`

#### Análise da Implementação Atual
Atualmente localizado em `HotelWise.Data.Repository.Generic.GenericRepositoryBase<T, TContext>`, o repositório fornece operações assíncronas padrão:
- `GetAllAsync()` (com `AsNoTracking()`)
- `GetByIdAsync(long id)`
- `FindAsync(Expression<Func<T, bool>> predicate)`
- `AddAsync(T entity)` / `AddRangeAsync(IEnumerable<T> entities)`
- `UpdateAsync(T entity)` / `UpdateRangeAsync(IEnumerable<T> entities)`
- `DeleteAsync(long id)`
- `CountAsync()`
- `ExistsAsync(Expression<Func<T, bool>> predicate)`
- `FetchAsync(int offset, int limit)`

#### Estratégia Canônica no Core.SDK
No Core.SDK, a classe será migrada com suporte tanto para `DbContext` genérico tipado quanto para o tipo base `Microsoft.EntityFrameworkCore.DbContext`:

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

### 3.2 Helpers de Configuração EF Core

#### `ModelBuilderExtensions.cs`
- Centraliza métodos utilitários para aplicar convenções globais de nomenclatura, filtros de soft-delete, conversões de DateTime para UTC e aplicação em massa de configurações fluentes.

#### `HelperCharSet.cs` & `ConfigurationEntitiesHelper.cs`
- Utilitários para definir CharSet padrão (ex: `utf8mb4`), Collation e convenções de chave primária/estrangeira de forma agnóstica a bancos relacionais.

---

## 4. Padrão de Shim no Host (`HotelWise.Data`)

O arquivo original em `HotelWise.Data/Repository/Generic/GenericRepositoryBase.cs` será mantido com a declaração de obsolescência e herança direta da classe canônica do Core:

```csharp
namespace HotelWise.Data.Repository.Generic
{
    using Microsoft.EntityFrameworkCore;
    using CoreRepo = HotelWise.Core.SDK.Infrastructure;

    // Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    [Obsolete("Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Infrastructure.GenericRepositoryBase<T, TContext>.", error: false, DiagnosticId = "HW_CORE_SDK_REPO")]
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

## 5. Adaptação dos Repositórios de Domínio

Os repositórios de domínio concretos (`HotelRepository`, `RoomRepository`, etc.) continuam em `HotelWise.Data`, porém atualizam suas importações para herdar diretamente de `HotelWise.Core.SDK.Infrastructure.GenericRepositoryBase`:

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

        // Métodos específicos de domínio...
    }
}
```

---

## 6. Plano de Testes (`HotelWise.Core.SDK.Tests`)

Para garantir cobertura $\ge 90\%$, o projeto de testes do SDK implementará:
1. **`GenericRepositoryBaseTests.cs`:**
   - Criação de um `DbContext` em memória (`Microsoft.EntityFrameworkCore.InMemory` ou SQLite in-memory).
   - Teste de todas as operações CRUD (`AddAsync`, `AddRangeAsync`, `GetByIdAsync`, `GetAllAsync`, `FindAsync`, `UpdateAsync`, `UpdateRangeAsync`, `DeleteAsync`, `CountAsync`, `ExistsAsync`, `FetchAsync`).
   - Teste de resiliência e tratamento de parâmetros nulos (`ArgumentNullException`).
2. **`ModelBuilderExtensionsTests.cs`:**
   - Validação da aplicação de convenções e extensões no `ModelBuilder`.
3. **`HelperCharSetTests.cs`:**
   - Validação das strings de formatação de charset/collation.

---

## 7. Checklist de Implementação e Validação

- [ ] Criar classe canônica `GenericRepositoryBase<T, TContext>` em `HotelWise.Core.SDK/Infrastructure/`.
- [ ] Criar `ModelBuilderExtensions.cs`, `HelperCharSet.cs` e `ConfigurationEntitiesHelper.cs` em `HotelWise.Core.SDK/`.
- [ ] Adicionar `[Obsolete]` e shim fino no arquivo `HotelWise.Data/Repository/Generic/GenericRepositoryBase.cs`.
- [ ] Adicionar `ProjectReference` para `HotelWise.Core.SDK` em `HotelWise.Data.csproj`.
- [ ] Atualizar `using`s nos repositórios de domínio (`HotelRepository`, `RoomRepository`, etc.).
- [ ] Implementar suíte de testes de persistência em `HotelWise.Core.SDK.Tests`.
- [ ] Executar `dotnet build HotelWise.Data/HotelWise.Data.csproj` e verificar ausência de erros.
