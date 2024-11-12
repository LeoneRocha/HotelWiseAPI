﻿namespace HotelWise.Domain.Interfaces.SemanticKernel
{
    public interface IVectorStoreAdapter<TVector>
    {
        Task UpsertDataAsync(string nameCollection, TVector dataVector);
        Task UpsertDatasAsync(string nameCollection, TVector[] dataVectors);

        Task<TVector?> GetByKey(string nameCollection, ulong dataKey);

        Task<bool> Exists(string nameCollection, ulong dataKey);
        Task<TVector[]> SearchDatasAsync(string nameCollection, string searchText);
    }
}