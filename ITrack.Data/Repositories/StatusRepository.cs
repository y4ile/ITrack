/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using ITrack.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITrack.Data.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly TrackDbContext _db;

        public StatusRepository(TrackDbContext context)
        {
            _db = context;
        }

        public async Task<bool> Create(Status status)
        {
            await _db.AddAsync(status);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Status status)
        {
            _db.Remove(status);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<Status> Update(Status status)
        {
            _db.Update(status);
            await _db.SaveChangesAsync();

            return status;
        }

        public async Task<List<Status>> GetAll()
        {
            return await _db.Statuses.ToListAsync();
        }

        public async Task<Status> GetById(int id)
        {
            return await _db.Statuses.FirstOrDefaultAsync(d => d.StatusId == id);
        }
    }
}
