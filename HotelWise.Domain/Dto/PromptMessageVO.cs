using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;

namespace HotelWise.Domain.Dto
{
    public class PromptMessageVO
    {
        public RoleAiPromptsType RoleType { get; set; }

        public string Role => RoleType.GetDescription();

        public string Content { get; set; } = string.Empty;
    }

}
