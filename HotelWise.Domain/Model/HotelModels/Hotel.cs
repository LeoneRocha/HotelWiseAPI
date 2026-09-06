using System.ComponentModel.DataAnnotations.Schema;
using SmartCoreHub.Core.SDK.Domain.Abstractions;

namespace HotelWise.Domain.Model.HotelModels;

/// <summary>
/// Entidade de domínio que representa um estabelecimento hoteleiro com localização, classificação e dados de auditoria.
/// </summary>
public class Hotel : LongEntityBase, IEntityFieldBaseLog
{
    /// <summary>
    /// Alias de API/compat para a PK legada <c>HotelId</c> (coluna mapeada em <see cref="LongEntityBase.Id"/>).
    /// Não usar em predicados EF traduzidos — preferir <see cref="LongEntityBase.Id"/>.
    /// </summary>
    [NotMapped]
    public long HotelId
    {
        get => Id;
        set => Id = value;
    }

    /// <summary>
    /// Nome comercial do hotel.
    /// </summary>
    public string HotelName { get; set; } = string.Empty;

    /// <summary>
    /// Descrição institucional e atrativos do estabelecimento.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Tags e palavras-chave associadas aos diferenciais do hotel (ex: piscina, spa, praia).
    /// </summary>
    public string[] Tags { get; set; } = [];

    /// <summary>
    /// Classificação do hotel em número de estrelas (1 a 5).
    /// </summary>
    public byte Stars { get; set; }

    /// <summary>
    /// Valor base inicial da diária de quarto no hotel.
    /// </summary>
    public decimal InitialRoomPrice { get; set; }

    /// <summary>
    /// Código de endereçamento postal (CEP / ZipCode).
    /// </summary>
    public string ZipCode { get; set; } = string.Empty;

    /// <summary>
    /// Endereço ou localização textual do hotel.
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Cidade onde o hotel está sediado.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Sigla ou código do estado federativo (ex: "SP", "RJ", "CA").
    /// </summary>
    public string StateCode { get; set; } = string.Empty;

    /// <summary>
    /// Usuário responsável pela criação do registro.
    /// </summary>
    public User? CreatedUser { get; set; }

    /// <summary>
    /// Identificador do usuário que criou o registro.
    /// </summary>
    public long? CreatedUserId { get; set; }

    /// <summary>
    /// Usuário responsável pela última alteração do registro.
    /// </summary>
    public User? ModifyUser { get; set; }

    /// <summary>
    /// Identificador do usuário que realizou a última alteração.
    /// </summary>
    public long? ModifyUserId { get; set; }

    /// <summary>
    /// Alias legado de <see cref="LongEntityBase.CreatedAt"/>.
    /// </summary>
    [NotMapped]
    public DateTime CreatedDate
    {
        get => CreatedAt;
        set => CreatedAt = value;
    }

    /// <summary>
    /// Alias legado de <see cref="LongEntityBase.UpdatedAt"/>.
    /// </summary>
    [NotMapped]
    public DateTime ModifyDate
    {
        get => UpdatedAt;
        set => UpdatedAt = value;
    }
}
