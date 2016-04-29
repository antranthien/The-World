using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace TheWorld2.Models
{
    //DbContext
    public class WorldContext : IdentityDbContext<WorldUser>
    {
        // Don't rely on Database.EnsureCreated, use 'dnx ef database update' instead
        public DbSet<Trip> Trips { get; set; }

        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Startup.Configuration["Data:ConnectionString"];
            // specify the database information
            optionsBuilder.UseSqlServer(connString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
