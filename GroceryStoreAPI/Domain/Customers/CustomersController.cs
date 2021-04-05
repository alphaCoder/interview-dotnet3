using System.Net;
using GroceryStoreModels;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStoreAPI.Domain.Customers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomerManager _manager;
        public CustomersController(ICustomerManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var r = _manager.GetAll();
            if (r.Success)
            {
                return Ok(r.Data);
            }
            return Problem(statusCode: (int?) HttpStatusCode.InternalServerError);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var r = _manager.Get(id);
            if (r.Success)
            {
                return Ok(r.Data);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Post([FromBody]string customerName)
        {
            var r = _manager.Add(customerName);
            if (r.Success)
            {
                return Ok(r.Data);
            }

            if (r.ErrorMessage == HttpErrorCode.BadRequest)
            {
                return BadRequest();
            }

            return Conflict();
        }

        [HttpPut]
        public IActionResult Put([FromBody] Customer customer)
        {
            var r = _manager.Update(customer);
            if (r.Success)
            {
                return Ok(r.Data);
            }

            if (r.ErrorMessage == HttpErrorCode.BadRequest)
            {
                return BadRequest();
            }

            return NotFound();
        }
    }
}
