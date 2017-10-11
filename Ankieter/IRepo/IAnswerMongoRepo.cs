using System.Collections.Generic;
using System.Threading.Tasks;
using Ankieter.Models;

namespace Ankieter.IRepo
{
    public interface IAnswerMongoRepo
    {
        Task<IEnumerable<Answer>> GetAllAsync();
        Task<Answer> GetByIdAsync(int id);
        Task CreateAsync(Answer item);
        Task UpdateAsync(Answer item);
        Task DeleteAsync(int id);
    }
}