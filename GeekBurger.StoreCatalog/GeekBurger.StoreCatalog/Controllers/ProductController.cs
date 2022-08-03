using System;
using System.Collections.Generic;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Core.Interfaces;
using GeekBurger.StoreCatalog.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.StoreCatalog.Controllers
{
    /// <summary>
    /// Products endpoints
    /// </summary>
    public class ProductController : Controller
    {
        private readonly IProductCore _productCore;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productCore"></param>
        public ProductController(IProductCore productCore)
        {
            _productCore = productCore;
        }

        /// <summary>
        /// Return all products for user with restrictions. 
        /// (Os produtos não estão filtrados porque o serviço de production area não ficou pronto a tempo)
        /// </summary>
        /// <param name="user">Object User with restrictions</param>
        /// <response code="200">Returned successfully</response>
        /// <response code="400">Returned bad request</response>
        [HttpGet]
        [Route("products/{user}")]
        public async System.Threading.Tasks.Task<IActionResult> GetProductsAsync([FromBody] User user)
        {
            var result = new OperationResult<IEnumerable<Product>>();

            try
            {
                result.Data = _productCore.GetProductsFromUser(user);
                var wrapper = new ProductsByUserWrapper(user, result.Data);
                var sendMessage = new SendMessage();
                await sendMessage.SendMessagesAsync(wrapper);

                result.Success = true;
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
                return BadRequest(result);
            }
        }
    }
}
