using Ankieter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ankieter.Data;

namespace Ankieter.Controllers
{
    [Authorize]
    public class ApplicationRoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ApplicationDbContext context;

        public ApplicationRoleController(RoleManager<ApplicationRole> roleManager, ApplicationDbContext _context)
        {
            this.roleManager = roleManager;
            context = _context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<ApplicationRoleListViewModel> model = new List<ApplicationRoleListViewModel>();
            model = roleManager.Roles.Select(r => new ApplicationRoleListViewModel
            {
                RoleName = r.Name,
                Id = r.Id,
                Description = r.Description
            }).ToList();

            foreach(var role in model)
            {
                int rolesCount = context.UserRoles.Count(x => x.RoleId == role.Id);
                role.NumberOfUsers = rolesCount;
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddEditApplicationRole(string id)
        {
            ApplicationRoleViewModel model = new ApplicationRoleViewModel();
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationRole applicationRole = await roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    model.Id = applicationRole.Id;
                    model.RoleName = applicationRole.Name;
                    model.Description = applicationRole.Description;
                }
            }
            return View("_AddEditApplicationRole", model);
        }
        [HttpPost]
        public async Task<IActionResult> AddEditApplicationRole(string id, ApplicationRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isExist = !String.IsNullOrEmpty(id);
                ApplicationRole applicationRole = isExist ? await roleManager.FindByIdAsync(id) :
               new ApplicationRole
               {
                   CreatedDate = DateTime.UtcNow
               };
                applicationRole.Name = model.RoleName;
                applicationRole.Description = model.Description;
                applicationRole.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                IdentityResult roleRuslt = isExist ? await roleManager.UpdateAsync(applicationRole)
                                                    : await roleManager.CreateAsync(applicationRole);
                if (roleRuslt.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteApplicationRole(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationRole applicationRole = await roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    ApplicationRoleViewModel role = new ApplicationRoleViewModel()
                    {
                        Description = applicationRole.Description,
                        Id = applicationRole.Id,
                        RoleName = applicationRole.Name
                    };
                    return View("_DeleteApplicationRole", role);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApplicationRole([Bind("Description,RoleName,Id")]ApplicationRoleViewModel role)
        {
            if (role != null)
            {
                ApplicationRole applicationRole = await roleManager.FindByIdAsync(role.Id);
                if (applicationRole != null)
                {
                    IdentityResult roleRuslt = roleManager.DeleteAsync(applicationRole).Result;
                    if (roleRuslt.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View(role);
            }
            return NotFound();
        }
    }
}
