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
                 
                var anwserStat = new AnwserStatisticsModel
                {
                    QuestionId = formItem.Id,
                    AnswerIdToNumberOfAnwsers = new List<KeyValuePair<int, int>>(),
                    AnwserStringAnwser = new List<string>()
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

                    var questionsOfSelectableType = mongoQuestionare.Questions.Where(x => x.Type.Id > 0).Select(y => y.Id);
                    var updateableStats =
                        questStats.Anwsers.Where(x => questionsOfSelectableType.Contains(x.QuestionId)).Select(y => y.QuestionId);

                    var recordsToUpdate = anwsersDes.Items.Where(x => updateableStats.Contains(x.Id));

                    var updateDoc = new BsonDocument
                    {
                        {"numberOfAnwsers", 1},
                        //     { "anwsers.0.answerIdToNumberOfAnwsers.0.v", 1 }
                    };

                    var addStringUpdate = new BsonDocument();

                    foreach (var recordToUpdate in recordsToUpdate)
                    {
                        var typeId = mongoQuestionare.Questions.FirstOrDefault(x => x.Id == recordToUpdate.Id).Type.Id;

                        if (typeId == 1 || typeId == 2)
                        {
                            addStringUpdate.Add(new BsonDocument()
                            {
                                { $"anwsers.{recordToUpdate.Id}.anwserStringAnwser", recordToUpdate.Answer }
                            });
                            continue;
                        }

                        if (string.IsNullOrEmpty(recordToUpdate.Answer))
                        {
                            int i = -1;
                            foreach (var answerOption in recordToUpdate.Answers)
                            {
                                i++;
                                if (!answerOption.Value)
                                    continue;

                                updateDoc.Add(new BsonDocument()
                                {
                                    { $"anwsers.{recordToUpdate.Id}.answerIdToNumberOfAnwsers.{i}.v", 1 }
                                });
                            }

                            continue;
                        }

                        updateDoc.Add(new BsonDocument()
                        {
                            { $"anwsers.{recordToUpdate.Id}.answerIdToNumberOfAnwsers.{recordToUpdate.Answer}.v", 1 }
                        });
                    }
                    
                    var updateInc = new BsonDocument("$inc", updateDoc);
                    var updateAddStr = new BsonDocument("$addToSet", addStringUpdate);

                    var update = new BsonDocument()
                    {
                        updateInc,
                        updateAddStr
                    };

                    await _context.AnwserStaticticsMongo.FindOneAndUpdateAsync(filter, update);
                    
                    _context.SaveChanges();
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

        public async Task<ReportModel> GetFormReport(int id)
        {
                try
                {
                    var quest = _context.QuestionnaireSqls.Find(id);

                    var statsMongoQuery = await _context.AnwserStaticticsMongo.FindAsync(x =>
                        x.Id == ObjectId.Parse(quest.AnwsersStatisticsMongoId));

                    var stats = statsMongoQuery.First();
                    var questionareStruct = (await _context.QuestionnairesMongo.FindAsync(x =>
                        x.Id == ObjectId.Parse(quest.QuestionnaireMongoId))).First();

                    var result = new ReportModel()
                    {
                        QuestionareId = id,
                        NumberOfUsersAnwsered = stats.NumberOfAnwsers,
                        Questions = new List<ReportModel.Question>()
                    };

                    foreach (var anwser in stats.Anwsers)
                    {
                        var questionStruct = questionareStruct.Questions.Find(x => x.Id == anwser.QuestionId);
                    
                        var questionReport = new ReportModel.Question()
                        {
                            Name = questionStruct.Name,
                            AnwsersString = anwser.AnwserStringAnwser,
                            AnwserOptions = new List<ReportModel.Question.AnwserOption>()
                        };

                        foreach (var anwserOption in questionStruct.ClicableOptions)
                        {
                            questionReport.AnwserOptions.Add(new ReportModel.Question.AnwserOption()
                            {
                                Name = anwserOption.Content,
                                AnwserCount = anwser.AnswerIdToNumberOfAnwsers.Find(x => x.Key == anwserOption.Id).Value
                            });
                        }

                        result.Questions.Add(questionReport);
                    }

                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
        }
    }
}