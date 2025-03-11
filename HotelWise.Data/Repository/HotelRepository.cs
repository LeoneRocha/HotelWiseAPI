using HotelWise.Data.Context;
using HotelWise.Data.Repository.Generic;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository
{
    public class HotelRepository : GenericRepositoryBase<Hotel>, IHotelRepository
    {
        public HotelRepository(HotelWiseDbContextMysql context) : base(context)
        {
        }

        public async Task<int> GetTotalHotelsCountAsync()
        {
            return await _dataset.AsNoTracking().CountAsync();
        }

        public async Task<Hotel[]> FetchHotelsAsync(int offset, int limit)
        {
            return await _dataset.AsNoTracking().Skip(offset).Take(limit).ToArrayAsync();
        }

        public async Task<string[][]> GetAllTagsAsync(int offset, int limit)
        {
            return await _dataset.AsNoTracking().Select(h => h.Tags).Skip(offset).Take(limit).ToArrayAsync();
        }
    }
}
