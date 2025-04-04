﻿using HotelWise.Domain.Dto.IA;

namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAssistantService
    {
        Task<float[]?> GenerateEmbeddingAsync(string text);

        Task<AskAssistantResponse[]?> AskAssistant(AskAssistantRequest request);
        void SetUserId(long id);
    }
}
