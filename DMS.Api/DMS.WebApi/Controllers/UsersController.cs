using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Domain.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DMS.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        // GET: api/Users
        [HttpGet]
        public IEnumerable<UserSDto> Get()
        {
            var users = new List<UserSDto>
            {
                new UserSDto { Id = 1, FirstName = "Wijitha", LastName = "Wijenayake", Email = "wijitha@yopmail.com", Active = true },
                new UserSDto { Id = 2, FirstName = "Widura", LastName = "Wijenayake", Email = "Widura@yopmail.com", Active = true },
                new UserSDto { Id = 3, FirstName = "Wickrama", LastName = "Wijenayake", Email = "Wickrama@yopmail.com", Active = true },
            };

            return users;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Users
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
