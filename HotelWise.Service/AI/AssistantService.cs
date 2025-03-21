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
            _eIAInferenceAdapterType = applicationConfig.RagConfig.GetAInferenceAdapterType();
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

        public async Task<AskAssistantResponse[]?> AskAssistant(AskAssistantRequest request)
        {
            try
            { 
                //Feature 1) Save history by token (ID_Chat_Token 'guid', DataHistory 'json' , TotalTokens CountMenssage, DateTime, IdUser
                //SQL BUT EXPECTED MONGO BD OR AZURE DATATABLE
                //Feature 2) Get History add request if not great rule max token 
                PromptMessageVO[] historyPrompts = CreatePrompts(request);

                var result = await _aIInferenceService.GenerateChatCompletionAsync(historyPrompts, _eIAInferenceAdapterType);
                return [
                    new AskAssistantResponse() 
                    {
                        Message = result,
                        Role = RoleAiPromptsType.Assistant 
                    }];
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "AssistantService AskAssistant: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            }
            return null;
        }

        private static PromptMessageVO[] CreatePrompts(AskAssistantRequest request)
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
                Content = request.Message
            };
            PromptMessageVO[] messages = [sysMsgRule01, sysMsgRule02, sysMsgLanguage, sysMsgHtmlOut, userMsg];
            return messages;
        }
    }
}