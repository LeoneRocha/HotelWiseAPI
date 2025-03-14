﻿using System.ComponentModel.DataAnnotations;
using HotelWise.Domain.Interfaces;

namespace HotelWise.Domain.Dto.AppConfig
{
    public sealed class RagConfig : IRagConfig
    {
        public const string ConfigSectionName = "Rag";

        [Required]
        public string AIChatService { get; set; } = string.Empty;

        [Required]
        public string AIEmbeddingService { get; set; } = string.Empty;

        [Required]
        public bool BuildCollection { get; set; } = true;

        [Required]
        public string CollectionName { get; set; } = string.Empty;

        [Required]
        public int DataLoadingBatchSize { get; set; } = 2;

        [Required]
        public int DataLoadingBetweenBatchDelayInMilliseconds { get; set; } = 0;

        [Required]
        public string[]? PdfFilePaths { get; set; }

        [Required]
        public string VectorStoreType { get; set; } = string.Empty;
        public SearchSettings SearchSettings { get; set; } = new();
    }
}