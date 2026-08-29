# Diretrizes para Testes Automatizados e Cobertura (Coverage) — Backend (Genérico C# / .NET)

**Documento:** Guia operacional padronizado e reutilizável para engenharia de testes e metas de cobertura backend  
**Arquivo:** `Diretrizes-Coverage-Backend-Generico.md`  
**Escopo:** Soluções C# / .NET (APIs, Domain, Services, Repositories, SDKs)  
**Ferramental de Referência:** NUnit, xUnit, Moq, Moq.EntityFrameworkCore, Bogus, AwesomeAssertions, Coverlet, Testcontainers  
**Target Platform:** .NET 10 / C# 13+  
**Data da Revisão:** 2026-08-28  

---

## 1. Objetivo

Padronizar a criação, refatoração e manutenção de testes automatizados (unitários e de integração) em soluções C# / .NET, assegurando:

1. **Meta de Cobertura Total (100% de Linhas e Ramos em Lógica de Negócio):** Garantir que toda a lógica de negócio, ramificações condicionais (`if`, `switch`, *pattern matching*, ternários), manipulação de exceções e fluxos de dados estejam cobertos por testes determinísticos.
2. **Independência e Reprodutibilidade:** Cada teste deve ser autocontido, isolado, idempotente e sem efeitos colaterais em execuções paralelas.
3. **Clareza e Legibilidade:** Estrutura declarativa e autoexplicativa baseada no padrão **Arrange / Act / Assert (AAA)**.
4. **Massa de Dados Realista e Dinâmica:** Utilização do **Bogus** para geração de dados sintéticos ricos e testes de valores limite (*boundary testing*).
5. **Simulação Precisa de Dependências:** Utilização de **Moq** e **Moq.EntityFrameworkCore** para controle total de contratos, retornos assíncronos, simulação de falhas e verificação de interações (`Verify`).
6. **Asserções Fluentes e Seguras:** Adoção do **AwesomeAssertions** (licença permissiva Apache 2.0) e `Assert.Multiple` para asserções robustas e legíveis.

---

## 2. Padrões Obrigatórios de Escrita de Testes

### 2.1 Padrão de Nomenclatura dos Métodos (Inglês)
Todos os métodos de teste devem ser nomeados em inglês seguindo a convenção tripartite:
```text
NomeDoMetodo_CenarioSobTeste_ResultadoEsperado
```

**Exemplos Homologados:**
- `GetByIdAsync_WhenEntityExists_ReturnsMappedDto`
- `CreateAsync_WhenPayloadIsInvalid_ThrowsValidationException`
- `DeleteAsync_WhenEntityNotFound_ReturnsFalse`
- `ProcessReservationAsync_WithAvailableRoom_ConfirmsBookingAndDispatchesNotification`
- `ExecutePromptAsync_WhenLlmFails_AppliesPollyRetryAndThrowsServiceException`

---

### 2.2 Comentários de Contexto (Português)
Acima de cada método de teste, adicionar obrigatoriamente um bloco de comentário em português explicando o cenário e o objetivo do teste:

```csharp
// Cenário: Tentativa de recuperação de recurso inexistente na persistência.
// Objetivo: Garantir que o serviço lance NotFoundException e não execute o mapeamento.
[Test]
public async Task GetByIdAsync_WhenEntityDoesNotExist_ThrowsNotFoundException()
{
    // ...
}
```

---

### 2.3 Estrutura Arrange / Act / Assert (AAA)
O corpo de todo teste unitário deve ser explicitamente delimitado pelos marcadores:

```csharp
// Cenário: Criação de hóspede com e-mail já existente na base de dados.
// Objetivo: Validar que uma exceção de conflito de negócio seja lançada e o repositório não persista novos registros.
[Test]
public async Task CreateGuestAsync_WhenEmailAlreadyExists_ThrowsBusinessConflictException()
{
    // Arrange
    var guestDto = new Faker<GuestCreateDto>()
        .RuleFor(g => g.FullName, f => f.Person.FullName)
        .RuleFor(g => g.Email, f => f.Internet.Email())
        .RuleFor(g => g.Document, f => f.Random.Replace("###.###.###-##"))
        .Generate();

    _guestRepositoryMock
        .Setup(r => r.ExistsByEmailAsync(guestDto.Email, It.IsAny<CancellationToken>()))
        .ReturnsAsync(true);

    // Act
    Func<Task> act = async () => await _guestService.CreateGuestAsync(guestDto, CancellationToken.None);

    // Assert
    await act.Should().ThrowAsync<BusinessConflictException>()
        .WithMessage("*já cadastrado*");

    _guestRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<GuestEntity>(), It.IsAny<CancellationToken>()), Times.Never);
}
```

