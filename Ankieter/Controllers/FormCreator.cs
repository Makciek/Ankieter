using Ankieter.Models.Forms;
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
        
        [HttpPost]
        public IActionResult Index(CreatedForm form)
        {
            return View();
        }
    }
}