using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace TheWorld2.Models
{
    public class WorldUser : IdentityUser // inherit IdentityUser to support Identity
    {
        public DateTime FirstTrip { get; set; }
    }
}