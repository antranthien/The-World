using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld2.Models
{
    public class WorldContextSeedData
    {
        private WorldContext _context;
        private UserManager<WorldUser> _userManager;

        public WorldContextSeedData(WorldContext context, UserManager<WorldUser> userManager) //UserManager: for User identity
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task SeedData()
        {
            if (await _userManager.FindByEmailAsync("antt@yahoo.com") == null)
            {
                var newUser = new WorldUser
                {
                    UserName = "testuser",
                    Email = "testuser@yahoo.com"
                };

                // Use UserManager to create a user
                await _userManager.CreateAsync(newUser, "Test!123");
            }
            if (!_context.Trips.Any())
            {
                var norwayTrip = new Trip()
                {
                    Name = "Norway Trip",
                    Created = DateTime.UtcNow,
                    UserName = "testuser",
                    Stops = new List<Stop>()
                    {
                        new Stop()
                        {
                            Name = "Halden", Arrival = new DateTime(2016, 3, 1), Latitude = 59.132996, Longitude = 11.387457, Order = 1
                        },
                        new Stop()
                        {
                            Name = "Oslo", Arrival = new DateTime(2016, 3, 5), Latitude = 59.913869, Longitude = 10.752245, Order = 2
                        },
                        new Stop()
                        {
                            Name = "Bergen", Arrival = new DateTime(2016, 3, 8), Latitude = 60.391263, Longitude = 5.322054, Order = 3
                        },
                        new Stop()
                        {
                            Name = "Trondheim", Arrival = new DateTime(2016, 3, 12), Latitude = 63.430515, Longitude = 10.395053, Order = 4
                        },
                        new Stop()
                        {
                            Name = "Tromsø", Arrival = new DateTime(2016, 3, 15), Latitude = 69.649205, Longitude = 18.955324, Order = 5
                        }
                    }
                };

                _context.Trips.Add(norwayTrip);
                _context.Stops.AddRange(norwayTrip.Stops);

                var scandinaviaTrip = new Trip()
                {
                    Name = "Scandinavia Trip",
                    Created = DateTime.UtcNow,
                    UserName = "testuser",
                    Stops = new List<Stop>()
                    {
                        new Stop()
                        {
                            Name = "Copenhagen, Denmark", Arrival = new DateTime(2016, 3, 18), Latitude = 55.676097, Longitude = 12.568337, Order = 1
                        },
                        new Stop()
                        {
                            Name = "Oslo, Norway", Arrival = new DateTime(2016, 3, 20), Latitude = 59.913869, Longitude = 10.752245, Order = 2
                        },
                        new Stop()
                        {
                            Name = "Malmø, Sweden", Arrival = new DateTime(2016, 3, 22), Latitude = 55.604981, Longitude = 13.003822, Order = 3
                        },
                        new Stop()
                        {
                            Name = "Helsinki, Finland", Arrival = new DateTime(2016, 3, 25), Latitude = 60.169856, Longitude = 24.938379, Order = 4
                        }
                    }
                };

                _context.Trips.Add(scandinaviaTrip);
                _context.Stops.AddRange(scandinaviaTrip.Stops);

                await _context.SaveChangesAsync();
            }
        }
    }
}
