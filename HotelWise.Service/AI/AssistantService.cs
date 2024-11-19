using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Model;
using Microsoft.Graph.Models;

namespace HotelWise.Service.Entity
{
    public class AssistantService : IAssistantService
    {
        private readonly IAIInferenceService _aIInferenceService;
        private readonly EIAInferenceAdapterType _eIAInferenceAdapterType;

        public AssistantService(IAIInferenceService aIInferenceService)
        {
            _eIAInferenceAdapterType = EIAInferenceAdapterType.GroqApi;
            _aIInferenceService = aIInferenceService;
        }

        public async Task<float[]?> GenerateEmbeddingAsync(string text)
        {
            return await _aIInferenceService.GenerateEmbeddingAsync(text, _eIAInferenceAdapterType);
        }

        public async Task<AskAssistantResponse[]> AskAssistant(SearchCriteria searchCriteria)
        {
            PromptMessageVO system1Msg = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsEnum.System,
                Content = "Você é um assistente de viagens e turismo. Você só responde a perguntas relacionadas a viagens, reservas de hotéis e turismo. Se a pergunta estiver fora desse escopo, responda de forma objetiva que não pode ajudar com isso. Não forneca nehuma infomação fora do escopo sobre viagens, reservas de hotéis e turismo.  Responda sempre em idioma português brasileiro em pt-br. E Também em formato html"
            };
            PromptMessageVO system2Msg = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsEnum.System,
                Content = "Só responda exclusivamente em tópicos relacionados a viagens e turismo, e a responder de forma respeitosa e breve quando a pergunta estiver fora desse escopo. Responda sempre em idioma português brasileiro em pt-br. E Também em formato html"
            };
            PromptMessageVO system3Msg = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsEnum.System,
                Content = "Responda sempre em idioma português brasileiro em pt-br. E Também em formato html"
            };
            PromptMessageVO userMsg = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsEnum.User,
                Content = searchCriteria.SearchTextCriteria
            };
            PromptMessageVO[] messages = [system1Msg, system2Msg, userMsg];

            var result = await _aIInferenceService.GenerateChatCompletionAsync(messages, _eIAInferenceAdapterType);
            return [new AskAssistantResponse() { Response = result }];
        }
    }
}