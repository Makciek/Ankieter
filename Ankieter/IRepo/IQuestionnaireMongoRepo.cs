using System.Collections.Generic;
using System.Threading.Tasks;
using Ankieter.Models;

namespace Ankieter.IRepo
{
    public interface IQuestionnaireMongoRepo
    {
        Task<IEnumerable<QuestionnaireMongo>> GetAllAsync();
        Task<QuestionnaireMongo> GetByIdAsync(int id);
        Task CreateAsync(QuestionnaireMongo item);
        Task UpdateAsync(QuestionnaireMongo item);
        Task DeleteAsync(int id);
    }
}