using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.Extensions;
using SchDto = SmartCoreHub.Core.SDK.Domain.AI.DTO;
using SchEnums = SmartCoreHub.Core.SDK.Domain.AI.Enums;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Fragmento de contexto vetorial embutido em prompts RAG.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.DataVectorVO. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class DataVectorVO : SchDto.DataVectorVO
{
}

/// <summary>
/// Mensagem de prompt para adapters de inferência.
/// Herda SCH; enum HW espelhado via <c>new</c> + cast.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.PromptMessageVO. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class PromptMessageVO : SchDto.PromptMessageVO
{
    public new DataVectorVO[] DataContextRag
    {
        get
        {
            var b = base.DataContextRag;
            if (b is DataVectorVO[] hw)
                return hw;
            if (b is null || b.Length == 0)
                return Array.Empty<DataVectorVO>();
            return Array.ConvertAll(b, x => x is DataVectorVO d
                ? d
                : new DataVectorVO { KeyVector = x.KeyVector, DataVector = x.DataVector });
        }
        set => base.DataContextRag = value ?? Array.Empty<DataVectorVO>();
    }

    public new RoleAiPromptsType RoleType
    {
        get => (RoleAiPromptsType)(int)base.RoleType;
        set => base.RoleType = (SchEnums.RoleAiPromptsType)(int)value;
    }

    public new string Role => RoleType.GetDescription();
}
