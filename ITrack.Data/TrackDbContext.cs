/*
 *      @Author: yaile
 */

using ITrack.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITrack.Data
{
    public class TrackDbContext : IdentityDbContext<User>
    {
        public TrackDbContext(DbContextOptions<TrackDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<DevelopmentDirection> DevelopmentDirections { get; set; }
        public DbSet<PlanStage> PlanStages { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
