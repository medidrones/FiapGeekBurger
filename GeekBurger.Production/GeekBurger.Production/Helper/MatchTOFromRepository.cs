using System.Linq;
using AutoMapper;
using GeekBurger.Production.Contract;
using GeekBurger.Production.Model;
using GeekBurger.Production.Repository;

namespace GeekBurger.Production.Helper
{
    public class MatchTOFromRepository : IMappingAction<ProductionArea, ProductionAreaDTO>
    {
        private IProductionAreaRepository _productionAreaRepository;
        public MatchTOFromRepository(IProductionAreaRepository productionAreaRepository)
        {
            _productionAreaRepository = productionAreaRepository;
        }

        public void Process(ProductionArea source, ProductionAreaDTO destination)
        {
            destination.Restrictions = source.Restrictions?.Select(r => r.Name).ToList();
        }
    }
}
