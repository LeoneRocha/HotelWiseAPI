using AutoMapper;
using FluentValidation;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Helpers.AI;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.Entity.IA;
using HotelWise.Domain.Interfaces.IA;
using HotelWise.Domain.Model.AI;
using HotelWise.Domain.Validator.AI;
using System.Text;

namespace HotelWise.Service.Entity
{
    public class AssistantService : IAssistantService
    {
        private readonly IAIInferenceService _aIInferenceService;
        private readonly InferenceAiAdapterType _eIAInferenceAdapterType;
        private readonly IChatSessionHistoryService _chatSessionHistoryService;
        protected readonly IMapper _mapper;

        protected long UserId { get; private set; }
        private readonly Serilog.ILogger _logger;
        public AssistantService(Serilog.ILogger logger, IApplicationIAConfig applicationConfig,
            IAIInferenceService aIInferenceService,
            IChatSessionHistoryService chatSessionHistoryService,
            IMapper mapper
            )
        {
            _logger = logger;
            _eIAInferenceAdapterType = applicationConfig.RagConfig.GetAInferenceAdapterType();
            _aIInferenceService = aIInferenceService;
            _chatSessionHistoryService = chatSessionHistoryService;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
                // Valida o request
                var requestValidator = new AskAssistantRequestValidator();
                var requestValidationResult = requestValidator.Validate(request);

                if (!requestValidationResult.IsValid)
                {
                    throw new ValidationException(requestValidationResult.Errors);
                }
                var currentToken = string.IsNullOrWhiteSpace(request.Token) ? Guid.NewGuid().ToString() : request.Token;

                // Verifica se já existe uma sessão para o token
                ChatSessionHistoryDto existingSession = await _chatSessionHistoryService.GetByIdTokenAsync(currentToken);

                //Feature 3) Get History add request if not great rule max token 
                PromptMessageVO[] historyPrompts = CreatePrompts(request, existingSession);

                // valida prompts  
                var promptsValidator = new HistoryPromptsValidator();
                var promptsValidationResult = promptsValidator.Validate(historyPrompts);

                if (!promptsValidationResult.IsValid)
                {
                    throw new ValidationException(promptsValidationResult.Errors);
                }
                AskAssistantResponse[] askAssistantResponses;
                if (historyPrompts.Length > 0 && historyPrompts.Any(x => x.RoleType == RoleAiPromptsType.Agent))
                {
                    askAssistantResponses = await ChatCompletionByAgent(historyPrompts);
                }
                else
                {
                    askAssistantResponses = await ChatCompletion(historyPrompts);
                }
                var userCurrentPrompt = historyPrompts.First(mg => mg.RoleType == RoleAiPromptsType.User);

                await PersistChatAsync(request, userCurrentPrompt, askAssistantResponses, existingSession, currentToken);
                return askAssistantResponses;
            }
            catch (ValidationException ex)
            {
                _logger.Error(ex, "Erro de validação: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "AssistantService AskAssistant: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            }
            return null;
        }
        #region PersistChat

