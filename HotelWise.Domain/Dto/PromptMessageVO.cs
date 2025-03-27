using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Helpers.AI;

namespace HotelWise.Domain.Dto
{
    public class PromptMessageVO
    {
        public DataVectorVO[] DataContextRag { get; set; }
        public RoleAiPromptsType RoleType { get; set; }
        public string Role => RoleType.GetDescription();
        public string Content { get; set; } = string.Empty;
        public string AgentName { get; set; } = string.Empty;
        public int TokenCount
        {
            get

            {
                if (!string.IsNullOrWhiteSpace(Content))
                {
                    return TokenCounterHelper.CountTokens(Content);
                }
                if (DataContextRag != null && DataContextRag.Length > 0)
                {
                    TokenCounterHelper.CalculateDataVectorLength(DataContextRag);
                }
                return 0;
            }
        }
        public int ContentLenght { get { return Content.Length; } }
    }
    public class DataVectorVO
    {
        public string KeyVector { get; set; } = string.Empty;
        public string DataVector { get; set; } = string.Empty;
    }
}
