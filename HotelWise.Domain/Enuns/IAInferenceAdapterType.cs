using System.ComponentModel;

namespace HotelWise.Domain.Enuns
{
    public enum IAInferenceAdapterType
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
         
    }
}