# Diretrizes para Ajuste de Issues e Code Smells — Backend (Genérico C# / .NET)

**Documento:** Guia operacional padronizado e reutilizável para qualidade estática e governança de código backend  
**Arquivo:** `Diretrizes-CodeSmell-Backend-Generico.md`  
**Escopo:** Soluções C# / .NET (APIs, bibliotecas de domínio, serviços de negócio, persistência, workers, SDKs, suítes de teste)  
**Ferramental de Referência:** SonarQube, SonarCloud, Roslyn Analyzers, .NET CLI, dotnet-format  
**Target Platform:** .NET 10 / C# 13+ (com suporte a multi-targeting `net8.0;net10.0`)  
**Data da Revisão:** 2026-08-28  

---

## 1. Objetivo

Padronizar e orientar o processo de identificação, diagnóstico e remediação de **Code Smells**, **Bugs**, **Vulnerabilidades** e **Security Hotspots** apontados por analisadores estáticos de código (SonarQube, SonarCloud, Roslyn Analyzers, dotnet format) em soluções C# / .NET, garantindo:

1. **Zero Regressão de Negócio:** Nenhuma refatoração para eliminação de Code Smell pode alterar o comportamento observável, regras de negócio ou contratos públicos de APIs (REST, gRPC, SignalR, MCP).
2. **Manutenibilidade e Legibilidade:** Elevação contínua do índice de manutenibilidade (*Maintainability Rating A*), eliminando duplicações, complexidade ciclomática excessiva e acoplamentos espúrios.
3. **Segurança e Confiabilidade:** Eliminação de brechas de segurança (injeções, manipulação insegura de recursos, vazamento de memória/conexões) e bugs latentes (null dereferences, chamadas incorretas de async/await, deadlocks, loops infinitos).
4. **Governança Ética de Warnings:** Vedação da supressão indiscriminada de regras via `#pragma warning disable` ou `[SuppressMessage]` sem justificativa arquitetural formal documentada.

---

## 2. Taxonomia de Issues do Sonar em .NET

```mermaid
flowchart TD
    Issue[Issue SonarQube / SonarCloud] --> CS[Code Smell\n(Manutenibilidade / Débito Técnico)]
    Issue --> Bug[Bug\n(Confiabilidade / Erro Latente)]
    Issue --> Vuln[Vulnerabilidade\n(Segurança / Brecha Imediata)]
    Issue --> Hotspot[Security Hotspot\n(Revisão de Segurança Necessária)]

    CS --> S1[Complexidade Ciclomática / Tamanho de Método]
    CS --> S2[Parâmetros Excessivos / DI Explosion]
    CS --> S3[Nomenclatura e Código Morto / Redundâncias]

    Bug --> B1[Null Pointer / Nullable Flow Analysis]
    Bug --> B2[Uso Incorreto de IDisposable / Async State Machine]
    Bug --> B3[Condições Booleanas Invariantes]

    Vuln --> V1[Injeção de SQL / Comandos]
    Vuln --> V2[Exposição de Informações Sensíveis / Credenciais]
    Vuln --> V3[Configurações Criptográficas / CORS Inseguros]
```

---

## 3. Catálogo das Regras Sonar C# Mais Comuns e Padrões de Correção

### 3.1 Manutenibilidade e Code Smells

