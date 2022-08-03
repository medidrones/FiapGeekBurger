using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekBurger.StoreCatalog.Infra.Repositories
{
    public class StoreCatalogDbContext : DbContext, IDbContext
    {
        public StoreCatalogDbContext(DbContextOptions<StoreCatalogDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductionAreas> ProductionsAreas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductionAreas>().ToTable("ProductionsAreas");
            modelBuilder.Entity<ProductionAreas>().HasKey(x => x.ProductionAreaId);
            modelBuilder.Entity<ProductionAreas>().Property(x => x.Name).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<ProductionAreas>().Property(x => x.Status).IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=yourservername;Database=ContactDB;User Id=youruserid;Password=yourpassword;Trusted_Connection=True;");
            }
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
}
