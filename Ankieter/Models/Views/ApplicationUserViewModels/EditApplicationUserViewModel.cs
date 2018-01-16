using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ankieter.Models
{
    public class EditApplicationUserViewModel
    {
        public string Id { get; set; }
        
        [Display(Name = "Imię")]
        public string Name { get; set; }

        public string Email { get; set; }

        public List<SelectListItem> ApplicationRoles { get; set; }

        [Display(Name = "Rola")]
        public string ApplicationRoleId { get; set; }
    }
}
