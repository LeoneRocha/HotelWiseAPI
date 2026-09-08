# Habilitar Gemini Free (chat + embeddings) — HotelWise

Gemini entra pelo conector **OpenAI-compatible** já existente no SDK  
(`AIChatServiceApi=OpenAI` + endpoint Google).

## Segurança (obrigatório)

- **Nunca** coloque API keys em `appsettings.json` (versionado).
- Chaves locais só em `appsettings.Development.json` (já está no `.gitignore`) ou variáveis de ambiente.
- Backups (`appsettings.*.backup.json`) usam placeholder `YOUR_GEMINI_API_KEY` — também estão no `.gitignore`.
- Em produção/Azure DevOps: secret/variável de ambiente, nunca commit.

```text
ApplicationIAConfig__AIServices__OpenAI__ApiKey=<sua-chave>
ApplicationIAConfig__AIServices__OpenAIEmbeddings__ApiKey=<sua-chave>
```

## 1) Criar API Key

1. Abra [Google AI Studio](https://aistudio.google.com/apikey)
2. Faça login com conta Google
3. **Create API key** → copie a chave

## 2) Aplicar o perfil Gemini (local)

1. Use o modelo em `appsettings.Development.Gemini.backup.json` (placeholders).
2. Em `appsettings.Development.json` (local, não versionado):
   - Endpoint Gemini + `gemini-3.5-flash` / `gemini-embedding-001`
   - Cole a chave **somente** em `OpenAI:ApiKey` e `OpenAIEmbeddings:ApiKey`
3. `appsettings.json` versionado deve manter `ApiKey` vazio (produção usa env/secret).

## 3) Valores que devem bater

| Setting | Valor |
|---------|--------|
| Endpoint chat/embeddings | `https://generativelanguage.googleapis.com/v1beta/openai/` |
| Chat `ModelId` | `gemini-3.5-flash` (evite `gemini-2.5-flash` — 404 para chaves novas) |
| Embedding `ModelId` | `gemini-embedding-001` |
| `AIChatServiceApi` | `OpenAI` |
| `AIEmbeddingServiceApi` | `OpenAIEmbeddings` |
| `VectorStoreDimensions` | `768` (recomendado; nativo seria 3072) |
| Prefixo coleção (prod) | `production_gemini768_` |
| Prefixo coleção (dev) | `develop_gemini768_` |

O SDK registra `AddOpenAIEmbeddingGenerator(..., dimensions: VectorStoreDimensions)` e também envia `EmbeddingGenerationOptions.Dimensions`.
Se o endpoint ainda devolver 3072, `EmbeddingHelper.FitToDimensions` aplica truncamento MRL + L2 normalize para 768.

## 4) Reindexar

Coleção antiga (Mistral 1024 / Nomic 768) **não** serve.

1. Reinicie a API
2. Reindexe os hotéis no vector store
3. Teste busca semântica + chat

## 5) Voltar para outro provider

| Perfil | Arquivo |
|--------|---------|
| Mistral | `appsettings.Development.Mistral.backup.json` |
| Gemini | `appsettings.Development.Gemini.backup.json` |
| LM Studio (local `192.168.15.21:1234`) | `appsettings.Development.LMStudio.backup.json` |
| Local secrets | `appsettings.Development.json` (gitignore) |

Sempre: alinhar modelo ↔ `VectorStoreDimensions` ↔ prefixo novo ↔ reindex.

## Docs Google

- [OpenAI compatibility](https://ai.google.dev/gemini-api/docs/openai)
- [Embeddings](https://ai.google.dev/gemini-api/docs/embeddings)
