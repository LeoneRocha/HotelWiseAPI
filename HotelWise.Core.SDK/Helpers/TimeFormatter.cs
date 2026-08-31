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
    /// <summary>
    /// Formata um <see cref="TimeSpan"/> como <c>HH:mm:ss</c> (horas, minutos e segundos com dois dígitos).
    /// </summary>
    /// <param name="elapsed">Intervalo decorrido a formatar.</param>
    /// <returns>String no formato <c>00:00:00</c>.</returns>
    public static string FormatElapsedTime(TimeSpan elapsed)
    {
        return string.Format("{0:00}:{1:00}:{2:00}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
    }
}
