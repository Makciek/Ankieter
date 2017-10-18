using System.Threading.Tasks;
using Ankieter.IRepo;
using Ankieter.Models.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Ankieter.Controllers
{
    public class FormCreator : Controller
    {
        private readonly IQuestionnaireMongoRepo _questionnaireMongoRepo;

        public FormCreator(IQuestionnaireMongoRepo questionnaireMongoRepo)
        {
            _questionnaireMongoRepo = questionnaireMongoRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreatedForm form)
        {
            await _questionnaireMongoRepo.CreatedFormToQuestionnaireAndCreate(form);

            return View();
        }
    }
}