using System;
using System.Collections.Generic;
using System.Linq;
using GeekBurger.Production.Model;
using GeekBurger.Production.Service;
using Microsoft.EntityFrameworkCore;

namespace GeekBurger.Production.Repository
{
    /// <summary>
    /// Repositório da área de produção
    /// </summary>
    public class ProductionAreaRepository : IProductionAreaRepository
    {
        private ProductionContext _context;
        private IProductionAreaChangedService _productionAreaChangedService;
        private IOrderFinishedService _orderFinishedService;

        /// <summary>
        /// Inicialização com o contexto necessário e as interfaces já implementadas
        /// </summary>
        /// <param name="context"></param>
        /// <param name="productionAreaChangedService"></param>
        /// <param name="orderFinishedService"></param>
        public ProductionAreaRepository(
            ProductionContext context, 
            IProductionAreaChangedService productionAreaChangedService, 
            IOrderFinishedService orderFinishedService)
        {
            _context = context;
            _productionAreaChangedService = productionAreaChangedService;
            _orderFinishedService = orderFinishedService;
        }

        /// <summary>
        /// Busca de áreas de produção disponíveis (ligadas)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductionArea> GetAvailableProductionAreas()
        {
            return _context.ProductionAreas?.Include(r => r.Restrictions).Where(pa => pa.Status == true).ToList();
        }

        /// <summary>
        /// Busca áreas de produção que possuam respectiva restrição
        /// </summary>
        /// <param name="restrictionName"></param>
        /// <returns></returns>
        public IEnumerable<ProductionArea> GetProductionAreasByRestrictionName(string restrictionName)
        {
            return _context.ProductionAreas?
                .Include(pa => pa.Restrictions)
                .Where(pa => !pa.Restrictions.Any(r => r.Name.Equals(restrictionName, StringComparison.InvariantCultureIgnoreCase))).ToList();
        }
        
        /// <summary>
        /// Busca área de produção por ID
        /// </summary>
        /// <param name="productionAreaId"></param>
        /// <returns></returns>
        public ProductionArea GetProductionAreaById(Guid productionAreaId)
        {
            return _context.ProductionAreas?
                .Include(r => r.Restrictions)
                .FirstOrDefault(productionArea => productionArea.Id == productionAreaId);
        }

        /// <summary>
        /// Criação de nova área de produção
        /// </summary>
        /// <param name="productionArea"></param>
        /// <returns></returns>
        public bool CreateProductionArea(ProductionArea productionArea)
        {
            productionArea.Id = new Guid();
            _context.ProductionAreas.Add(productionArea);

            return true;
        }

        /// <summary>
        /// Atualização de área de produção
        /// </summary>
        /// <param name="productionAreaId"></param>
        /// <param name="updatedProductionArea"></param>
        /// <returns></returns>
        public bool UpdateProductionArea(Guid productionAreaId, ProductionArea updatedProductionArea)
        {
            var productionAreaToUpdate = _context.ProductionAreas?
                .Include(r => r.Restrictions)
                .FirstOrDefault(pa => pa.Id == productionAreaId);

            if (EqualityComparer<ProductionArea>.Default.Equals(productionAreaToUpdate, default(ProductionArea))) return false;
            
            productionAreaToUpdate.Name = updatedProductionArea.Name;
            productionAreaToUpdate.Restrictions = updatedProductionArea.Restrictions;
            _context.ProductionAreas.Update(productionAreaToUpdate);

            return true;
        }

        /// <summary>
        /// Remoção de área de produção
        /// </summary>
        /// <param name="productionAreaId"></param>
        /// <returns></returns>
        public bool RemoveProductionArea(Guid productionAreaId)
        {
            var productionAreaToDelete = _context.ProductionAreas?.FirstOrDefault(pa => pa.Id == productionAreaId);

            if (EqualityComparer<ProductionArea>.Default.Equals(productionAreaToDelete, default(ProductionArea))) return false;
            _context.ProductionAreas?.Remove(productionAreaToDelete);

            return true;
        }

        /// <summary>
        /// Atualização dos dados do repositório de Produção com as devidas modificações.
        /// </summary>
        public void Save()
        {
            _productionAreaChangedService.AddToMessageList(_context.ChangeTracker.Entries<ProductionArea>());
            _context.SaveChanges();
            _productionAreaChangedService.SendMessagesAsync();
        }
    }
}
