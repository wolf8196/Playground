using AzureSamples.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AzureSamples.Infrastructure
{
    public class AzureSamplesDbContext : DbContext
    {
        public AzureSamplesDbContext(DbContextOptions<AzureSamplesDbContext> options)
            : base(options)
        {
        }

        public DbSet<EventEntity> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}