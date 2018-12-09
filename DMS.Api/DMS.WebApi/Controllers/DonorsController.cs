using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Domain.Dtos.User;
using DMS.Utills.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DMS.WebApi.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/Donors")]
    public class DonorsController : Controller
    {
        [HttpGet]
        public async Task<IEnumerable<UserDto>> Get()
        {
            throw new DMSException("Not Implemented!");
        }
    }
}