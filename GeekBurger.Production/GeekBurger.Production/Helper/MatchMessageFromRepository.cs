using System.Collections.Generic;
using AutoMapper;
using GeekBurger.Production.Contract;
using GeekBurger.Production.Model;
using GeekBurger.Production.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GeekBurger.Production.Helper
{
    public class MatchMessageFromRepository : IMappingAction<EntityEntry<ProductionArea>, ProductionAreaChangedMessage>
    {
        private IProductionAreaRepository _productionAreaRepository;

        public MatchMessageFromRepository(IProductionAreaRepository productionAreaRepository)
        {
            _productionAreaRepository = productionAreaRepository;
        }

        public void Process(EntityEntry<ProductionArea> source, ProductionAreaChangedMessage destination)
        {
            destination.ProductionArea = new ProductionAreaDTO
            {
                Id = source.Entity.Id,
                Name = source.Entity.Name,
                Status = source.Entity.Status,
                Restrictions = new List<string>()
            };

            foreach (Restriction restriction in source.Entity.Restrictions)
            {
                destination.ProductionArea.Restrictions.Add(restriction.Name);
            }

            destination.State = ProductionAreaChangedMessage.ProductionAreaState.Added;
        }
    }
}
