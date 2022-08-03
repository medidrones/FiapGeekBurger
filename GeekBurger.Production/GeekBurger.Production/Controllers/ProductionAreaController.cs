using System;
using System.Collections.Generic;
using AutoMapper;
using GeekBurger.Production.Contract;
using GeekBurger.Production.Model;
using GeekBurger.Production.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.Production.Controllers
{
    [Produces("application/json")]
    [Route("api/productionarea")]
    public class ProductionAreaController : Controller
    {
        private IProductionAreaRepository _productionAreaRepository;
        private IMapper _mapper;

        public ProductionAreaController(IProductionAreaRepository productionAreaRepository, IMapper mapper)
        {
            _productionAreaRepository = productionAreaRepository;
            _mapper = mapper;
        }

        [HttpGet("{productionAreaId}", Name = "GetProductionArea")]
        public IActionResult GetProductionArea(Guid productionAreaId)
        {
            if (productionAreaId == null || productionAreaId == Guid.Empty) return BadRequest();

            var productionArea = _productionAreaRepository.GetProductionAreaById(productionAreaId);

            if (EqualityComparer<ProductionArea>.Default.Equals(productionArea, default(ProductionArea))) return NotFound();

            var productionAreaTO = _mapper.Map<ProductionAreaDTO>(productionArea);

            return Ok(productionAreaTO);
        }

        [HttpPost()]
        public IActionResult CreateProductionArea([FromBody] ProductionAreaCRUD newProductionArea)
        {
            if (newProductionArea == null || EqualityComparer<ProductionAreaCRUD>.Default.Equals(newProductionArea, default(ProductionAreaCRUD))) return BadRequest();

            var _productionArea = _mapper.Map<ProductionArea>(newProductionArea);
            var resultProductionAreaCreated = _productionAreaRepository.CreateProductionArea(_productionArea);

            if (!resultProductionAreaCreated) return NotFound();

            _productionAreaRepository.Save();

            var productionAreaTO = _mapper.Map<ProductionAreaDTO>(_productionArea);

            return CreatedAtRoute("GetProductionArea", new { productionAreaId = productionAreaTO.Id }, productionAreaTO);
        }

        [HttpPut("{productionAreaId}", Name = "UpdateProductionArea")]
        public IActionResult UpdateProductionArea(Guid productionAreaId, [FromBody] ProductionAreaCRUD updatedProductionArea)
        {
            if (productionAreaId == null || productionAreaId == Guid.Empty || EqualityComparer<ProductionAreaCRUD>.Default.Equals(updatedProductionArea, default(ProductionAreaCRUD))) return BadRequest();

            var _updatedProductionArea = _mapper.Map<ProductionArea>(updatedProductionArea);
            var resultProductionAreaUpdated = _productionAreaRepository.UpdateProductionArea(productionAreaId, _updatedProductionArea);

            if (!resultProductionAreaUpdated) return NotFound();

            _updatedProductionArea.Id = productionAreaId;
            _productionAreaRepository.Save();

            var productionAreaTO = _mapper.Map<ProductionAreaDTO>(_updatedProductionArea);

            return CreatedAtRoute("GetProductionArea", new { productionAreaId = productionAreaTO.Id }, productionAreaTO);
        }

        [HttpDelete("{productionAreaId}", Name = "RemoveProductionArea")]
        public IActionResult RemoveProductionArea(Guid productionAreaId)
        {
            if (productionAreaId == null || productionAreaId == Guid.Empty) return BadRequest();

            var resultProductionAreaRemoved = _productionAreaRepository.RemoveProductionArea(productionAreaId);

            if (!resultProductionAreaRemoved) return NotFound();

            _productionAreaRepository.Save();

            return Ok();
        }
    }
}
