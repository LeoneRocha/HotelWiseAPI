using HotelWise.Domain.Enuns.IA;

namespace HotelWise.Domain.Dto
{
    public class AskAssistantResponse
    {
        public RoleAiPromptsType Role { get; set; }

        public string Message { get; set; } = string.Empty;        
        public string Token { get; set; } = string.Empty;
    } 
    public class AskAssistantRequest
    {
        public RoleAiPromptsType Role { get; } = RoleAiPromptsType.User;
        public string Message { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
