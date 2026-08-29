using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.Extensions;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Fragmento de contexto vetorial em prompts.
/// </summary>
public class DataVectorVO
{
    public string KeyVector { get; set; } = string.Empty;
    public string DataVector { get; set; } = string.Empty;
}

/// <summary>
/// Mensagem de prompt para adapters de inferência.
/// </summary>
public class PromptMessageVO
{
    public DataVectorVO[] DataContextRag { get; set; } = Array.Empty<DataVectorVO>();
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
                return HotelWise.Core.SDK.AI.Helpers.TokenCounterHelper.CountTokens(Content);
            }
            if (DataContextRag != null && DataContextRag.Length > 0)
            {
                HotelWise.Core.SDK.AI.Helpers.TokenCounterHelper.CalculateDataVectorLength(DataContextRag);
            }
            return 0;
        }
    }

    public int ContentLenght => Content.Length;
}
