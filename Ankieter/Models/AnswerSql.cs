namespace Ankieter.Models
{
    public class AnswerSql : BaseEntiity
    {
        public ApplicationUser User { get; set; }
        public QuestionnaireSql Questionnare { get; set; }
        public string AnwserMongoId { get; set; }
    }
}