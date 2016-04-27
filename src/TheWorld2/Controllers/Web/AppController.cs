﻿using Microsoft.AspNet.Mvc;
using System.Linq;
using TheWorld2.Models;
using TheWorld2.Services;
using TheWorld2.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld2.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private WorldContext _context;

        public AppController(IMailService service, WorldContext context)
        {
            _mailService = service;
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var trips = _context.Trips.ToList();
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings:SiteEmailAddress"]; // Hierachy separation is a colon
                email = null;
                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("", "Could not send email, configuration problem");
                }
                else
                {
                    if (_mailService.SendMail(email,
                    email,
                    $"Contact Page from {model.Name} ({model.Email})",
                    model.Message))
                    {
                        ModelState.Clear(); //Clear the entire form
                        ViewBag.Message = "Mail sent. Thanks";
                    }
                }             
            }
            
            return View();
        }
    }
}
