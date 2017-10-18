using System.Collections.Generic;
using System.Threading.Tasks;
using Ankieter.Data;
using Ankieter.IRepo;
using Ankieter.Models;
using MongoDB.Driver;

namespace Ankieter.Repo
{
    public class QuestionnaireMongoRepo : IQuestionnaireMongoRepo
    {
        private readonly ApplicationDbContext _context;

        public QuestionnaireMongoRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuestionnaireMongo>> GetAllAsync()
            => await _context.Questionnaires.AsQueryable().ToListAsync();

        public async Task<QuestionnaireMongo> GetByIdAsync(int id)
            //=> await _context.Answers.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
            => await _context.Questionnaires.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(QuestionnaireMongo item)
            => await _context.Questionnaires.InsertOneAsync(item);

        public async Task UpdateAsync(QuestionnaireMongo item)
            //=> await _context.Answers.UpdateOneAsync(x => x.Id == id, item);
            => await _context.Questionnaires.ReplaceOneAsync(x => x.Id == item.Id, item);

        public async Task DeleteAsync(int id)
            => await _context.Questionnaires.DeleteOneAsync(x => x.Id == id);
    }
}