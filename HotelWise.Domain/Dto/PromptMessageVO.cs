using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;

namespace HotelWise.Domain.Dto
{
    public class PromptMessageVO
    {
        public DataVectorVO[] DataContextRag { get; set; }
        public RoleAiPromptsType RoleType { get; set; }

        public string Role => RoleType.GetDescription();

        public string Content { get; set; } = string.Empty;

        public string AgentName { get; set; } = string.Empty;
    }
    public class DataVectorVO
    {
        public string KeyVector { get; set; } = string.Empty;
        public string DataVector { get; set; } = string.Empty;
    }
}
