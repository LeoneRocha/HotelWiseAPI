using HotelWise.Domain.Dto.IA.SemanticKernel;

namespace HotelWise.Domain.Helpers.AI
{
    public static class TokenCounterHelper
    {
        public static int CountTokens(string text)
        {
            // Simula a lógica de tokenização simples (aproximação)
            // Em um caso real, integre uma biblioteca de tokenização como o Tiktoken para cálculos precisos
            return text.Length / 4; // Aproximação: 1 token ≈ 4 caracteres
        }

        /// <summary>
        /// Calcula o total de caracteres em todos os DataVectors de um único PromptMessageVO.
        /// </summary>
        /// <param name="promptMessage">Instância de PromptMessageVO.</param>
        /// <returns>Total de caracteres somados de todos os DataVectors.</returns>
        public static int CalculateDataVectorLength(DataVectorVO[] dataContextRag)
        {
            if (dataContextRag == null || dataContextRag.Length == 0)
                return 0;

            return dataContextRag.Where(dv => dv != null && !string.IsNullOrEmpty(dv.DataVector)) // Filtra DataVectorVOs válidos
                .Sum(dv => dv.DataVector.Length);   // Soma o comprimento de cada DataVector
        }

        /// <summary>
        /// Calcula o total de caracteres em todos os DataVectors de uma coleção de PromptMessageVO.
        /// </summary>
        /// <param name="promptMessages">Coleção de objetos PromptMessageVO.</param>
        /// <returns>Total de caracteres somados de todos os DataVectors.</returns>
        public static int CalculateTotalDataVectorLength(PromptMessageVO[] promptMessages)
        {
            if (promptMessages == null || promptMessages.Length == 0)
                return 0;

            return promptMessages
                .Where(p => p != null) // Filtra objetos PromptMessageVO válidos
                .Sum(p => CalculateDataVectorLength(p.DataContextRag)); // Usa o método para um único objeto
        }

        /// <summary>
        /// Calcula o total de tokens em uma coleção de PromptMessageVO.
        /// </summary>
        /// <param name="promptMessages">Coleção de PromptMessageVO.</param>
        /// <returns>Total de tokens.</returns>
        public static int CalculateTotalTokens(PromptMessageVO[] promptMessages)
        {
            if (promptMessages == null || promptMessages.Length == 0)
                return 0;

            return promptMessages.Sum(p => CountTokensFromPrompt(p));
        }
        /// <summary>
        /// Calcula o total de tokens em um único PromptMessageVO.
        /// </summary>
        /// <param name="promptMessage">Instância de PromptMessageVO.</param>
        /// <returns>Total de tokens do PromptMessageVO.</returns>
        public static int CountTokensFromPrompt(PromptMessageVO promptMessage)
        {
            if (promptMessage == null)
                return 0;

            int contentTokens = CountTokens(promptMessage.Content); 
            int dataVectorTokens = CalculateDataVectorLength(promptMessage.DataContextRag);
            return contentTokens + dataVectorTokens;
        } 
    }
}