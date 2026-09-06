using System.ComponentModel;
using System.Reflection;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Extensions;

/// <summary>
/// Extensões para enumerações, incluindo leitura de <see cref="DescriptionAttribute"/>.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Extensions.EnumExtensions", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Extensions.EnumExtensions em SmartCoreHub.Core.SDK.")]
public static class EnumExtensions
{
    /// <summary>
    /// Obtém a descrição declarada no atributo <see cref="DescriptionAttribute"/> do valor de enum, ou seu nome textual.
    /// </summary>
    /// <param name="value">Valor de enum.</param>
    /// <returns>Descrição do enum.</returns>
    public static string GetDescription(this Enum value) =>
        SmartCoreHub.Core.SDK.Domain.Extensions.EnumExtensions.GetDescription(value);
}
