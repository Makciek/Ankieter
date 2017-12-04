//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Ankieter.Data;
//using Ankieter.IRepo;
//using Ankieter.Models;
//using Ankieter.Mongo;
//using Microsoft.Extensions.Options;
//using MongoDB.Driver;

//namespace Ankieter.Repo
//{
//    public class QuestionMongoRepo : IQuestionMongoRepo
//    {
//        private readonly ApplicationDbContext _context;

//        public QuestionMongoRepo(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<Question>> GetAllAsync()
//            => await _context.Questions.AsQueryable().ToListAsync();

//        public async Task<Question> GetByIdAsync(int id)
//            //=> await _context.Questions.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
//            => await _context.Questions.Find(x => x.Id == id).FirstOrDefaultAsync();

//        public async Task CreateAsync(Question item)
//            => await _context.Questions.InsertOneAsync(item);

//        public async Task UpdateAsync(Question item)
//            //=> await _context.Questions.UpdateOneAsync(x => x.Id == id, item);
//            => await _context.Questions.ReplaceOneAsync(x => x.Id == item.Id, item);

//        public async Task DeleteAsync(int id)
//            => await _context.Questions.DeleteOneAsync(x => x.Id == id);
//    }
//}