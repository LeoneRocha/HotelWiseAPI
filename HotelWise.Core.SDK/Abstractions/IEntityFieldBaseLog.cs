namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de auditoria por usuário com campos escalares portáveis.
/// Registra quem criou/alterou o registro e quando, sem acoplar navegações
/// à entidade de usuário de domínio — essas permanecem no host da aplicação.
/// </summary>
public interface IEntityFieldBaseLog
{
    /// <summary>
    /// Identificador do usuário que criou o registro; <c>null</c> se não aplicável.
    /// </summary>
    long? CreatedUserId { get; set; }

    /// <summary>
    /// Identificador do usuário que alterou o registro pela última vez; <c>null</c> se não aplicável.
    /// </summary>
    long? ModifyUserId { get; set; }

    /// <summary>
    /// Data e hora de criação do registro.
    /// </summary>
    DateTime CreatedDate { get; set; }

    /// <summary>
    /// Data e hora da última alteração do registro.
    /// </summary>
    DateTime ModifyDate { get; set; }
}
