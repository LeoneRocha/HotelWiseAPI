using HotelWise.Domain.Enuns;

namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAIInferenceAdapterFactory
    {
        IAIInferenceAdapter CreateAdapter(IAInferenceAdapterType eIAInferenceAdapterType, IModelStrategy modelStrategy);
    }
}