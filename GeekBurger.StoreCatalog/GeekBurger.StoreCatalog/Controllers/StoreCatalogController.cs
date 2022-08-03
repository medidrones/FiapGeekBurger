using System;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.StoreCatalog.Controllers
{
    /// <summary>
    /// Store Catalog endpoints
    /// </summary>
    public class StoreCatalogController : Controller
    {
        private IStoreCatalogCore _storeCatalogCore;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storeCatalogCore"></param>
        public StoreCatalogController(IStoreCatalogCore storeCatalogCore)
        {
            _storeCatalogCore = storeCatalogCore;
        }

        /// <summary>
        /// Check if dependence services are available
        /// </summary>
        /// <response code="200">Returned successfully</response>
        /// <response code="500">Returned services not available</response>
        /// <response code="503">Returned internal server error</response>
        [HttpGet("statusServer/")]
        public IActionResult GetStatusServer()
        {
            var result = new OperationResult<bool>();

            try
            {
                result.Data = _storeCatalogCore.StatusServers();
                result.Success = true;

                if (result.Data)
                    return Ok(result);
                else
                {
                    result.Message = "Services not available";
                    return StatusCode(503, result);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return StatusCode(500, result);
            }
        }
    }
}
