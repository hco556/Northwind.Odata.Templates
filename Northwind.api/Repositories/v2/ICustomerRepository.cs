using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models.v2;

namespace Northwind.api.Repositories.v2
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(string id);
        Task<Customer> AddAsync(Customer customer);
        Task<Customer?> UpdateAsync(string id, Customer customer);
        Task<bool> DeleteAsync(string id);
    }
}