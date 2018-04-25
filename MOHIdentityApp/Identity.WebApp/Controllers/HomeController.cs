using Microsoft.AspNetCore.Mvc;

namespace Identity.WebApp.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            
            return View();
        }

        
    }
}