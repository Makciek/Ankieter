using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Ankieter.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Questionnaires = new HashSet<QuestionnaireSql>();
        }

        public string Name { get; set; }

        public virtual ICollection<QuestionnaireSql> Questionnaires { get; private set; }
    }
}