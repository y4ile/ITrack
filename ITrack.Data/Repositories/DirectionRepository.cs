/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using ITrack.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITrack.Data.Repositories
{
    public class DirectionRepository : IDirectionRepository
    {
        private readonly TrackDbContext _db;

        public DirectionRepository(TrackDbContext context)
        {
            _db = context;
        }

        public async Task<bool> Create(DevelopmentDirection direction)
        {
            await _db.AddAsync(direction);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(DevelopmentDirection direction)
        {
            _db.Remove(direction);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<DevelopmentDirection> Update(DevelopmentDirection direction)
        {
            _db.Update(direction);
            await _db.SaveChangesAsync();

            return direction;
        }

        public async Task<List<DevelopmentDirection>> GetAll()
        {
            return await _db.DevelopmentDirections.ToListAsync();
        }

        public async Task<DevelopmentDirection> GetById(int id)
        {
            return await _db.DevelopmentDirections.FirstOrDefaultAsync(d => d.DirectionID == id);
        }
    }
}
