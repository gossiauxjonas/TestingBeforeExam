using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

using LesVoorbeeldClient.Models;
using LesVoorbeeldClient.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LesVoorbeeldClient.Controllers
{
    //Jquery consumption: http://www.binaryintellect.net/articles/1f93b9fd-585e-416e-b3ca-a60e13b5909c.aspx
    //.NET consumption:   http://www.binaryintellect.net/articles/6903087e-5e5c-445a-be20-cee0a7eb434e.aspx

    [Route("")]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        HttpClient client;

        public CustomerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:2017");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Route("")]
        [Route("[action]")]
        // GET: /<controller>/
        public IActionResult Index()
        {
            HttpResponseMessage response = client.GetAsync("/api/customers").Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<Customer> data = JsonConvert.DeserializeObject<List<Customer>>(stringData);

            return View(data.Select(d => new CustomerSpecial() {
                                Naam = $"{d.LastName} {d.FirstName}",
                                Land = d.Country
                            })
                            .OrderBy(d => d.Naam)
                            .ToList());
        }

        [Route("[action]")]
        public ActionResult Add(/*Customer customer*/)
        {
            Customer newCustomer = new Customer()
            {
                FirstName = "Jan",
                LastName = "Hoet",
                City = "Ghent",
                Country = "Belgium",
                Phone = "0777777777"
            };

            string stringData = JsonConvert.SerializeObject(newCustomer);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync("/api/customers", contentData).Result;
            return Content(response.Content.ReadAsStringAsync().Result.ToString());
        }
    }
}
