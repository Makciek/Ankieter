using Microsoft.AspNetCore.Mvc;

namespace Ankieter.Controllers
{
    public class FormCreator : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}