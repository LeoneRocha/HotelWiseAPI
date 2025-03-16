using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;

namespace HotelWise.Service.Entity
{
    public class AssistantService : IAssistantService
    {
        private readonly IAIInferenceService _aIInferenceService;
        private readonly InferenceAiAdapterType _eIAInferenceAdapterType;
        protected long UserId { get; private set; }
        private readonly Serilog.ILogger _logger;


        public AssistantService(Serilog.ILogger logger, IApplicationIAConfig applicationConfig, IAIInferenceService aIInferenceService)
        {
            _logger = logger;
            _eIAInferenceAdapterType = getAdapterType(applicationConfig); 
            _aIInferenceService = aIInferenceService;
        }

        private static InferenceAiAdapterType getAdapterType(IApplicationIAConfig applicationConfig)
        {
            if (applicationConfig != null && applicationConfig.RagConfig != null)
            {
                var typeAdp = applicationConfig.RagConfig.AIChatService;

                switch (typeAdp)
                {
                    case AIChatServiceType.AzureOpenAI:
                        break;
                    case AIChatServiceType.OpenAI:
                        break;
                    case AIChatServiceType.GroqApi:
                        return InferenceAiAdapterType.GroqApi;  
                    case AIChatServiceType.MistralApi:
                        return InferenceAiAdapterType.Mistral;
                        break;
                    case AIChatServiceType.Anthropic:
                        break;
                    case AIChatServiceType.Cohere:
                        break;
                    case AIChatServiceType.Ollama:
                        return InferenceAiAdapterType.Ollama;
                        break;
                    case AIChatServiceType.LlamaCpp:
                        break;
                    case AIChatServiceType.HuggingFace:
                        break;
                    default:
                        break;
                }
            }
            return InferenceAiAdapterType.GroqApi; 
        }

        public void SetUserId(long id)
        {
            UserId = id;
        }
        public async Task<float[]?> GenerateEmbeddingAsync(string text)
        {
            return await _aIInferenceService.GenerateEmbeddingAsync(text, _eIAInferenceAdapterType);
        }

        public async Task<AskAssistantResponse[]> AskAssistant(SearchCriteria searchCriteria)
        {
            try
            {
                PromptMessageVO sysMsgRule01 = new PromptMessageVO()
                {
                    RoleType = RoleAiPromptsType.System,
                    Content = "Você é um assistente de viagens e turismo. Você só responde a perguntas relacionadas a viagens, reservas de hotéis e turismo. Se a pergunta estiver fora desse escopo, responda de forma objetiva que não pode ajudar com isso. Não forneca nehuma infomação fora do escopo sobre viagens, reservas de hotéis e turismo."
                };
                PromptMessageVO sysMsgRule02 = new PromptMessageVO()
                {
                    RoleType = RoleAiPromptsType.System,
                    Content = "Só responda exclusivamente em tópicos relacionados a viagens e turismo, e a responder de forma respeitosa e breve quando a pergunta estiver fora desse escopo."
                };
                PromptMessageVO sysMsgLanguage = new PromptMessageVO()
                {
                    RoleType = RoleAiPromptsType.System,
                    Content = "Responda sempre em idioma português brasileiro em pt-br."
                };
                PromptMessageVO sysMsgHtmlOut = new PromptMessageVO()
                {
                    RoleType = RoleAiPromptsType.System,
                    Content = "E responda em formato html com tags html ou em Markdown, formatando a resposta para ser renderizado no site."
                };
                PromptMessageVO userMsg = new PromptMessageVO()
                {
                    RoleType = RoleAiPromptsType.User,
                    Content = searchCriteria.SearchTextCriteria
                };
                PromptMessageVO[] messages = [sysMsgRule01, sysMsgRule02, sysMsgLanguage, sysMsgHtmlOut, userMsg];

                var result = await _aIInferenceService.GenerateChatCompletionAsync(messages, _eIAInferenceAdapterType);
                return [new AskAssistantResponse() { Response = result }];
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "HotelVectorStoreService GetById: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                throw ;
            } 
            
        }
    }
}