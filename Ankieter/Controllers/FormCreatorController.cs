using System.Threading.Tasks;
using Ankieter.Models.Views.Forms;
using Ankieter.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ankieter.Controllers
{
    [Authorize]
    public class FormCreatorController : Controller
    {
        private readonly IFormService _formService;
        
        public FormCreatorController(IFormService formService)
        {
            _formService = formService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreatedForm form)
        {
            if (!await _formService.CreateForm(form))
            {
                ViewData["Error"] = "Something went wrong";
            }
            return View();
        }
    }
}