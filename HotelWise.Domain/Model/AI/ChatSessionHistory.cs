using HotelWise.Domain.Dto;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelWise.Domain.Model.AI
{
    public class ChatSessionHistory
    {
        [Column("Id", Order = 0)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string IdToken { get; set; } = string.Empty;    // GUID será usado como chave primária
        public PromptMessageVO[] PromptMessageHistory { get; set; } = []; // Array de PromptMessageVO, EF cuidará da serialização        public int TotalTokens { get; set; }  // Total de tokens usados na sessão
        public int CountMessages { get; set; }
        public int TotalTokensMessage { get; set; }
        public DateTime SessionDateTime { get; set; } // Data e hora da sessão
        public long? IdUser { get; set; } // Opcional: pode armazenar o ID do usuário, se necessário
    }
}
