using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using TheWorld2.Models;
using TheWorld2.Services;
using TheWorld2.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld2.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IWorldRepository _repository;

        public AppController(IMailService service, IWorldRepository repository)
        {
            _mailService = service;
            _repository = repository;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Trips()
        {
            var trips = _repository.GetAllTrips();
            return View(trips);
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
        public IActionResult Contact(ContactViewModel viewModel)
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
                        $"Contact Page from {viewModel.Name} ({viewModel.Email})",
                        viewModel.Message))
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
