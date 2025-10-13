using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Northwind.api.Data;

using Shared.Models.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace OData.API.Controllers
{
    //https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022
    //[Route("api/[controller]")]
    //[ApiController]

   // [Route("odata")]
    public class CustomersController : ODataController
    {
        private readonly ODataAPIDbContext _context;

        public CustomersController(ODataAPIDbContext db)
        {
            _context = db;
        }
        public ActionResult<IQueryable<Customer>> Get()
        {
            return Ok(_context.Customers);
        }
        //// GET: api/Customers
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        //{
        //    return await _context.Customers.ToListAsync();
        //}

        // GET: api/Customers/5
        public ActionResult<Customer> Get([FromRoute] int key)
        {
            var customer = _context.Customers.SingleOrDefault(d => d.Id == key);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        //[HttpGet("odata/Customers({id})")]
        //public ActionResult GetCustomer([FromRoute] int id)
        //{
        //    var customer = _context.Customers.SingleOrDefault(d => d.Id == id);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(customer);
      //  }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Customer>> GetCustomer(int id)
        //{
        //    var customer = await _context.Customers.FindAsync(id);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return customer;
        //}
        public ActionResult Post([FromBody] Customer customer)
        {
            _context.Customers.Add(customer);

            _context.SaveChanges();

            return Created(customer);
        }


        public ActionResult Patch([FromRoute] int key, [FromBody] Delta<Customer> delta)
        {
            var customer = _context.Customers.SingleOrDefault(d => d.Id == key);

            if (customer == null)
            {
                return NotFound();
            }

            delta.Patch(customer);

            _context.SaveChanges();

            return Updated(customer);
        }

        public ActionResult Put([FromRoute] int key, [FromBody] Customer updatedCustomer)
        {
            var customer = _context.Customers.SingleOrDefault(d => d.Id == key);

            if (customer == null)
            {
                return NotFound();
            }

            customer.Name = updatedCustomer.Name;
            customer.CustomerType = updatedCustomer.CustomerType;
            customer.CreditLimit = updatedCustomer.CreditLimit;
            customer.CustomerSince = updatedCustomer.CustomerSince;

            _context.SaveChanges();

            return Updated(customer);
        }

        public ActionResult Delete([FromRoute] int key)
        {
            var customer = _context.Customers.SingleOrDefault(d => d.Id == key);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            _context.SaveChanges();

            return NoContent();
        }
        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCustomer(int id, Customer customer)
        //{
        //    if (id != customer.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(customer).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CustomerExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Customers
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        //{
        //    _context.Customers.Add(customer);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        //}

        //// DELETE: api/Customers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCustomer(int id)
        //{
        //    var customer = await _context.Customers.FindAsync(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Customers.Remove(customer);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CustomerExists(int id)
        //{
        //    return _context.Customers.Any(e => e.Id == id);
        //}
    }
}
