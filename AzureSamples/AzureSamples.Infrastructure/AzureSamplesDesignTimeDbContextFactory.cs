using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AzureSamples.Infrastructure
{
    public class AzureSamplesDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AzureSamplesDbContext>
    {
        public AzureSamplesDbContext CreateDbContext(string[] args)
        {
            // Create options for the DbContext
            var optionsBuilder = new DbContextOptionsBuilder<AzureSamplesDbContext>();
            optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=AzureSamples;Integrated Security=True;TrustServerCertificate=True");

            // Return a new instance of AzureSamplesDbContext with the options
            return new AzureSamplesDbContext(optionsBuilder.Options);
        }
    }
}