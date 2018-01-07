using System.Collections.Generic;
using System.Threading.Tasks;
using Ankieter.Data;
using Ankieter.IRepo;
using Ankieter.Models;
using MongoDB.Bson;
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
            => await _context.QuestionnairesMongo.AsQueryable().ToListAsync();

        public async Task<QuestionnaireMongo> GetByIdAsync(string id)
            //=> await _context.Answers.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
            => await _context.QuestionnairesMongo.Find(x => x.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();

        public async Task CreateAsync(QuestionnaireMongo item)
            => await _context.QuestionnairesMongo.InsertOneAsync(item);

        public async Task UpdateAsync(QuestionnaireMongo item)
            //=> await _context.Answers.UpdateOneAsync(x => x.Id == id, item);
            => await _context.QuestionnairesMongo.ReplaceOneAsync(x => x.Id == item.Id, item);

        public async Task DeleteAsync(string id)
            => await _context.QuestionnairesMongo.DeleteOneAsync(x => x.Id == ObjectId.Parse(id));
    }
}