using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld2.Models;
using System.Net;
using AutoMapper;
using TheWorld2.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld2.Controllers.Api
{
    // Walking down the tree from Trips to Stops
    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private ILogger<TripController> _logger;
        private IWorldRepository _repository;

        public StopController(IWorldRepository repository, ILogger<TripController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try
            {
                var result = _repository.GetTripsByName(tripName);

                if(result == null)
                {
                    return Json(null);
                }

                return Json(Mapper.Map<IEnumerable<StopViewModel>>(result.Stops.OrderBy(s => s.Order)));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get stops for a trip {tripName}", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
            
        }

        [HttpPost("")]
        public JsonResult Post(string tripName, [FromBody]StopViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Map to Stop model
                    var newStop = Mapper.Map<Stop>(viewModel);

                    // Look up Coordinates
                    // save to DB
                    _repository.AddStop(tripName, newStop);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<StopViewModel>(newStop));
                    }                  
                }
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json("false");
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new stop", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }
    }
}
