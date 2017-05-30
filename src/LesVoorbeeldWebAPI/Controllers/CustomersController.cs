using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LesVoorbeeldWebAPI.Entities;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LesVoorbeeldWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private OrderDBContext db;

        public CustomersController(OrderDBContext context)
        {
            db = context;
        }

        //GET: api/Customers/
        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                List<Customer> customers = db.Customer
                                        .Select(c => c)
                                        .Distinct()
                                        .ToList();
                return Ok(customers);
            }
            catch (Exception e)
            {
                return BadRequest($"O oh, something ({e.Message}) went wrong");
            }
        }


        //GET: api/Customers/4
        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult Get(int id)
        {
            try
            {
                Customer customer = db.Customer
                                        .FirstOrDefault(c => c.Id == id);
                if (customer == null)
                {
                    return NotFound($"Customer with id {id} is not found...");
                }
                return Ok(customer);
            }
            catch (Exception e)
            {
                return BadRequest($"O oh, something ({e.Message}) went wrong");
            }
        }

        //POST: api/Customers/
        [HttpPost("")]
        public IActionResult Post([FromBody]Customer customer)
        {
            try
            {
                if (customer != null)
                {
                    Customer newCustomer = new Customer()
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        City = customer.City,
                        Country = customer.Country,
                        Phone = customer.Phone
                    };
                    db.Customer.Add(newCustomer);
                    db.SaveChanges();
                    return Created(Url.Link("GetCustomer", new { id = newCustomer.Id }), newCustomer);
                } else                
                {
                    return NotFound("No Customer info retrieved...");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"O oh, something ({e.Message}) went wrong");
            }
        }

        //PUT: api/customers/45
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Customer customer)
        {
            try
            {
                Customer oldCustomer = db.Customer.FirstOrDefault(c => c.Id == id);
                if (oldCustomer != null)
                {
                    oldCustomer.FirstName = customer.FirstName ?? oldCustomer.FirstName;
                    oldCustomer.LastName = customer.LastName ?? oldCustomer.LastName;
                    oldCustomer.City = customer.City ?? oldCustomer.City;
                    oldCustomer.Country = customer.Country ?? oldCustomer.Country;
                    oldCustomer.Phone = customer.Phone ?? oldCustomer.Phone;
 
                    db.Customer.Update(oldCustomer);
                    db.SaveChanges();

                    return Ok(oldCustomer);
                }
                else
                {
                    return NotFound("No Customer found...");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"O oh, something ({e.Message}) went wrong");
            }
        }

        //DELETE: api/customers/45
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Customer oldCustomer = db.Customer.FirstOrDefault(c => c.Id == id);
                if (oldCustomer != null)
                {
                    db.Customer.Remove(oldCustomer);
                    db.SaveChanges();

                    return Ok($"Customer with id {id} has been succesfully deleted");
                }
                else
                {
                    return NotFound($"No Customer found with id {id}");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"O oh, something ({e.Message}) went wrong");
            }
        }
    }
}
