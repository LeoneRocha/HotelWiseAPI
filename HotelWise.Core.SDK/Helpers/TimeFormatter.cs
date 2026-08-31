namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Formatação de intervalos de tempo (<see cref="TimeSpan"/>) para exibição
/// em logs, UI e relatórios no padrão HH:mm:ss.
/// </summary>
/// <example>
/// <code>
/// var label = TimeFormatter.FormatElapsedTime(sw.Elapsed);
/// // ex.: "01:05:09"
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Helpers.TimeFormatter. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class TimeFormatter
{
    /// <summary>Formata um intervalo <see cref="TimeSpan"/> no padrão HH:mm:ss.</summary>
    /// <param name="elapsed">Intervalo de tempo.</param>
    /// <returns>String formatada.</returns>
    public static string FormatElapsedTime(TimeSpan elapsed) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.TimeFormatter.FormatElapsedTime(elapsed);
}
