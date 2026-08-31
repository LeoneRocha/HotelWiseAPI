using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#if NET8_0_OR_GREATER
using AutoMapper.Configuration.Annotations;
using Swashbuckle.AspNetCore.Annotations;
#endif

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Detalhe de erro padronizado em respostas de serviço.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.ErrorResponse. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class ErrorResponse : SmartCoreHub.Core.SDK.Common.ErrorResponse
{
}
