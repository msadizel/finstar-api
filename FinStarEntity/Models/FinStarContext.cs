using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace FinStarEntity.Models
{
    public class FinStarContext : DbContext
    {
        public FinStarContext()
        {
        }
        [ActivatorUtilitiesConstructor]
        public FinStarContext(DbContextOptions<FinStarContext> options)
            : base(options)
        {
        }

        public DbSet<Items> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //            if (!optionsBuilder.IsConfigured)
            //            {

            //#if DEBUG
            //                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DebugConnection"].ConnectionString);
            //#elif RELEASE
            //                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ReleaseConnection"].ConnectionString);
            //#endif
            //            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Items>(entity =>
            {
                entity.ToTable("Items");
                entity.Property(x => x.ID).ValueGeneratedOnAdd();
                entity.HasKey(x => x.ID);
            });
        }
    }

}
