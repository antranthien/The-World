using AutoMapper;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using TheWorld2.Models;
using TheWorld2.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld2.Controllers.Api
{
    [Route("api/trips")]
    public class TripController : Controller
    {
        private ILogger<TripController> _logger;
        private IWorldRepository _repository;

        public TripController(IWorldRepository repository, ILogger<TripController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        // GET: api/values
        [HttpGet("")]
        public JsonResult Get()
        {
            var results = Mapper.Map<IEnumerable<TripViewModel>>(_repository.GetAllTripsWithStops());
            return Json(results);
        }

        // POST api/values
        [HttpPost("")]
        public JsonResult Post([FromBody]TripViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTrip = Mapper.Map<Trip>(viewModel);

                    // save to DB
                    _logger.LogInformation("Saving a new trip");
                    _repository.AddTrip(newTrip);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<TripViewModel>(newTrip));
                    }                    
                }

                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json("false");
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new trip", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
           
        }
    }
}
