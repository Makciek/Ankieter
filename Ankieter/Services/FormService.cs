using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ankieter.Data;
using Ankieter.IRepo;
using Ankieter.Models;
using Ankieter.Models.Views.Forms;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Ankieter.Services
{
    public class FormService : IFormService
    {
        private readonly ApplicationDbContext _context;
        private readonly IQuestionnaireMongoRepo _questionnaireMongoRepo;
        private readonly IQuestionnaireSqlRepo _questionnaireSqlRepo;

        public FormService(ApplicationDbContext context, IQuestionnaireMongoRepo questionnaireMongoRepo,
            IQuestionnaireSqlRepo questionnaireSqlRepo)
        {
            _context = context;
            _questionnaireMongoRepo = questionnaireMongoRepo;
            _questionnaireSqlRepo = questionnaireSqlRepo;
        }

        public async Task<bool> CreateForm(CreatedForm form)
        {
            var formDes = JsonConvert.DeserializeObject<List<dynamic>>(form.FormStructure);

            //var anwserdefault = new List<AnwserForm>();
            //foreach (var formItem in formDes)
            //{
            //    var anwsersOptions = new List<AnwserForm.AnswerOption>();
            //    if(formItem.Type == )

            //    anwserdefault.Add(new AnwserForm()
            //    {
            //        Id = formItem.Id,
            //        Answer = "",
            //        AnswerName = "",
            //        Answers = anwsersOptions
            //    });
            //}


            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var mongoId = ObjectId.GenerateNewId(DateTime.UtcNow);
                    var anwsersMongoId = ObjectId.GenerateNewId(DateTime.UtcNow);

                    var sqlModel = new QuestionnaireSql()
                    {
                        Name = form.Name,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        QuestionnaireMongoId = mongoId.ToString(),
                        AnwsersStatisticsMongoId = anwsersMongoId.ToString()
                    };
                    await _context.QuestionnaireSqls.AddAsync(sqlModel);
                    await _context.SaveChangesAsync();

                    AnwserStaticticsMongo anwserStaticticsMongo = null;
                    var mongoRecord = new QuestionnaireMongo()
                    {
                        Id = mongoId,
                        Questions = BsonSerializer.Deserialize<BsonArray>(form.FormStructure),
                        QuestionnaireSqlId = sqlModel.Id.ToString(),
                    };

                    anwserStaticticsMongo = new AnwserStaticticsMongo()
                    {
                        Id = anwsersMongoId,
                      //  Anwsers = ,
                        QuestionnaireMongo = mongoRecord,
                        QuestionnaireSqlId = sqlModel.Id.ToString()
                    };

                    mongoRecord.AnwsersStaticticsMongo = anwserStaticticsMongo;

                    await _context.QuestionnairesMongo.InsertOneAsync(mongoRecord);
                    await _context.AnwserStaticticsMongo.InsertOneAsync(anwserStaticticsMongo);
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
            return (await _questionnaireSqlRepo.GetAllAsync()).Select(x => new FormViewModel()
            {
                Id = x.Id,
                MongoId = x.QuestionnaireMongoId,
                Name = x.Name,
                Modification = x.UpdateDate
            });
        }

        public async Task<FormDetailsViewModel> SaveAnwsers(int id)
        {
            var sqlModel = await _questionnaireSqlRepo.GetByIdAsync(id);
            var mongoModel = await _questionnaireMongoRepo.GetByIdAsync(sqlModel.QuestionnaireMongoId);

            var result = new FormDetailsViewModel()
            {
                Id = sqlModel.Id,
                Modification = sqlModel.UpdateDate,
                MongoId = sqlModel.QuestionnaireMongoId,
                Name = sqlModel.Name,
                JsonStructure = mongoModel.Questions.ToJson()
            };
            return result;
        }

        public async Task<bool> SaveAnwsers(string anwserJson, ApplicationUser user)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var isAnwsered = await _context.AnswersSql.AnyAsync(x => x.User.Id == user.Id);
                    if (isAnwsered)
                        return false;

                    var mongoId = ObjectId.GenerateNewId(DateTime.UtcNow);

                    var anwsers = JsonConvert.DeserializeObject<List<AnwserForm>>(anwserJson);

                    var anwsersSql = new AnswerSql()
                    {
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        User = user,
                        AnwserMongoId = mongoId.ToString()
                    };
                    await _context.AnswersSql.AddAsync(anwsersSql);
                    await _context.AnswersMongo.InsertOneAsync(new AnswerMongo()
                    {
                        Id = mongoId,
                        Anwsers = BsonSerializer.Deserialize<BsonArray>(anwserJson),
                        AnwsersSqlId = anwsersSql.Id.ToString()
                    });

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
    }
}