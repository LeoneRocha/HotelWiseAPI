using System.ComponentModel;
using System.Reflection;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Extensões para enumerações, incluindo leitura de <see cref="DescriptionAttribute"/>.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Extensions.EnumExtensions. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class EnumExtensions
{
    public static string GetDescription(this Enum value) =>
        SmartCoreHub.Core.SDK.Domain.Extensions.EnumExtensions.GetDescription(value);
}
