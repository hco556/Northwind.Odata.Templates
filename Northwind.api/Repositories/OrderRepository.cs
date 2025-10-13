namespace Northwind.api.Repositories
{
    using Microsoft.AspNetCore.OData.Deltas;
    using Microsoft.EntityFrameworkCore;
    using Northwind.api.Data;
    using Shared.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderRepository : IOrderRepository
    {
        private readonly NorthwindContext _context;

        public OrderRepository(NorthwindContext context)
        {
            _context = context;
        }

        public IQueryable<Order> GetOrders()
        {
            // Let OData handle query composition
            return _context.Orders
                           .Include(o => o.Customer)
                           .Include(o => o.Employee)
                           .Include(o=> o.ShipViaNavigation)
                           .Include(o => o.OrderDetails);
        }

        public async Task<Order?> GetOrderAsync(int key)
        {
            return await _context.Orders
                                 .Include(o => o.OrderDetails)
                                 .FirstOrDefaultAsync(o => o.OrderId == key);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> PatchOrderAsync(int key, Delta<Order> patch)
        {
            var existing = await GetOrderAsync(key);
            if (existing == null) return null;

            patch.Patch(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteOrderAsync(int key)
        {
            var existing = await GetOrderAsync(key);
            if (existing == null) return;

            _context.Orders.Remove(existing);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> OrderExistsAsync(int key)
        {
            return await _context.Orders.AnyAsync(o => o.OrderId == key);
        }
    }

}
