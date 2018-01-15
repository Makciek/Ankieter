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
            var formDes = JsonConvert.DeserializeObject<List<QuestionnaireMongo.Question>>(form.FormStructure);

            var anwserdefault = new List<AnwserStatisticsModel>();
            foreach (var formItem in formDes)
            {
                /* Types
                   { id: 0, name: "Not Selected" },
                   { id: 1, name: "Text" },
                   { id: 2, name: "Textarea" },
                   { id: 3, name: "Radio" },
                   { id: 4, name: "Checkbox" },
                   { id: 5, name: "Dropdown" },                 
                 */

                if (formItem.Type.Id < 3) // we do not save text anwsers in statistics
                    continue;

                var anwserStat = new AnwserStatisticsModel
                {
                    QuestionId = formItem.Id,
                    AnswerIdToNumberOfAnwsers = new List<KeyValuePair<int, int>>()
                };

                foreach (var clicableOption in formItem.ClicableOptions)
                {
                    anwserStat.AnswerIdToNumberOfAnwsers.Add(new KeyValuePair<int, int>(clicableOption.Id, 0));
                }
                anwserdefault.Add(anwserStat);
            }

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
                        Questions = formDes,
                        QuestionnaireSqlId = sqlModel.Id.ToString(),
                        AnwsersStaticticsMongoId = anwsersMongoId
                    };

                    anwserStaticticsMongo = new AnwserStaticticsMongo()
                    {
                        Id = anwsersMongoId,
                        NumberOfAnwsers = 0,
                        Anwsers = anwserdefault,
                        QuestionnaireMongoId = mongoRecord.Id,
                        QuestionnaireSqlId = sqlModel.Id.ToString()
                    };

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

        public async Task<FormDetailsViewModel> GetForm(int id)
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
            var def = new
            {
                Id = "",
                Items = new List<AnwserForm>()
            };
            var anwsersDes = JsonConvert.DeserializeAnonymousType(anwserJson, def);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var mongoId = ObjectId.GenerateNewId(DateTime.UtcNow);

                    var isAnwsered = await _context.AnswersSql.AnyAsync(x => x.User.Id == user.Id && x.Questionnare.Id == Convert.ToInt32(anwsersDes.Id));
                    if (isAnwsered)
                        return false;

                    var questionnare = _context.QuestionnaireSqls.First(x => x.Id == Convert.ToInt32(anwsersDes.Id));

                    var anwsersSql = new AnswerSql()
                    {
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        User = user,
                        AnwserMongoId = mongoId.ToString(),
                        Questionnare = questionnare
                    };
                    await _context.AnswersSql.AddAsync(anwsersSql);

                    var mongoAnwsers = new AnswerMongo()
                    {
                        Id = mongoId,
                        Anwsers = BsonSerializer.Deserialize<BsonArray>(JsonConvert.SerializeObject(anwsersDes.Items)),
                        AnwsersSqlId = anwsersSql.Id.ToString()
                    };
                    await _context.AnswersMongo.InsertOneAsync(mongoAnwsers);

                    var mongoQuestionare = (await _context.QuestionnairesMongo.FindAsync(x =>
                        x.Id == ObjectId.Parse(questionnare.QuestionnaireMongoId))).First();

                    var filter = Builders<AnwserStaticticsMongo>.Filter.Eq(x => x.QuestionnaireSqlId, questionnare.Id.ToString());

                    var questStats = (await _context.AnwserStaticticsMongo.FindAsync(x =>
                        x.Id == ObjectId.Parse(questionnare.AnwsersStatisticsMongoId))).First();

                    var questionsOfSelectableType = mongoQuestionare.Questions.Where(x => x.Type.Id > 2).Select(y=>y.Id);
                    var updateableStats =
                        questStats.Anwsers.Where(x => questionsOfSelectableType.Contains(x.QuestionId)).Select(y=>y.QuestionId);

                    var recordsToUpdate = anwsersDes.Items.Where(x => updateableStats.Contains(x.Id));

                    var updateDoc = new BsonDocument
                    {
                        {"NumberOfAnwsers", 1},
                        //     { "anwsers.0.answerIdToNumberOfAnwsers.0.v", 1 }
                    };

                    foreach (var recordToUpdate in recordsToUpdate)
                    {
                        if (string.IsNullOrEmpty(recordToUpdate.Answer))
                        {
                            int i = 0;
                            foreach (var answerOption in recordToUpdate.Answers)
                            {
                                if (!answerOption.Value)
                                    continue;

                                updateDoc.AddRange(new BsonDocument()
                                {
                                    { $"anwsers.{recordToUpdate.Id}.answerIdToNumberOfAnwsers.{i++}.v", 1 }
                                });
                            }

                            continue;
                        }
                        
                        updateDoc.AddRange(new BsonDocument()
                        {
                            { $"anwsers.{recordToUpdate.Id}.answerIdToNumberOfAnwsers.{recordToUpdate.Answer}.v", 1 }
                        });
                    }

                    var update = new BsonDocument("$inc", updateDoc);

                    await _context.AnwserStaticticsMongo.FindOneAndUpdateAsync(filter, update);

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