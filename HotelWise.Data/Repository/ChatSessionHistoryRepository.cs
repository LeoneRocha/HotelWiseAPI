using HotelWise.Data.Context;
using HotelWise.Data.Repository.Generic;
using HotelWise.Domain.Interfaces.Entity.IA;
using HotelWise.Domain.Model.AI;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Repository
{
    public class ChatSessionHistoryRepository : GenericRepositoryBase<ChatSessionHistory, HotelWiseDbContextMysql>, IChatSessionHistoryRepository
    {
        public ChatSessionHistoryRepository(HotelWiseDbContextMysql context, DbContextOptions<HotelWiseDbContextMysql> options) : base(context, options)
        {
        }

        public Task DeleteByIdTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<ChatSessionHistory?> GetByIdTokenAsync(string token)
        {
            return await _dataset.AsNoTracking().FirstOrDefaultAsync(et=> et.IdToken.Equals(token));
        }
    }
} 