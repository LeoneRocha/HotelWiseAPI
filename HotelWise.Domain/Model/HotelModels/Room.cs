using HotelWise.Core.SDK.Abstractions;
using HotelWise.Domain.Enuns.Hotel;

namespace HotelWise.Domain.Model.HotelModels;

/// <summary>
/// Entidade de domínio que representa uma acomodação/quarto pertencente a um hotel.
/// </summary>
public class Room : IEntityFieldBaseLog
{
    /// <summary>
    /// Identificador único do quarto.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Identificador do hotel ao qual o quarto pertence.
    /// </summary>
    public long HotelId { get; set; }

    /// <summary>
    /// Classificação de acomodação (Single, Double, Suite, etc.).
    /// </summary>
    public RoomType RoomType { get; set; }

    /// <summary>
    /// Capacidade máxima de pessoas comportadas pelo quarto.
    /// </summary>
    public short Capacity { get; set; }

    /// <summary>
    /// Descrição dos itens, espaço e características do quarto.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Estado operacional do quarto (Disponível, Ocupado, Limpeza, etc.).
    /// </summary>
    public RoomStatus Status { get; set; }

    /// <summary>
    /// Número mínimo de diárias exigido para locação deste quarto.
    /// </summary>
    public short MinimumNights { get; set; } = 1;

    /// <summary>
    /// Referência de navegação ao <see cref="Hotel"/> proprietário.
    /// </summary>
    public Hotel? Hotel { get; set; }

    #region Auditoria e Relacionamentos

    /// <summary>
    /// Usuário que cadastrou o quarto no sistema.
    /// </summary>
    public User? CreatedUser { get; set; }

    /// <summary>
    /// Identificador do usuário que criou o registro.
    /// </summary>
    public long? CreatedUserId { get; set; }

    /// <summary>
    /// Usuário que realizou a última alteração no quarto.
    /// </summary>
    public User? ModifyUser { get; set; }

    /// <summary>
    /// Identificador do usuário que alterou o registro por último.
    /// </summary>
    public long? ModifyUserId { get; set; }

    /// <summary>
    /// Data e hora de criação do registro.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Data e hora da última modificação do registro.
    /// </summary>
    public DateTime ModifyDate { get; set; }

    /// <summary>
    /// Coleção de períodos de disponibilidade cadastrados para o quarto.
    /// </summary>
    public ICollection<RoomAvailability> RoomAvailabilities { get; set; } = new List<RoomAvailability>();

    /// <summary>
    /// Nome ou designador do quarto (ex: "Suíte Presidencial 101").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    #endregion
}