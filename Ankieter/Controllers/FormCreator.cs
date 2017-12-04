using System;
using System.Threading.Tasks;
using Ankieter.IRepo;
using Ankieter.Models;
using Ankieter.Models.Forms;
using Ankieter.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace Ankieter.Controllers
{
    public class FormCreator : Controller
    {
        private readonly IFormService _formService;

        public FormCreator(IFormService formService)
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