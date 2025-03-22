using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using System.Text;

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
                if (historyPrompts.Length > 0 && historyPrompts.Any(x => x.RoleType == RoleAiPromptsType.Agent))
                {
                    return await ChatCompletionByAgent(historyPrompts);
                }
                else
                {
                    return await ChatCompletion(historyPrompts);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "AssistantService AskAssistant: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            }
            return null;
        }

        private async Task<AskAssistantResponse[]> ChatCompletion(PromptMessageVO[] historyPrompts)
        {
            var result = await _aIInferenceService.GenerateChatCompletionAsync(historyPrompts, _eIAInferenceAdapterType);
            return [
                new AskAssistantResponse()
                    {
                        Message = result,
                        Role = RoleAiPromptsType.Assistant
                    }];
        }

        private async Task<AskAssistantResponse[]?> ChatCompletionByAgent(PromptMessageVO[] historyPrompts)
        {
            var result = await _aIInferenceService.GenerateChatCompletionByAgentAsync(historyPrompts, _eIAInferenceAdapterType);
            return [
                new AskAssistantResponse()
                    {
                        Message = result,
                        Role = RoleAiPromptsType.Assistant
                    }];
        }

        private static PromptMessageVO[] CreatePrompts(AskAssistantRequest request)
        {
            var sysMsgRuleAgent = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsType.Agent,
                Content = new StringBuilder()
                .AppendLine("Você é um especializado em viagens e turismo. Responda exclusivamente a perguntas relacionadas a:")
                .AppendLine("- Planejamento de viagens.")
                .AppendLine("- Reservas de hotéis, voos e transporte.")
                .AppendLine("- Recomendações de destinos turísticos, passeios, atrações e pacotes de viagem.")
                .AppendLine("- Seu nome é StayMate. Invente uma persona e personalidade para seu nome.")
                .AppendLine()
                .AppendLine("Diretrizes:")
                .AppendLine("1. Forneça respostas completas e confiáveis sobre turismo.")
                .AppendLine("2. Adote um tom positivo e amigável para encorajar o usuário.")
                .AppendLine("3. Utilize formatos visuais em Markdown para apresentar informações, sempre em português brasileiro.")
                .AppendLine()
                .AppendLine("Limitações:")
                .AppendLine("- Não forneça informações fora do escopo de viagens e turismo.")
                .AppendLine("- Caso a pergunta esteja fora do escopo, responda com respeito e objetividade, indicando que não pode ajudar com o tópico abordado.")
                .ToString()
            };
            PromptMessageVO sysMsgUnified = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsType.System,
                Content = "Você é um assistente especializado em viagens e turismo. Sua função é responder exclusivamente a perguntas relacionadas a viagens, reservas de hotéis e turismo. Limite suas respostas a esses tópicos, e quando uma pergunta estiver fora desse escopo, responda de forma educada, objetiva e concisa, informando que não pode ajudar com o tópico mencionado. Responda sempre em português brasileiro (pt-BR), utilizando linguagem clara e fluida. Formate suas respostas para exibição em HTML ou Markdown, utilizando tags adequadas para renderização correta no site."
            }; 

            PromptMessageVO userMsg = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsType.User,
                Content = request.Message
            };

            PromptMessageVO[] messages = [sysMsgRuleAgent, sysMsgUnified, userMsg];
            return messages;
        }
    }
}