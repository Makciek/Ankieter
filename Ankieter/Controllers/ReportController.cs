using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ankieter.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ankieter.Controllers
{
    public class ReportController : Controller
    {
        private readonly IFormService _formService;

        public ReportController(IFormService formService)
        {
            _formService = formService;
        }
        // GET: Report
        public async Task<ActionResult> Index()
        {
            var questionnares = await _formService.GetAllForms();
            return View(questionnares);
        }

        // GET: Report/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var report = await _formService.GetFormReport(id);
            return View(report);
        }

        // GET: Report/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Report/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Report/Edit/5
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

        // GET: Report/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Report/Delete/5
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