﻿using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IHotelRepository
    {
        Task<Hotel[]> GetAll();
        Task<Hotel[]> GetAllAsync();
        Task<Hotel?> GetByIdAsync(long id);
        Task AddAsync(Hotel hotel);
        Task AddRangeAsync(Hotel[] hotels);
        Task UpdateAsync(Hotel hotel);
        Task UpdateRangeAsync(Hotel[] hotel);
        Task DeleteAsync(long id);
        Task<int> GetTotalHotelsCountAsync();
        Task<Hotel[]> FetchHotelsAsync(int offset, int limit);
        Task<string[][]> GetAllTags(int offset, int limit);
    }
}