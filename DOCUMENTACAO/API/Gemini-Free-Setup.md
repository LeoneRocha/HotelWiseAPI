# Habilitar Gemini Free (chat + embeddings) — HotelWise

Gemini entra pelo conector **OpenAI-compatible** já existente no SDK  
(`AIChatServiceApi=OpenAI` + endpoint Google).

## 1) Criar API Key

1. Abra [Google AI Studio](https://aistudio.google.com/apikey)
2. Faça login com conta Google
3. **Create API key** → copie a chave

## 2) Aplicar o perfil Gemini

Arquivo de referência:

`HotelWise.API/appsettings.Development.Gemini.backup.json`

Opções:

- **Produção / base:** já apontado em `appsettings.json` (Gemini 768)
- **Development:** copie o bloco `ApplicationIAConfig` do backup Gemini para `appsettings.Development.json`, **ou** mantenha LM Studio local e use Gemini só em produção

Substitua `YOUR_GEMINI_API_KEY` / `ApiKey` vazia pela chave real.

Em Azure / IIS, preferível variável de ambiente:

```text
ApplicationIAConfig__AIServices__OpenAI__ApiKey=<sua-chave>
ApplicationIAConfig__AIServices__OpenAIEmbeddings__ApiKey=<sua-chave>
```

## 3) Valores que devem bater

| Setting | Valor |
|---------|--------|
| Endpoint chat/embeddings | `https://generativelanguage.googleapis.com/v1beta/openai/` |
| Chat `ModelId` | `gemini-2.5-flash` |
| Embedding `ModelId` | `gemini-embedding-001` |
| `AIChatServiceApi` | `OpenAI` |
| `AIEmbeddingServiceApi` | `OpenAIEmbeddings` |
| `VectorStoreDimensions` | `768` (recomendado; nativo seria 3072) |
| Prefixo coleção | `production_gemini768_` |

O SDK envia `EmbeddingGenerationOptions.Dimensions = VectorStoreDimensions` para truncar o vetor Gemini.

## 4) Reindexar

Coleção antiga (Mistral 1024 / Nomic 768) **não** serve.

1. Reinicie a API
2. Reindexe os hotéis no vector store (coleção `production_gemini768_skhotels`)
3. Teste busca semântica + chat

## 5) Voltar para outro provider

| Perfil | Arquivo |
|--------|---------|
| Mistral | `appsettings.Development.Mistral.backup.json` |
| Gemini | `appsettings.Development.Gemini.backup.json` |
| LM Studio (dev) | `appsettings.Development.json` atual |

Sempre: alinhar modelo ↔ `VectorStoreDimensions` ↔ prefixo novo ↔ reindex.

## Docs Google

- [OpenAI compatibility](https://ai.google.dev/gemini-api/docs/openai)
- [Embeddings](https://ai.google.dev/gemini-api/docs/embeddings)
