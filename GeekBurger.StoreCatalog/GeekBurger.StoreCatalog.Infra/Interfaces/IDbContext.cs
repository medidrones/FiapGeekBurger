using Microsoft.EntityFrameworkCore;

namespace GeekBurger.StoreCatalog.Infra.Interfaces
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();
    }
}
