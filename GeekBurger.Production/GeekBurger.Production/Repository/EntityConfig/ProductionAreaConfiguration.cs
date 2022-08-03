using GeekBurger.Production.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeekBurger.Production.Repository.EntityConfig
{
    public class ProductionAreaConfiguration : IEntityTypeConfiguration<ProductionArea>
    {
        public void Configure(EntityTypeBuilder<ProductionArea> builder)
        {
            builder.ToTable("TBProductionArea");
        }
    }
}
