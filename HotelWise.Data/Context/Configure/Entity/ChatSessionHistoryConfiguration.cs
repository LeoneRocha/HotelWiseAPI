using HotelWise.Data.Context.Configure.Helper;
using HotelWise.Domain.Constants;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns;
using HotelWise.Domain.Model.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace HotelWise.Data.Context.Configure.Entity
{
    public class ChatSessionHistoryConfiguration : IEntityTypeConfiguration<ChatSessionHistory>
    {
        public void Configure(EntityTypeBuilder<ChatSessionHistory> builder)
        {
            builder.ToTable("ChatSessionHistory");
            HelperCharSet.AddCharSet(builder);

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
                    v => JsonConvert.DeserializeObject<PromptMessageVO[]>(v)! // Desserialização
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
}