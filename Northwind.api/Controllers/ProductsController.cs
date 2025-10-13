using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Shared.Models;
using Northwind.api.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Northwind.api.Controllers
{
    //[Route("odata/[controller]")]
    public class ProductsController : ODataController
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [EnableQuery]
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _repository.GetAllAsync();
               return Ok(products);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }

        [EnableQuery]
        [HttpGet("GetProductById/{key}")]
        public async Task<IActionResult> Get([FromRoute] int key)
        {
            try
            {
                Product product = await _repository.GetByIdAsync(key);
                if (product == null)
                    return NotFound(new { Message = $"Product with ID {key} not found." });
                return Ok(product);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                var created = await _repository.AddAsync(product);
                return Created(created);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }

        [HttpPut("UpdateProduct/{key}")]
        public async Task<IActionResult> Put([FromRoute] int key, [FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Validation failed",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                }

                var updated = await _repository.UpdateAsync(key, product);
                if (updated == null)
                    return NotFound(new { Message = $"Product with ID {key} not found." });
                return Updated(updated);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }

        [HttpDelete("DeleteProduct/{key}")]
        public async Task<IActionResult> Delete([FromRoute] int key)
        {
            try
            {
                var deleted = await _repository.DeleteAsync(key);
                if (!deleted)
                    return NotFound(new { Message = $"Product with ID {key} not found." });
                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }
    }
}