| Regra Sonar | Descrição | Causa Típica | Solução Recomendada (.NET 10 / C# 13+) |
| ----------- | --------- | ------------ | -------------------------------------- |
| **`csharpsquid:S107`** | *Methods should not have too many parameters* | Construtores ou métodos com > 7 parâmetros (*DI Explosion*) | Agrupar dependências em objetos agregadores (*Parameter Object Pattern*, *Context Configurations* ou *Factories*). |
| **`csharpsquid:S112`** | *General exceptions should never be thrown* | Lançar `throw new Exception()` ou `throw new SystemException()` | Substituir por exceções especializadas de domínio (`ArgumentNullException`, `InvalidOperationException`, `BusinessException`, `NotFoundException`). |
| **`csharpsquid:S1135`** | *Track uses of "TODO" tags* | Comentários com `// TODO` esquecidos no código | Tratar o ponto pendente ou convertê-lo em issue no backlog do projeto, removendo o comentário solto. |
| **`csharpsquid:S1144`** / **`S1172`** | *Unused private types/methods/parameters* | Parâmetros ou métodos declarados que nunca são lidos/invocados | Remover membros privados não utilizados. Em interfaces/assinaturas públicas, avaliar se o parâmetro é parte do contrato obrigatório. |
| **`csharpsquid:S3236`** | *Caller information attributes should be used* | Passar nome de método ou arquivo manualmente em loggers | Usar atributos `[CallerMemberName]`, `[CallerFilePath]`, `[CallerLineNumber]`. |
| **`csharpsquid:S3928`** | *Parameter names should be passed correctly in ArgumentException* | `new ArgumentNullException("mensagem")` em vez do nome do parâmetro | Usar `ArgumentNullException.ThrowIfNull(param)` (.NET 8+) ou `nameof(parametro)` como identificador. |
| **`csharpsquid:S125`** | *Sections of code should not be commented out* | Blocos de código antigo comentados no arquivo | Remover código comentado (o histórico está preservado no controle de versão Git). |
| **`csharpsquid:S3260`** | *Non-public types without subclasses should be sealed* | Classes internas/privadas abertas sem necessidade | Adicionar o modificador `sealed` para permitir otimizações de desvirtualização no compilador e JIT. |
| **`csharpsquid:S6562`** | *Always use "DateTime.UtcNow" or "TimeProvider" instead of "DateTime.Now"* | Uso de `DateTime.Now` acoplado ao fuso local da máquina | Utilizar `TimeProvider` injetável ou `DateTime.UtcNow` para garantir determinismo e testabilidade. |

---

### 3.2 Confiabilidade e Bugs

| Regra Sonar | Descrição | Causa Típica | Solução Recomendada |
| ----------- | --------- | ------------ | ------------------- |
| **`csharpsquid:S2259`** | *Null pointers should not be dereferenced* | Acessar membros de um objeto que pode ser nulo sem checagem prévia | Utilizar pattern matching (`if (obj is not null)`), guard clauses (`ArgumentNullException.ThrowIfNull`) ou operador de navegação segura (`obj?.Property`). |
| **`csharpsquid:S2583`** | *Conditionally executed code should be reachable* | Condições booleanas redundantes que sempre avaliam para `true` ou `false` | Simplificar a expressão lógica eliminando checagens duplicadas ou variáveis de estado invariantes. |
| **`csharpsquid:S2953`** / **`S3881`** | *Methods named "Dispose" should implement IDisposable* | Classes com método `Dispose` sem implementar formalmente o padrão IDisposable | Implementar a interface `IDisposable` (e opcionalmente `IAsyncDisposable`), implementando o padrão `Dispose(bool disposing)`. |
| **`csharpsquid:S4457`** | *Parameter validation in async/iterator methods* | Validar argumentos dentro do state machine assíncrono | Dividir o método: método público síncrono para validação de argumentos + método privado local `async` para execução. |
| **`csharpsquid:S2933`** | *Fields that are only assigned in the constructor should be "readonly"* | Campos atribuídos apenas no construtor sem modificador `readonly` | Adicionar o modificador `readonly` ao campo privado. |
| **`csharpsquid:S3168`** | *"async void" methods should not be used* | Métodos assíncronos com retorno `void` (exceto event handlers de UI) | Alterar o retorno para `Task` ou `ValueTask` para permitir tratamento correto de exceções e await. |

---

### 3.3 Segurança e Vulnerabilidades

| Regra Sonar | Descrição | Causa Típica | Solução Recomendada |
| ----------- | --------- | ------------ | ------------------- |
| **`csharpsquid:S2077`** | *Formatting SQL queries is security-sensitive* | Concatenação direta de strings em comandos SQL (`$"SELECT * FROM ... WHERE Id = {id}"`) | Utilizar consultas parametrizadas com EF Core (`FromSqlInterpolated`), Dapper (`new { Id = id }`) ou `DbParameter`. |
| **`csharpsquid:S5144`** | *Server-Side Request Forgery (SSRF)* | Construir URLs HTTP para `HttpClient` a partir de inputs de usuário não validados | Validar e sanitizar URLs contra uma lista de permissões (*allowlist*) de hosts/domínios aceitos. |
| **`csharpsquid:S4502`** | *CSRF protection should not be disabled* | Desabilitar `[ValidateAntiForgeryToken]` ou `[IgnoreAntiforgeryToken]` em endpoints state-changing | Manter proteção CSRF ativa ou usar autenticação baseada em Bearer Tokens não associada a cookies em APIs REST stateless. |
| **`csharpsquid:S3330`** | *HttpOnly flag should be set for sensitive cookies* | Cookies de autenticação/sessão gerados sem `HttpOnly = true` | Configurar explicitamente `CookieOptions.HttpOnly = true` e `Secure = true`. |
| **`csharpsquid:S6437`** | *Hard-coded credentials should not be used* | Chaves de API, senhas ou connection strings em código-fonte | Utilizar `IConfiguration`, Azure Key Vault, User Secrets ou variáveis de ambiente. |

---

## 4. Fluxo Operacional de Saneamento Passo a Passo

```mermaid
flowchart TD
    P1[1. Extração de Relatório Sonar / Roslyn] --> P2[2. Triagem e Priorização\n(Vulnerabilidades > Bugs > Code Smells)]
    P2 --> P3[3. Diagnóstico e Causa Raiz]
    P3 --> P4[4. Aplicação da Refatoração Limpa]
    P4 --> P5[5. Validação Local de Build e Testes]
    P5 --> P6{Passou com 0 erros e 100% testes?}
    P6 -- Não --> P3
    P6 -- Sim --> P7[6. Reanálise Sonar e Emissão de Evidências]
```

### 4.1 Comandos de Diagnóstico e Validação (.NET CLI)

```powershell
# 1. Compilação Release determinística com análise de código ativa
dotnet build <Solucao>.sln -c Release /p:TreatWarningsAsErrors=false

# 2. Execução de formatador automático de código .NET
dotnet format <Solucao>.sln --verify-no-changes --verbosity diagnostic

# 3. Execução de toda a suíte de testes automatizados
dotnet test <Solucao>.sln -c Release --no-build

# 4. Execução de testes com cobertura para validação de não-regressão
dotnet test <Solucao>.sln -c Release /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

---

## 5. Checklist de Qualidade Obrigatório

- [ ] **Compilação Limpa**: `dotnet build -c Release` conclui com 0 erros e 0 warnings novos.
- [ ] **100% de Testes Verdes**: Todos os projetos de teste da solução passam sem quebras (`dotnet test`).
- [ ] **Preservação de Assinaturas Públicas**: Nenhuma alteração em interfaces públicas, DTOs de API ou contratos de integração.
- [ ] **Zero Supressões Não Justificadas**: Nenhum `#pragma warning disable` adicionado sem comentário explicativo e aprovação técnica.
- [ ] **Registro de Evidências**: Relação de arquivos alterados, regras Sonar resolvidas e status do Quality Gate.

---

## 6. Template de Registro de Evidências

Ao finalizar o lote de correções, documentar no relatório de entrega:

```text
================================================================================
RELATÓRIO DE SANEAMENTO DE CODE SMELLS (BACKEND)
================================================================================
Data: AAAA-MM-DD
Solução: <NomeSolucao>.sln

1. Sumário de Issues Resolvidas:
   - Vulnerabilidades / Segurança: 0 pendentes
   - Bugs Latentes: 0 pendentes
   - Code Smells de Manutenibilidade: N corrigidos

2. Principais Regras Saneadas:
   - S107 (Parameter count): Refatorado via Parameter Object / Context Configs em X classes
   - S112 (Generic exceptions): Substituído por exceções de domínio especializadas
   - S2259 (Null dereference): Adicionadas guard clauses e pattern matching
   - S3260 (Private sealed): Seladas N classes internas
   - S6562 (DateTime): Migrado para TimeProvider / UtcNow

3. Validação:
   - Build Release: 0 erros / 0 warnings
   - Testes Automatizados: N / N aprovados (100%)
================================================================================
```
