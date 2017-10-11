using System.Collections.Generic;
using System.Threading.Tasks;
using Ankieter.Data;
using Ankieter.IRepo;
using Ankieter.Models;
using Ankieter.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ankieter.Repo
{
    public class AnswerMongoRepo : IAnswerMongoRepo
    {
        private readonly ApplicationDbContext _context;

        public AnswerMongoRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Answer>> GetAllAsync()
            => await _context.Answers.AsQueryable().ToListAsync();

        public async Task<Answer> GetByIdAsync(int id)
            //=> await _context.Answers.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
            => await _context.Answers.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Answer item)
            => await _context.Answers.InsertOneAsync(item);

        public async Task UpdateAsync(Answer item)
            //=> await _context.Answers.UpdateOneAsync(x => x.Id == id, item);
            => await _context.Answers.ReplaceOneAsync(x => x.Id == item.Id, item);

        public async Task DeleteAsync(int id)
            => await _context.Answers.DeleteOneAsync(x => x.Id == id);
    }
}