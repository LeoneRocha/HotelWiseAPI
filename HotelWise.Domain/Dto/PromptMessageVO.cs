using HotelWise.Domain.Enuns;
using HotelWise.Domain.Helpers;

namespace HotelWise.Domain.Dto
{
    public class PromptMessageVO
    {
        public RoleAiPromptsEnum RoleType { get; set; }

        public string Role => RoleType.GetDescription();

        public string Content { get; set; } = string.Empty;
    }

}
