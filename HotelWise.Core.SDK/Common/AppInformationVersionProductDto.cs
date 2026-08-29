namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO com informações de versão e ambiente do produto.
/// Utilizado para expor metadados da aplicação (identidade, nome, versão e ambiente)
/// em endpoints de health, diagnóstico ou about.
/// </summary>
public class AppInformationVersionProductDto
{
    /// <summary>
    /// Identificador do produto ou da instância da aplicação.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Nome do produto ou da aplicação.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Versão do produto (ex.: semântica ou build).
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Nome do ambiente de execução (ex.: Development, Staging, Production).
    /// </summary>
    public string EnvironmentName { get; set; } = string.Empty;

    /// <summary>
    /// Mensagem complementar associada à informação de versão/ambiente.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
