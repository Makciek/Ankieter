using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ankieter.Models;
using Ankieter.Models.Views.Forms;
using Ankieter.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ankieter.Controllers
{
    [Authorize]
    public class FormCreatorController : Controller
    {
        private readonly IFormService _formService;
        private UserManager<ApplicationUser> _userManager;

        public FormCreatorController(UserManager<ApplicationUser> userManager, IFormService formService)
        {
            _formService = formService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<ApplicationUserListViewModel> model = new List<ApplicationUserListViewModel>();
            model = _userManager.Users.Select(u => new ApplicationUserListViewModel
            {
                Id = u.Id,
                Name = u.UserName ?? u.Name,
                Email = u.Email
            }).ToList();

            ViewBag.Users = model;

            return View(/*model*/);
        }

        public class User
        {
            public string Id { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreatedForm inputs)
        {
            if (!await _formService.CreateForm(inputs))
            {
                ViewData["Error"] = "Something went wrong";
            }
            return View();
        }
    }
}