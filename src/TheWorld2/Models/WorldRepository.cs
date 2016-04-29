using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld2.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddTrip(Trip newTrip)
        {
            _context.Add(newTrip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            try
            {
                return _context.Trips.OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError("Could not load the Trip list", ex);
                return null;
            }
            
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return _context.Trips
                .Include(t => t.Stops)
                .OrderBy(t => t.Name)
                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not load the Trip list with stops", ex);
                return null;
            }
            
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public Trip GetTripsByName(string tripName)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .FirstOrDefault(t => t.Name == tripName);
        }

        public void AddStop(string tripName, Stop newStop)
        {
            var trip = GetTripsByName(tripName);

            if (trip.Stops.Count == 0)
            {
                newStop.Order = 1;
            }
            else
            {
                newStop.Order = trip.Stops.Max(s => s.Order) + 1;
            }
            
            trip.Stops.Add(newStop);
            _context.Stops.Add(newStop);
        }
    }
}
