using matala1.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace matala1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatsController : ControllerBase
    {
        // GET: api/<FlatsController>
        [HttpGet]
        public IEnumerable<Flat> Get()
        {
            Flat f = new Flat();
             return f.Read();
        }

        //[HttpGet("/{id}")]
        //public List<Flat> ReadFrom(int id)
        //{
        //    Flat f = new Flat();
        //    return f.Read(id);
        //}
        [HttpGet("search")]
        public IActionResult GetUnderPrice(double price, string city)
        {
            Flat f = new Flat();
            List<Flat> flatsToReturn = Flat.ReadCityUnderCertainPrice(price,city);
            if(flatsToReturn.Count == 0)
            {
                return NotFound("Sorry, no such flats");
            }
            return Ok(flatsToReturn);
        }


        // GET api/<FlatsController>/5
        [HttpGet("{id}")]
        public List<Flat> ReadFrom(int id)
        {
            Flat f = new Flat();
            return f.Read(id);
        }


        // POST api/<FlatsController>
        [HttpPost]
       
        public int Post([FromBody] Flat flat)
        {
            return flat.Insert();
        }

        // PUT api/<FlatsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FlatsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
