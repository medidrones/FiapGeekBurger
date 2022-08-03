using GeekBurger.Production.Model;
using GeekBurger.Production.Repository.EntityConfig;
using Microsoft.EntityFrameworkCore;

namespace GeekBurger.Production.Repository
{
    public class ProductionContext : DbContext
    {
        public ProductionContext(DbContextOptions<ProductionContext> options) : base(options)
        {
        }

        public DbSet<ProductionArea> ProductionAreas { get; set; }

        /// <summary>
        /// Método chamado no momento que o modelo está sendo criado, é utilizado para configurar a estrutura de dados da área de produção
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductionAreaConfiguration());
        }
    }
}
