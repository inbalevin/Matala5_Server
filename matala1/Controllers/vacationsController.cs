using Microsoft.AspNetCore.Mvc;
using matala1.BL;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace matala1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vacationsController : ControllerBase
    {
        // GET: api/<vacationsController>
        [HttpGet]
        public IEnumerable<Vacation> Get()
        {
            Vacation v = new Vacation();
            return v.Read();
        }

        [HttpGet("getByDates/startDate/endDate")]
        public IActionResult getByDates(DateTime startDate, DateTime endDate)
        {
            List<Vacation> vacations = Vacation.ReadgetByDates(startDate, endDate);
          if(vacations.Count == 0)
            {
                return NotFound("sorry, try another date");

            }
          return Ok(vacations);
        }

        // GET api/<vacationsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<vacationsController>
        [HttpPost]
        public int Post([FromBody] Vacation vacation)
        {
          return vacation.Insert();
        }

        // PUT api/<vacationsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<vacationsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
