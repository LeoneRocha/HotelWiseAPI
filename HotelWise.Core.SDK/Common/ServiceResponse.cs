namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Resposta padronizada de operações de serviço.
/// </summary>
/// <typeparam name="T">Tipo do payload de dados retornado.</typeparam>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.ServiceResponse. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class ServiceResponse<T> : SmartCoreHub.Core.SDK.Common.ServiceResponse<T>, HotelWise.Core.SDK.Abstractions.IServiceResponse<T>
{
}
