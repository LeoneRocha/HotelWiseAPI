using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;

namespace HotelWise.Service.Entity
{
    public class AssistantService : IAssistantService
    {
        private readonly IAIInferenceService _aIInferenceService;
        private readonly InferenceAiAdapterType _eIAInferenceAdapterType;
        protected long UserId { get; private set; }

        public AssistantService(IAIInferenceService aIInferenceService)
        {
            _eIAInferenceAdapterType = InferenceAiAdapterType.GroqApi;//Get TYPE By APPCONFIG 


            _aIInferenceService = aIInferenceService;
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
            PromptMessageVO system1Msg = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsType.System,
                Content = "Você é um assistente de viagens e turismo. Você só responde a perguntas relacionadas a viagens, reservas de hotéis e turismo. Se a pergunta estiver fora desse escopo, responda de forma objetiva que não pode ajudar com isso. Não forneca nehuma infomação fora do escopo sobre viagens, reservas de hotéis e turismo.  Responda sempre em idioma português brasileiro em pt-br. E Também em formato html"
            };
            PromptMessageVO system2Msg = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsType.System,
                Content = "Só responda exclusivamente em tópicos relacionados a viagens e turismo, e a responder de forma respeitosa e breve quando a pergunta estiver fora desse escopo. Responda sempre em idioma português brasileiro em pt-br. E Também em formato html"
            };
            PromptMessageVO system3Msg = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsType.System,
                Content = "Responda sempre em idioma português brasileiro em pt-br. E Também em formato html"
            };
            PromptMessageVO userMsg = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsType.User,
                Content = searchCriteria.SearchTextCriteria
            };
            PromptMessageVO[] messages = [system1Msg, system2Msg, system3Msg, userMsg];

            var result = await _aIInferenceService.GenerateChatCompletionAsync(messages, _eIAInferenceAdapterType);
            return [new AskAssistantResponse() { Response = result }];
        }
    }
}