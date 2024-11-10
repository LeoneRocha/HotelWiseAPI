using System.ComponentModel;

namespace HotelWise.Domain.Enuns
{
    public enum EIAInferenceAdapterType
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