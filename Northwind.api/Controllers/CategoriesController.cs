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
    //[ApiController]
    //[Route("odata/[controller]")]
    public class CategoriesController : ODataController
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryRepository repository, ILogger<CategoriesController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categories = await _repository.GetAllAsync();
                return Ok(categories);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<IActionResult> Get([FromRoute] int key)
        {
            try
            {
                var category = await _repository.GetByIdAsync(key);
                if (category == null)
                    return NotFound(new { Message = $"Category with ID {key} not found." });
                return Ok(category);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category)
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

                var created = await _repository.AddAsync(category);
                return Created(created);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }

        [HttpPut("{key}")]
        public async Task<IActionResult> Put([FromRoute] int key, [FromBody] Category category)
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

                var updated = await _repository.UpdateAsync(key, category);
                if (updated == null)
                    return NotFound(new { Message = $"Category with ID {key} not found." });
                return Updated(updated);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete([FromRoute] int key)
        {
            try
            {
                var deleted = await _repository.DeleteAsync(key);
                if (!deleted)
                    return NotFound(new { Message = $"Category with ID {key} not found." });
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