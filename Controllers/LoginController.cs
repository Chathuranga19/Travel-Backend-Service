using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportManagmentSystemAPI.DBconfig;
using TransportManagmentSystemAPI.Models;
using TransportManagmentSystemAPI.Services;

namespace TransportManagmentSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginservice;
        public LoginController(LoginService loginService)
        {
            _loginservice = loginService;
        }

        [HttpGet]
        public Task<User> Get()
        {
            return null;
        }

        [HttpPost]
        public ActionResult Post(User user)
        {
            if (user.Nic != null && user.Password != null)
            {
                var ValdatedAccount = _loginservice.MakeLogin(user);
                if (ValdatedAccount != null)
                {
                    return Ok(ValdatedAccount);
                }
                else
                {
                    return Unauthorized();
                }
            }
            else {
               return  BadRequest("Please enter NIC and Password");
            }
            
        }
    }
}