---

## 3. Guia de Ferramental e Boas Práticas

### 3.1 Geração de Dados com Bogus
- Evitar valores fixos arbitrários (*hardcoded* como `"teste"`, `"12345"`).
- Utilizar `Faker<T>` para criar entidades válidas, dados aleatórios consistentes e variações de limites (strings nulas, vazias, limites máximos de caracteres, datas passadas/futuras):

```csharp
private readonly Faker<RoomEntity> _roomFaker = new Faker<RoomEntity>()
    .RuleFor(r => r.Id, f => f.Random.Long(1, 10000))
    .RuleFor(r => r.RoomNumber, f => f.Random.Number(101, 999).ToString())
    .RuleFor(r => r.DailyRate, f => f.Finance.Amount(100, 2000))
    .RuleFor(r => r.IsAvailable, f => f.Random.Bool());
```

---

### 3.2 Simulação de Dependências com Moq
- Configurar retornos assíncronos explícitos: `.ReturnsAsync(...)`, `.ThrowsAsync(...)`.
- Usar `It.IsAny<CancellationToken>()` para permitir cancelamentos sem acoplamento.
- Usar `.Verify(..., Times.Once)` ou `.Verify(..., Times.Never)` para assegurar que apenas os métodos esperados foram invocados.
- Em múltiplas asserções sobre propriedades de um objeto retornado, agrupar com `Assert.Multiple`:

```csharp
// Assert
Assert.Multiple(() =>
{
    result.Should().NotBeNull();
    result.Id.Should().Be(expectedId);
    result.Status.Should().Be(ReservationStatus.Confirmed);
});
```

---

### 3.3 Mocks de EF Core com `Moq.EntityFrameworkCore`
Ao simular `DbSet<T>` e consultas LINQ assíncronas do EF Core:
```csharp
var rooms = _roomFaker.Generate(5);
_dbContextMock.Setup(db => db.Rooms).ReturnsDbSet(rooms);
```

---

## 4. Matriz de Cenários para Cobertura Total (100%)

Para cada método ou caso de uso, cobrir obrigatoriamente os 4 quadrantes:

```mermaid
quadrantChart
    title Quadrantes de Cobertura de Testes
    x-axis Casos de Sucesso --> Casos de Exceção
    y-axis Dados Típicos --> Dados Extremos / Limites
    Fluxo Principal (Happy Path): [0.25, 0.75]
    Fluxos Alternativos: [0.25, 0.25]
    Validação de Limites (Boundary): [0.75, 0.75]
    Tratamento de Erros e Exceções: [0.75, 0.25]
```

1. **Fluxo Principal (*Happy Path*):** Entrada válida, estado esperado, retorno com sucesso e mapeamento correto.
2. **Fluxos Alternativos:** Filtros opcionais, paginação, buscas vazias, cenários condicionais secundários.
3. **Casos Limite (*Boundary Testing*):** Arrays vazios, valores nulos, strings com tamanho máximo permitido, números negativos, datas limítrofes.
4. **Erros e Exceções:** Falhas de validação (FluentValidation), recursos não encontrados (404), indisponibilidade de serviços de IA externos, timeouts e cancelamentos via `CancellationToken`.

---

## 5. Roteiro Operacional de Execução e Coleta de Cobertura

```powershell
# 1. Compilar toda a solução em modo Release
dotnet build <Solucao>.sln -c Release

# 2. Executar toda a suíte de testes com coleta de cobertura via Coverlet
dotnet test <Solucao>.sln -c Release --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Threshold=90

# 3. Executar projeto de teste específico isoladamente
dotnet test <CaminhoProjetoTeste>.csproj -c Release /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# 4. Gerar relatório HTML de cobertura com ReportGenerator
reportgenerator -reports:**/coverage.opencover.xml -targetdir:./CoverageReport -reporttypes:Html
```

---

## 6. Checklist de Qualidade para Novos Testes

- [ ] Nome do método em inglês no padrão `Metodo_Cenario_Resultado`.
- [ ] Comentários em português `// Cenário:` e `// Objetivo:` presentes acima do método.
- [ ] Seções `// Arrange`, `// Act`, `// Assert` demarcadas no corpo do método.
- [ ] Dados dinâmicos gerados via Bogus (sem strings arbitrárias fixas).
- [ ] Mocks configurados com comportamentos assíncronos e verificações (`Verify`).
- [ ] Asserções fluentes (`AwesomeAssertions`) e `Assert.Multiple` quando houver mais de uma validação.
- [ ] Cobertura de linhas e ramos verificada sem regressões.
- [ ] Execução rápida e determinística (sem dependência de ordem de execução ou estado compartilhado).
