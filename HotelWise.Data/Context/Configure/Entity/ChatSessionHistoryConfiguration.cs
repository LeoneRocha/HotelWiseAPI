using HotelWise.Data.Context.Configure.Helper;
using HotelWise.Domain.Model.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace HotelWise.Data.Context.Configure.Entity;

/// <summary>
/// Mapeamento Fluent API para a entidade <see cref="ChatSessionHistory"/> no MySQL, configurando tipos de coluna, índices e serialização JSON de mensagens.
/// </summary>
public class ChatSessionHistoryConfiguration : IEntityTypeConfiguration<ChatSessionHistory>
{
    /// <summary>
    /// Aplica as regras de mapeamento e configuração de schema para <see cref="ChatSessionHistory"/>.
    /// </summary>
    /// <param name="builder">Construtor de configuração da entidade.</param>
    public void Configure(EntityTypeBuilder<ChatSessionHistory> builder)
    {
        builder.ToTable("ChatSessionHistory");
        PomeloCharSetHelper.AddCharSet(builder);

        // Definição de chave primária 
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.IdToken)
            .HasMaxLength(50)
            .IsRequired();

        // Configuração do DataHistory para serialização/desserialização automática
        builder.Property(e => e.PromptMessageHistory)
            .IsRequired()
            .HasMaxLength(EntityTypeConfigurationConstants.GetMaxLengthByTypeDataBase(ETypeDataBase.Mysql))
            .HasColumnType(EntityTypeConfigurationConstants.GetTypeTextByTypeDataBase(ETypeDataBase.Mysql))
            .HasConversion(
                v => JsonConvert.SerializeObject(v), // Serialização
                v => JsonConvert.DeserializeObject<HotelWise.Core.SDK.AI.DTO.PromptMessageVO[]>(v)! // Desserialização
            );
        // Outros campos
        builder.Property(e => e.CountMessages)
            .IsRequired();

        builder.Property(e => e.TotalTokensMessage)
          .IsRequired();

        builder.Property(e => e.SessionDateTime)
            .IsRequired();

        builder.Property(e => e.IdUser)
            .IsRequired(false);

        builder.HasIndex(e => e.IdToken).HasDatabaseName("IX_ChatSessionHistory_IdToken");

        builder.HasIndex(e => e.SessionDateTime).HasDatabaseName("IX_ChatSessionHistory_SessionDateTime");
    }
}