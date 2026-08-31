namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato mínimo de entidade de domínio com identificador e flag de habilitação.
/// Base comum para entidades persistidas que suportam ativação/desativação lógica.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Abstractions.IEntityBase. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IEntityBase
{
    /// <summary>
    /// Identificador único da entidade.
    /// </summary>
    long Id { get; set; }

    /// <summary>
    /// Indica se a entidade está habilitada (ativa) no sistema.
    /// </summary>
    bool Enable { get; set; }
}
