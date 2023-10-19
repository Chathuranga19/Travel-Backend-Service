using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TransportManagmentSystemAPI.Models;
using TransportManagmentSystemAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportManagmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;


        }
        // GET: api/<ReservationController>
        [HttpGet]
        public ActionResult Get()
        {
           List<Reservation> reservationList =  _reservationService.DisplayAllReservation(null);
            if (reservationList != null)
            {
                return Ok(reservationList);
            }
            else 
            {
               return  NotFound();
            }
        }

        // GET api/<ReservationController>/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            List<Reservation> reservationList = _reservationService.DisplayAllReservation(id);
            if (reservationList != null)
            {
                return Ok(reservationList);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<ReservationController>
        [HttpPost]
        public ActionResult Post(Reservation reservation)
        {
            var resrvation = _reservationService.CreateReservation(reservation);
            if (resrvation.ContainsKey(100))
            {
                return BadRequest(resrvation[100]);
            }
            else if (resrvation.ContainsKey(200))
            {
                return BadRequest(resrvation[200]);
            }
            else if (resrvation.ContainsKey(400))
            {
                return Ok(resrvation[400]);
            }
            else 
            {
                return BadRequest();
            }
        }

        // PUT api/<ReservationController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, Reservation reservation)
        {
            var resrvation = _reservationService.CancelledReservation(id,reservation);
            if (resrvation.ContainsKey(100))
            {
                return Ok(resrvation[100]);
            }
            else 
            {
                return BadRequest(resrvation[500]);
            }
        }

        // DELETE api/<ReservationController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {

            // Log the method call
            Console.WriteLine($"Delete method called with ID: {id}");

            var deletionResult = _reservationService.DeleteReservation(id);

            if (deletionResult.ContainsKey(200))
            {
                return Ok("Reservation deleted successfully");
            }
            else if (deletionResult.ContainsKey(404))
            {
                return NotFound(deletionResult[404]);
            }
            else if (deletionResult.ContainsKey(400))
            {
                return NotFound(deletionResult[400]);
            }
            else
            {
                return BadRequest("Failed to delete reservation");
            }
        }
    }
}
