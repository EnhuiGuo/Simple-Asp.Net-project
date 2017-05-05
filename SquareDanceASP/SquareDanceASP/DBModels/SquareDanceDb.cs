using SquareDanceASP.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SquareDanceASP.DBModels
{
    public class SquareDanceDb : ApplicationDbContext
    {
        public SquareDanceDb()
        {

        }

        public DbSet<Pet> Pet { get; set; }
        public DbSet<PetImage> PetImage { get; set; }
        public DbSet<Sitter> Sitter { get; set; }
        public DbSet<ServiceAndRate> ServiceAndRate { get; set; }
        public DbSet<SitterImage> SitterImage { get; set; }
        public DbSet<SitterProfile> SitterProfile { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}