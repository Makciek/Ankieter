using System;
using System.Threading.Tasks;
using Ankieter.IRepo;
using Ankieter.Models;
using Ankieter.Models.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Ankieter.Controllers
{
    public class FormCreator : Controller
    {
        private readonly IQuestionnaireMongoRepo _questionnaireMongoRepo;
        private readonly IQuestionnaireSqlRepo _questionnaireSqlRepo;

        public FormCreator(IQuestionnaireMongoRepo questionnaireMongoRepo, IQuestionnaireSqlRepo questionnaireSqlRepo)
        {
            _questionnaireMongoRepo = questionnaireMongoRepo;
            _questionnaireSqlRepo = questionnaireSqlRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task Index(CreatedForm form)
        {
            var signature = Guid.NewGuid() + "/" + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;

            var questionnaireMongo = new QuestionnaireMongo()
            {
                FormStructure = form.FormStructure,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                QuestionnaireSqlId = signature
            };

            var questionnaireSql = new QuestionnaireSql()
            {
                Name = form.Name,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                QuestionnaireMongoId = signature
            };

            await _questionnaireMongoRepo.CreateAsync(questionnaireMongo);

            await _questionnaireSqlRepo.CreateAsync(questionnaireSql);
        }
    }
}