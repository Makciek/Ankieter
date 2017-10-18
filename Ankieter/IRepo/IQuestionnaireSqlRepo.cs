using System.Collections.Generic;
using System.Threading.Tasks;
using Ankieter.Models;

namespace Ankieter.IRepo
{
    public interface IQuestionnaireSqlRepo
    {
        Task<IEnumerable<QuestionnaireSql>> GetAllAsync();
        Task<QuestionnaireSql> GetByIdAsync(int id);
        Task CreateAsync(QuestionnaireSql item);
        Task UpdateAsync(QuestionnaireSql item);
        Task DeleteAsync(int id);
    }
}