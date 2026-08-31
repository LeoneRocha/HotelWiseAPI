namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de resposta padronizada — casca sobre SCH.
/// </summary>
/// <typeparam name="T">Tipo do payload de dados retornado.</typeparam>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Abstractions.IServiceResponse. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IServiceResponse<T> : SmartCoreHub.Core.SDK.Domain.Abstractions.IServiceResponse<T>
{
}
