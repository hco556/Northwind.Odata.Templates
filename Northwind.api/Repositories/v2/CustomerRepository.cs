using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Northwind.api.Data;
using Shared.Models.v2;

namespace Northwind.api.Repositories.v2
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly NorthwindContext _context;

        public CustomerRepository(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Set<Customer>()
                .Include(c => c.Orders)
                .Include(c => c.CustomerTypes)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(string id)
        {
            return await _context.Set<Customer>()
                .Include(c => c.Orders)
                .Include(c => c.CustomerTypes)
                .FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            _context.Set<Customer>().Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> UpdateAsync(string id, Customer customer)
        {
            var existing = await _context.Set<Customer>().FindAsync(id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(customer);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var customer = await _context.Set<Customer>().FindAsync(id);
            if (customer == null) return false;

            _context.Set<Customer>().Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}