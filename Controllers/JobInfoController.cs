 using Microsoft.AspNetCore.Mvc;

namespace JobExchange.Controllers
{
    public class JobInfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
