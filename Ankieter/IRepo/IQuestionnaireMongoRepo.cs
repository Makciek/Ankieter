using System.Collections.Generic;
using System.Threading.Tasks;
using Ankieter.Models;
using Ankieter.Models.Forms;

namespace Ankieter.IRepo
{
    public interface IQuestionnaireMongoRepo
    {
        Task<IEnumerable<Questionnaire>> GetAllAsync();
        Task<Questionnaire> GetByIdAsync(int id);
        Task CreateAsync(Questionnaire item);
        Task UpdateAsync(Questionnaire item);
        Task DeleteAsync(int id);
        Task CreatedFormToQuestionnaireAndCreate(CreatedForm form);
    }
}