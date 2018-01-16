using System.ComponentModel.DataAnnotations;

namespace Ankieter.Models
{
    public class ApplicationRoleViewModel
    {
        public string Id { get; set; }

        [Display(Name ="Nazwa roli")]
        public string RoleName { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }
    }
}
