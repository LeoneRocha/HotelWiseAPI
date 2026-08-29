# HotelWise.Core.SDK

Núcleo reutilizável do ecossistema **HotelWise**, empacotado como NuGet único (`PackageId = HotelWise.Core.SDK`).

## Escopo

Abstrações, helpers, infraestrutura genérica (repositório/serviço base), segurança, middlewares HTTP e módulo de AI (adapters LLM, vector store, Semantic Kernel, configs RAG).

**Não inclui** entidades de negócio hoteleiro, DbContext, migrations, repositórios/serviços concretos de produto — esses permanecem nos projetos host.

## Target Frameworks

| TFM | Conteúdo |
| :--- | :--- |
| `net10.0` / `net8.0` | SDK completo (EF Core, ASP.NET Core, Semantic Kernel, GroqApiLibrary) |
| `netstandard2.1` / `netstandard2.0` | Superfície leve (tipos e helpers sem dependências pesadas) |

## Consumo

```xml
<ProjectReference Include="..\HotelWise.Core.SDK\HotelWise.Core.SDK.csproj" />
```

Ou via pacote NuGet após `dotnet pack`.

## Documentação

Ver `HotelWiseAPI/DOCUMENTACAO/HotelWise.Core.SDK/`.

## Licença

MIT — ver [LICENSE](./LICENSE).
