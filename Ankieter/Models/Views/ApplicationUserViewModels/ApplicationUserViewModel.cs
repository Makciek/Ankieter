using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ankieter.Models
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name="Powtórz hasło")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Imię")]
        public string Name { get; set; }

        public string Email { get; set; }

        public List<SelectListItem> ApplicationRoles { get; set; }

        [Display(Name = "Rola")]
        public string ApplicationRoleId { get; set; }
    }
}
