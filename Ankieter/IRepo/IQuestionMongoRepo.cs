using System.Collections.Generic;
using System.Threading.Tasks;
using Ankieter.Models;

namespace Ankieter.IRepo
{
    public interface IQuestionMongoRepo
    {
        Task<IEnumerable<Question>> GetAllAsync();
        Task<Question> GetByIdAsync(int id);
        Task CreateAsync(Question item);
        Task UpdateAsync(Question item);
        Task DeleteAsync(int id);
    }
}