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
    public class TrainController : ControllerBase
    {
        private readonly TrainScheduleManagementService trainScheduleManagementService;
        public TrainController(TrainScheduleManagementService _trainScheduleManagementService)
        {
            trainScheduleManagementService = _trainScheduleManagementService;
        }
        // GET: api/<TrainController>
        [HttpGet]
        public ActionResult Get()
        {
           return Ok(trainScheduleManagementService.getAllActiveTrainswithSchedules());
        }

        // GET api/<TrainController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TrainController>
        [HttpPost]
        public ActionResult Post(Train train)
        {
           var createdTrain =  trainScheduleManagementService.CreateTrainWithSchedule(train);
            if (createdTrain != null)
            {
                return Ok(createdTrain);
            }
            else {
                return BadRequest();
            } 
        }

        // PUT api/<TrainController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, Schedule schedule)
        {
           return Ok(trainScheduleManagementService.AddNewScheduleForExisitingTrain(id, schedule)); 
        }

        // DELETE api/<TrainController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id , Train train)
        {
            return Ok(trainScheduleManagementService.cancellingTrain(id,train));
        }
    }
}
