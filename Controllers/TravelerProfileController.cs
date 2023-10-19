using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportManagmentSystemAPI.Models;
using TransportManagmentSystemAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportManagmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelerProfileController : ControllerBase
    {
        private readonly TravallerProfileService travallerProfileService;
        public TravelerProfileController(TravallerProfileService _travallerProfile)
        {
            travallerProfileService = _travallerProfile;
        }
        // GET: api/<TravelerProfileController>
        [HttpGet]
        public ActionResult<TravallerProfile> Get(bool isActive) 
        {
            var activeProfile = travallerProfileService.DisplayAllActiveProfile(isActive);
            if (activeProfile != null)
            {
                return Ok(activeProfile);
            }
            else 
            {
                return NotFound();
            }
        }

        // GET api/<TravelerProfileController>/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var profile = travallerProfileService.GetTravallerProfileByNic(id);
            if (profile != null)
            {
                return Ok(profile);
            }
            else 
            {
                return NotFound();
            }
                
        }

        // POST api/<TravelerProfileController>
        [HttpPost]
        public ActionResult Post(TravallerProfile _travallerProfile)
        {
           var createdAccount = travallerProfileService.CreateUpdateTravellerProfile(_travallerProfile);
            if (createdAccount != null)
            {
                return Ok(createdAccount);
            }
            else
            {
                return Conflict();
            }
        }

        // PUT api/<TravelerProfileController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, TravallerProfile _travallerProfile)
        {
            
            var updatedAccount = travallerProfileService.ManageActivationTravellerProfile(id, _travallerProfile);
            if (updatedAccount != null)
            {
                return Ok(updatedAccount);
            }
            else 
            {
                return BadRequest();
            }
        }

        // DELETE api/<TravelerProfileController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
           var deletedNic =  travallerProfileService.DeletedTravellerProfile(id);
            if (deletedNic != null)
            {
                return Ok("Deleted" + deletedNic);
            }
            else 
            {
                return BadRequest();
            }
        }
    }
}
