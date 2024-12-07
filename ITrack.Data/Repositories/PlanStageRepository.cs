/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using ITrack.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITrack.Data.Repositories
{
    public class PlanStageRepository : IPlanStageRepository
    {
        private readonly TrackDbContext _db;

        public PlanStageRepository(TrackDbContext context)
        {
            _db = context;
        }

        public async Task<bool> Create(PlanStage planStage)
        {
            await _db.AddAsync(planStage);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(PlanStage planStage)
        {
            _db.Remove(planStage);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<PlanStage> Update(PlanStage planStage)
        {
            _db.Update(planStage);
            await _db.SaveChangesAsync();

            return planStage;
        }

        public async Task<List<PlanStage>> GetAll()
        {
            return await _db.PlanStages.Include(ps => ps.Status)
                .Include(ps => ps.Plan)
                .ToListAsync();
        }

        public async Task<PlanStage> GetById(int id)
        {
            return await _db.PlanStages.Include(ps => ps.Status)
                .Include(ps => ps.Plan)
                .FirstOrDefaultAsync(ps => ps.StageID == id);
        }
    }
}
