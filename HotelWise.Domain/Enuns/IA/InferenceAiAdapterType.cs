using System.ComponentModel;

namespace HotelWise.Domain.Enuns.IA
{
    public enum InferenceAiAdapterType
    {
        /// <summary>
        /// GroqApi
        /// </summary>
        [Description("GroqApi")]
        GroqApi = 0,

        /// <summary>
        ///  Mistral
        /// </summary>
        [Description("Mistral")]
        Mistral = 1,

        [Description("Ollama")]
        Ollama = 2,

        [Description("SemanticKernel")]
        SemanticKernel = 3,
    }
}