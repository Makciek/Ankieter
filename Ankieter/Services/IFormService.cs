using System.Threading.Tasks;
using Ankieter.Models.Forms;

namespace Ankieter.Services
{
    public interface IFormService
    {
        Task<bool> CreateForm(CreatedForm form);
    }
}