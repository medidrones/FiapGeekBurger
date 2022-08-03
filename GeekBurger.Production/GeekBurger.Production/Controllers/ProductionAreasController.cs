using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GeekBurger.Production.Contract;
using GeekBurger.Production.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.Production.Controllers
{
    [Produces("application/json")]
    [Route("api/production")]
    public class ProductionAreasController : Controller
    {
        private IProductionAreaRepository _productionAreaRepository;
        private IMapper _mapper;

        public ProductionAreasController(IProductionAreaRepository productionAreaRepository, IMapper mapper)
        {
            _productionAreaRepository = productionAreaRepository;
            _mapper = mapper;
        }

        [HttpGet("areas")]
        public IActionResult GetAreas()
        {
            var availableProductionAreas = _productionAreaRepository.GetAvailableProductionAreas()?.ToList();

            if (availableProductionAreas.Count <= 0) return NotFound();

            var availableProductionAreasReturn = _mapper.Map<IEnumerable<ProductionAreaDTO>>(availableProductionAreas);

            return Ok(availableProductionAreasReturn);
        }
        
        [HttpGet("areas/{restrictionName}", Name = "GetProductionAreasWithoutRestriction")]
        public IActionResult GetProductionAreasWithoutRestriction(string restrictionName)
        {
            if (String.IsNullOrEmpty(restrictionName)) return BadRequest();

            var productionAreas = _productionAreaRepository.GetProductionAreasByRestrictionName(restrictionName)?.ToList();

            if (productionAreas.Count <= 0) return NotFound();

            var productionAreasReturn = _mapper.Map<IEnumerable<ProductionAreaDTO>>(productionAreas);

            return Ok(productionAreasReturn);
        }
    }
}
