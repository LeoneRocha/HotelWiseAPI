namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAIInferenceAdapterFactory
    {
        IAIInferenceAdapter CreateAdapter(string apiKey, IModelStrategy modelStrategy);
    }
}