        private async Task PersistChatAsync(AskAssistantRequest request, PromptMessageVO promptMessageUser, AskAssistantResponse[] askAssistantResponses, ChatSessionHistoryDto? existingSession, string currentToken)
        {
            try
            {
                if (existingSession != null)
                {
                    await UpdateExistingSession(existingSession, promptMessageUser, askAssistantResponses);
                }
                else
                {
                    await CreateNewSession(currentToken, promptMessageUser, askAssistantResponses);
                }
                // Atualiza o token nas respostas do assistente
                UpdateTokenInResponses(currentToken, askAssistantResponses);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "PersistChatAsync - Erro ao persistir o chat: {Message} at {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            }
        }
        private async Task UpdateExistingSession(ChatSessionHistory existingSession, PromptMessageVO promptMessageUser, AskAssistantResponse[] askAssistantResponses)
        {
            var updatedHistory = existingSession.PromptMessageHistory.ToList();

            // Adiciona a nova mensagem do usuário
            updatedHistory.Add(promptMessageUser);

            // Adiciona as respostas do assistente
            if (askAssistantResponses?.Length > 0)
            {
                updatedHistory.AddRange(askAssistantResponses.Select(response => new PromptMessageVO
                {
                    Content = response.Message,
                    RoleType = RoleAiPromptsType.Assistant
                }));
            }
            // Atualiza os dados da sessão
            existingSession.PromptMessageHistory = updatedHistory.ToArray();
            existingSession.CountMessages = updatedHistory.Count(mg => mg.RoleType == RoleAiPromptsType.User);
            existingSession.TotalTokensMessage = TokenCounterHelper.CalculateTotalTokens(updatedHistory.ToArray());
            var sessionUpdate = _mapper.Map<ChatSessionHistoryDto>(existingSession);
            // Persistir alterações
            await _chatSessionHistoryService.UpdateAsync(sessionUpdate);
        }
        private async Task CreateNewSession(string token, PromptMessageVO promptMessageUser, AskAssistantResponse[] askAssistantResponses)
        {
            var newSession = new ChatSessionHistoryDto
            {
                IdToken = token,
                Title = promptMessageUser.Content.Length > 50 ? promptMessageUser.Content.Substring(0, 50) : promptMessageUser.Content,
                PromptMessageHistory = BuildChatHistory(promptMessageUser, askAssistantResponses),
                CountMessages = 1 + (askAssistantResponses?.Length ?? 0),
                SessionDateTime = DataHelper.GetDateTimeNow(),
                IdUser = UserId
            };
            newSession.TotalTokensMessage = TokenCounterHelper.CalculateTotalTokens([promptMessageUser]);
            // Persistir nova sessão
            await _chatSessionHistoryService.AddAsync(newSession);
        }
        private static PromptMessageVO[] BuildChatHistory(PromptMessageVO promptMessageUser, AskAssistantResponse[] askAssistantResponses)
        {
            var messages = new List<PromptMessageVO> { promptMessageUser };

            if (askAssistantResponses?.Length > 0)
            {
                messages.AddRange(askAssistantResponses.Select(response => new PromptMessageVO
                {
                    Content = response.Message,
                    RoleType = RoleAiPromptsType.Assistant
                }));
            }

            return messages.ToArray();
        }
        private static void UpdateTokenInResponses(string token, AskAssistantResponse[] askAssistantResponses)
        {
            if (askAssistantResponses == null) return;

            foreach (var response in askAssistantResponses)
            {
                response.Token = token;
            }
        }
        #endregion PersistChat

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

        private static PromptMessageVO[] CreatePrompts(AskAssistantRequest request, ChatSessionHistoryDto? existingSession)
        {
            var msgAgent = new StringBuilder().AppendLine("Você é um especializado em viagens e turismo. Responda exclusivamente a perguntas relacionadas a:")
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
                .ToString();

            var msgSystem = "Você é um assistente especializado em viagens e turismo. Sua função é responder exclusivamente a perguntas relacionadas a viagens, reservas de hotéis e turismo. Limite suas respostas a esses tópicos, e quando uma pergunta estiver fora desse escopo, responda de forma educada, objetiva e concisa, informando que não pode ajudar com o tópico mencionado. Responda sempre em português brasileiro (pt-BR), utilizando linguagem clara e fluida. Formate suas respostas para exibição em HTML ou Markdown, utilizando tags adequadas para renderização correta no site.";
            var sysMsgRuleAgent = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsType.Agent,
                Content = msgAgent,
            };
            PromptMessageVO sysMsgUnified = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsType.System,
                Content = msgSystem,
            };
            PromptMessageVO userMsg = new PromptMessageVO()
            {
                RoleType = RoleAiPromptsType.User,
                Content = request.Message,
            };

            List<PromptMessageVO> promptMessageVOs = new List<PromptMessageVO>() { sysMsgRuleAgent, sysMsgUnified, userMsg };

            if (existingSession != null)
            {
                string context = ChatSessionHelper.GetHistoryContext(existingSession);

                PromptMessageVO histories = new PromptMessageVO()
                {
                    RoleType = RoleAiPromptsType.Context,
                    Content = HtmlHelper.RemoveHtml(context),
                };
                promptMessageVOs.Add(histories);
            }
            return promptMessageVOs.ToArray();
        }
    }
}