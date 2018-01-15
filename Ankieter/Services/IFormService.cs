using System.Collections.Generic;
using System.Threading.Tasks;
using Ankieter.Models;
using Ankieter.Models.Views.Forms;

namespace Ankieter.Services
{
    public interface IFormService
    {
        Task<bool> CreateForm(CreatedForm form);
        Task<IEnumerable<FormViewModel>> GetAllForms();
        Task<FormDetailsViewModel> GetForm(int id);
        Task<bool> SaveAnwsers(string anwserJson, ApplicationUser user);
    }
}