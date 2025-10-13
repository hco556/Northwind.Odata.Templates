using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Shared.Models.v2;
using Northwind.api.Repositories.v2;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Northwind.api.Controllers.v2
{
    [ApiController]
    [Route("odata/v2/[controller]")]
    public class CustomersController : ODataController
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerRepository repository, ILogger<CustomersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [EnableQuery]
        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var customers = await _repository.GetAllAsync();
                return Ok(customers);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }

        [EnableQuery]
        [HttpGet("GetCustomerById/{key}")]
        public async Task<IActionResult> Get([FromRoute] string key)
        {
            try
            {
                var customer = await _repository.GetByIdAsync(key);
                if (customer == null)
                    return NotFound(new { Message = $"Customer with ID {key} not found." });
                return Ok(customer);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }

        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> Post([FromBody] Customer customer)
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

                var created = await _repository.AddAsync(customer);
                return Ok(created);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }

        [HttpPut("UpdateCustomer/{key}")]
        public async Task<IActionResult> Put([FromRoute] string key, [FromBody] Customer customer)
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

                var updated = await _repository.UpdateAsync(key, customer);
                if (updated == null)
                    return NotFound(new { Message = $"Customer with ID {key} not found." });
                return Ok(updated);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Code: {Code}, Message: {Message}", ex.HResult, ex.Message);
                throw new System.Exception($"Error Code: {ex.HResult}, Message: {ex.Message}", ex);
            }
        }

        [HttpDelete("DeleteCustomer/{key}")]
        public async Task<IActionResult> Delete([FromRoute] string key)
        {
            try
            {
                var deleted = await _repository.DeleteAsync(key);
                if (!deleted)
                    return NotFound(new { Message = $"Customer with ID {key} not found." });
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