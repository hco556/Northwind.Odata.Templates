namespace Northwind.api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.OData.Deltas;
    using Microsoft.AspNetCore.OData.Formatter;
    using Microsoft.AspNetCore.OData.Query;
    using Microsoft.AspNetCore.OData.Routing.Controllers;
    using Northwind.api.Repositories;
    using Shared.Models;
    using System.Threading.Tasks;

    //[Route("odata/[controller]")]
    public class OrdersController : ODataController
    {
        private readonly IOrderRepository _repo;

        public OrdersController(IOrderRepository repo)
        {
            _repo = repo;
        }

        // GET odata/Orders
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_repo.GetOrders());
        }

        // GET odata/Orders(5)
        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var order = await _repo.GetOrderAsync(key);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // POST odata/Orders
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _repo.CreateOrderAsync(order);
            return Created(created);
        }

        // PUT odata/Orders(5)
        public async Task<IActionResult> Put(
            [FromODataUri] int key,
            [FromBody] Order order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (key != order.OrderId) return BadRequest();

            var exists = await _repo.OrderExistsAsync(key);
            if (!exists) return NotFound();

            await _repo.UpdateOrderAsync(order);
            return Updated(order);
        }

        // PATCH odata/Orders(5)
        public async Task<IActionResult> Patch(
            [FromODataUri] int key,
            [FromBody] Delta<Order> patch)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _repo.PatchOrderAsync(key, patch);
            if (updated == null) return NotFound();
            return Updated(updated);
        }

        // DELETE odata/Orders(5)
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var exists = await _repo.OrderExistsAsync(key);
            if (!exists) return NotFound();

            await _repo.DeleteOrderAsync(key);
            return NoContent();
        }
    }

}
