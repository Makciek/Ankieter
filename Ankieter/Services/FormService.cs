using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ankieter.Data;
using Ankieter.IRepo;
using Ankieter.Models;
using Ankieter.Models.Views.Forms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Ankieter.Services
{
    public class FormService : IFormService
    {
        private readonly ApplicationDbContext _context;
        private readonly IQuestionnaireMongoRepo _questionnaireMongoRepo;
        private readonly IQuestionnaireSqlRepo _questionnaireSqlRepo;

        public FormService(ApplicationDbContext context, IQuestionnaireMongoRepo questionnaireMongoRepo, IQuestionnaireSqlRepo questionnaireSqlRepo)
        {
            _context = context;
            _questionnaireMongoRepo = questionnaireMongoRepo;
            _questionnaireSqlRepo = questionnaireSqlRepo;
        }

        public async Task<bool> CreateForm(CreatedForm form)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.QuestionnaireSqls.AddAsync(new QuestionnaireSql()
                    {
                        Name = form.Name,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow
                    });

                    await _context.SaveChangesAsync();

                    var mondoRecord = new QuestionnaireMongo()
                    {
                        Id = ObjectId.Parse(form.Id.ToString().PadLeft(24, '0')),
                        Questions = BsonSerializer.Deserialize<BsonArray>(form.FormStructure)
                    };

                    await _context.QuestionnairesMongo.InsertOneAsync(mondoRecord);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return true;
        }

        public async Task<IEnumerable<FormViewModel>> GetAllForms()
        {
            return (await _questionnaireSqlRepo.GetAllAsync()).Select(x=>new FormViewModel()
            {
                Id = x.Id,
                MongoId = x.QuestionnaireMongoId,
                Name = x.Name,
                Modification = x.UpdateDate
            });
        }
    }
}