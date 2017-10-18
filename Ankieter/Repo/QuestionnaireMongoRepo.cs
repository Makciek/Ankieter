using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ankieter.Data;
using Ankieter.IRepo;
using Ankieter.Models;
using Ankieter.Models.Forms;
using Ankieter.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Ankieter.Repo
{
    public class QuestionnaireMongoRepo : IQuestionnaireMongoRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IAnswerMongoRepo _answerMongoRepo;
        private readonly IQuestionMongoRepo _questionMongoRepo;

        public QuestionnaireMongoRepo(ApplicationDbContext context, IAnswerMongoRepo answerMongoRepo, IQuestionMongoRepo questionMongoRepo)
        {
            _context = context;
            _answerMongoRepo = answerMongoRepo;
            _questionMongoRepo = questionMongoRepo;
        }

        public async Task<IEnumerable<Questionnaire>> GetAllAsync()
            => await _context.Questionnaires.AsQueryable().ToListAsync();

        public async Task<Questionnaire> GetByIdAsync(int id)
            //=> await _context.Answers.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
            => await _context.Questionnaires.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Questionnaire item)
            => await _context.Questionnaires.InsertOneAsync(item);

        public async Task UpdateAsync(Questionnaire item)
            //=> await _context.Answers.UpdateOneAsync(x => x.Id == id, item);
            => await _context.Questionnaires.ReplaceOneAsync(x => x.Id == item.Id, item);

        public async Task DeleteAsync(int id)
            => await _context.Questionnaires.DeleteOneAsync(x => x.Id == id);

        public async Task CreatedFormToQuestionnaireAndCreate(CreatedForm form)
        {
            var item = JsonConvert.DeserializeObject(form.FormStructure);

            //await CreateAsync(item);
            throw new NotImplementedException();
        }
    }
}