namespace HotelWise.Domain.Model
{
    public class HotelResponse : Hotel
    {
        //TODO FAZER UM OBJETO QUE TRAGA menssagem da IA Sugestao prompt e em seguida os hoteis recomendados pelo vector search 

        //A IDEIA E TRANSFORMAR O SCORE EM UMA TAG LEGIVEL PARA O USUARIO TIPO IA RECOMENDA HOT ETC  e no texto que ia retornar ver dentro do que o vetor achar agrupar em uma propriedade HotelsIARecomend e outra prorpieade com hoteis do VECTOR HotelsSuggestions

        /// <summary>
        /// 
        /// </summary>
        public double Score { get; set; }
    }

    public class AskAssistantResponse
    {
        public string Response { get; set; } = string.Empty;
    }
}