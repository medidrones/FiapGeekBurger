using System.Collections.Generic;
using AutoMapper;
using GeekBurger.Production.Contract;
using GeekBurger.Production.Model;
using GeekBurger.Production.Repository;

namespace GeekBurger.Production.Helper
{
    public class MatchRepositoryFromCRUD : IMappingAction<ProductionAreaCRUD, ProductionArea>
    {
        private IProductionAreaRepository _productionAreaRepository;

        public MatchRepositoryFromCRUD(IProductionAreaRepository productionAreaRepository)
        {
            _productionAreaRepository = productionAreaRepository;
        }

        public void Process(ProductionAreaCRUD source, ProductionArea destination)
        {
            destination.Restrictions = new List<Restriction>();

            foreach (string restriction in source.Restrictions)
            {
                destination.Restrictions.Add(new Restriction { Name = restriction });
            }
        }
    }
}
