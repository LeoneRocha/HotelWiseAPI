 

## Prompt — Migração Completa para SmartCoreHub.Core.SDK

> **Banner:** prompt **concluído / supersedido**. Genéricos consolidados no NuGet único; shims removidos. Ver [MigracaoGenericos.md](./SmartCoreHub.Core.SDK-MigracaoGenericos.md) e [Remocao-Shims.md](./SmartCoreHub.Core.SDK-Remocao-Shims.md). Não reexecutar este prompt como se a migração ainda estivesse pendente.
>
> **Complemento (2026-07-15):** extrações pendentes pós-migração executadas — ver [Extracao-Pendencias.md](./SmartCoreHub.Core.SDK-Extracao-Pendencias.md).

**Título:** Migração de Interfaces, Classes Genéricas, Repositórios, Adapters e Providers para SmartCoreHub.Core.SDK

**Objetivo:**  
- Revisar o documento **SmartCoreHub.Core.SDK-Substituicao.md**.  
- Criar no `SmartCoreHub.Core.SDK` todas as **interfaces, classes genéricas, repositórios, adapters e providers** que ainda não existem.  
- Obsoletar e aliasar os tipos originais em `Domain`, `Infrastructure` e `Service`.  
- Ajustar os projetos em `backend/Implementations` para consumir diretamente o SDK.  
- Validar build, testes e integração sem regressão funcional.  
- Validar EF Core com seed + migration para garantir integridade.

---

### Plano multi‑agente

1. **Revisão de classes genéricas**  
   - Conferir se já existem no SDK:  
     - `GenericRepository<TEntity>` (EF)  
     - `DapperAdapterGenericRepository<TEntity>`  
     - `GenericService<TEntity>`  
     - `ServiceResponse<T>` e DTOs comuns  
     - `CacheProviders` (Memory, Redis, Disk, Mongo, Cosmos)  
     - `Helpers` (`ParallelOptionsHelper`, `JsonSerializerHelper`, etc.)  
     - `ValueObjects` (`ConnectionString`, `Email`, `Role`, enums)  
   - Criar no SDK qualquer classe genérica, adapter ou provider que ainda não exista.

2. **Obsolescência e alias**  
   - Marcar originais com `[Obsolete]`.  
   - Criar alias/shim herdando ou delegando para o tipo do SDK.  
   - Mensagens de obsolescência devem apontar para o namespace correto no SDK.

3. **Ajuste dos projetos Implementations**  
   - Alterar referências para consumir diretamente o SDK.  
   - Suprimir warnings temporários no `.csproj` apenas durante a transição.  
   - Garantir que classes específicas de domínio continuem em `Implementations`.

4. **Validação de build e testes**  
   - Restaurar dependências e compilar solução completa.  
   - Executar testes unitários e `SmartCoreHub.Core.SDK.Tests` com cobertura ≥ 90%.  
   - Replicar/adaptar testes dos tipos migrados para o SDK.

5. **Validação de integração**  
   - Subir APIs afetadas e validar endpoints de saúde.  
   - Buildar imagens Docker (`docker compose build`) e confirmar execução (`docker compose up`).  
   - Comparar comportamento observável antes/depois para garantir **zero regressão funcional**.

6. **Validação EF Core**  
   - Inserir ou alterar um registro mínimo no **seed** do EF Core.  
   - Executar comando:  
     - `dotnet ef migrations add TestMigration`  
     - `dotnet ef database update`  
   - Validar se a migration foi aplicada corretamente e se não houve quebra no EF.  
   - Confirmar que o banco está atualizado e consistente após o update.

7. **Revisão de lacunas**  
   - Decidir sobre `AuditableBaseEntity` vs `AuditableEntity`.  
   - Confirmar política de identificador (`long` vs `Guid`).  
   - Validar guard validators (FluentValidation) e documentar se permanecem fora da migração.

---

### Checklist final

- Todas as **interfaces, classes genéricas, repositórios, adapters e providers** implementadas no `SmartCoreHub.Core.SDK`.  
- Projetos em `Implementations` consumindo diretamente o SDK.  
- `[Obsolete]` aplicado com alias/shim nos originais.  
- Build completo sem erros ou warnings inesperados.  
- Testes unitários e cobertura ≥ 90%.  
- Smoke tests e APIs validados.  
- Docker build e runtime confirmados.  
- **EF Core migrations testadas com seed + update sem erros.**  
- Nenhuma regressão funcional detectada.  
- Lacunas documentadas e plano de ação definido.
 