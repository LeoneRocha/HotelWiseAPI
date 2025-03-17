using HotelWise.Domain.Enuns.IA;

namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAIInferenceAdapterFactory
    {
        IAIInferenceAdapter CreateAdapter(InferenceAiAdapterType eIAInferenceAdapterType);
    }
}