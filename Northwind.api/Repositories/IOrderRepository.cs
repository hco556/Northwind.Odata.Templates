namespace Northwind.api.Repositories
{
    using Microsoft.AspNetCore.OData.Deltas;
    using Shared.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IOrderRepository
    {
        // Exposes IQueryable for OData to apply filters, sorting, paging, etc.
        IQueryable<Order> GetOrders();

        // Fetch a single order by key
        Task<Order?> GetOrderAsync(int key);

        // Create a new order
        Task<Order> CreateOrderAsync(Order order);

        // Full replace of an existing order
        Task UpdateOrderAsync(Order order);

        // Partial update (PATCH) of an existing order
        Task<Order?> PatchOrderAsync(int key, Delta<Order> patch);

        // Delete an order by key
        Task DeleteOrderAsync(int key);

        // Check existence (optional helper)
        Task<bool> OrderExistsAsync(int key);
    }

}
