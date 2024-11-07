namespace HotelWise.Domain.Interfaces
{
    public interface IAIInferenceAdapterFactory
    {
        IAIInferenceAdapter CreateAdapter(string apiKey, IModelStrategy modelStrategy);
    }
}