﻿using HotelWise.Data.Context;
using HotelWise.Data.Repository.Generic;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository
{
    public class HotelRepository : GenericRepositoryBase<Hotel, HotelWiseDbContextMysql>, IHotelRepository
    {
        public HotelRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options) : base(context, options)
        {
        } 
        public async Task<int> GetTotalHotelsCountAsync()
        {
            return await _dataset.AsNoTracking().CountAsync();
        }

        public async Task<Hotel[]> FetchHotelsAsync(int offset, int limit)
        {
            using (var context = CreateContext())
            {
                var resultRange = await context.Hotels.AsNoTracking().Skip(offset).Take(limit).ToArrayAsync();

                return resultRange;
            }
        }

        public async Task<string[][]> GetAllTagsAsync(int offset, int limit)
        {
            using (var context = CreateContext())
            {
                var resultRange = await context.Hotels.AsNoTracking().Select(h => h.Tags).Skip(offset).Take(limit).ToArrayAsync();

                return resultRange;
            }
        } 
    }
}
