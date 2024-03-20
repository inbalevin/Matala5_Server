using matala1.BL;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace matala1.Controllers
{
   [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            User u = new User();
            return u.Read();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public int Post([FromBody] User user)
        {
            
            return user.Insert();
        }


        [HttpPost("LoginUser")]
        public User LoginUser(User user)
        {
            DBservices dbs = new DBservices();
            User loggedInUser = dbs.LoginUs(user.Email, user.Password);
            return loggedInUser;
            
        }

      

        [HttpPut("Update")]
        public int Put([FromBody] User user)
        {
            DBservices dbs = new DBservices();
            return dbs.Update(user);
        }


        //DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        
        ////avgPriceForCityByMonth
        [HttpGet("month/{month}")]
        public List<Object> GetAvgPriceByCityAndMonth(int month)
        {
            DBservices dbs = new DBservices();

            List<Object> avgPrices = dbs.GetAverage(month);

            return avgPrices;
        }

    }
}
