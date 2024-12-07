/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using ITrack.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITrack.Data.Repositories
{
    public class StudyPlanRepository : IStudyPlanRepository
    {
        private readonly TrackDbContext _db;

        public StudyPlanRepository(TrackDbContext context)
        {
            _db = context;
        }

        public async Task<bool> Create(StudyPlan studyPlan)
        {
            await _db.AddAsync(studyPlan);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(StudyPlan studyPlan)
        {
            _db.Remove(studyPlan);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<StudyPlan> Update(StudyPlan studyPlan)
        {
            _db.Update(studyPlan);
            await _db.SaveChangesAsync();

            return studyPlan;
        }

        public async Task<List<StudyPlan>> GetAll()
        {
            return await _db.StudyPlans.Include(s => s.User)
                .Include(s => s.Direction)
                .Include(s => s.PlanStages)
                .ThenInclude(ps => ps.Status)
                .ToListAsync();
        }

        public async Task<StudyPlan> GetById(int id)
        {
            return await _db.StudyPlans.Include(s => s.User)
                .Include(s => s.Direction)
                .Include(s => s.PlanStages)
                .ThenInclude(ps => ps.Status)
                .FirstOrDefaultAsync(s => s.PlanID == id);
        }
    }
}
