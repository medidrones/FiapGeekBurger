using System;
using System.Collections.Generic;
using GeekBurger.Production.Model;

namespace GeekBurger.Production.Repository
{
    public interface IProductionAreaRepository
    {
        IEnumerable<ProductionArea> GetAvailableProductionAreas();
        IEnumerable<ProductionArea> GetProductionAreasByRestrictionName(string restrictionName);

        ProductionArea GetProductionAreaById(Guid productionAreaId);

        bool CreateProductionArea(ProductionArea productionArea);
        bool UpdateProductionArea(Guid productionAreaId, ProductionArea productionArea);
        bool RemoveProductionArea(Guid productionAreaId);

        void Save();
    }
}
