using System.ComponentModel;
using System.Reflection;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Extensões para enumerações, incluindo leitura de <see cref="DescriptionAttribute"/>.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Extensions.EnumExtensions. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class EnumExtensions
{
    /// <summary>
    /// Obtém a descrição do valor do enum via <see cref="DescriptionAttribute"/>,
    /// ou o nome do valor quando o atributo estiver ausente.
    /// </summary>
    /// <param name="value">Valor da enumeração.</param>
    /// <returns>Texto da descrição ou o nome do enum.</returns>
    public static string GetDescription(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString())!;
        DescriptionAttribute? attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
}
