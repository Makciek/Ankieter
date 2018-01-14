using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ankieter.Models;
using Ankieter.Models.Views.Forms;
using Ankieter.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ankieter.Controllers
{
    public class FormsController : Controller
    {
        private readonly IFormService _formService;
        private UserManager<ApplicationUser> _manager;

        public FormsController(IFormService formService, UserManager<ApplicationUser> manager)
        {
            _formService = formService;
            _manager = manager;
        }

        // GET: Forms
        public async Task<ActionResult> Index()
        {
            // todo: add paginatioN & per user filtering!!!
            var forms = await _formService.GetAllForms();
            return View(forms);
        }

        // GET: Forms/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var formDetailed = await _formService.SaveAnwsers(id);
            return View(formDetailed);
        }

        // GET: Forms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Forms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string answerStructure)
        {
            try
            {
                // TODO: Add insert logic here
                // security issues :P
                var user = await _manager.GetUserAsync(HttpContext.User);
                await _formService.SaveAnwsers(answerStructure, user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Forms/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Forms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Forms/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Forms/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}