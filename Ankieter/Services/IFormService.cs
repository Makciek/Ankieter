using System.Threading.Tasks;
using Ankieter.Models.Views.Forms;

namespace Ankieter.Services
{
    public interface IFormService
    {
        Task<bool> CreateForm(CreatedForm form);
    }
}