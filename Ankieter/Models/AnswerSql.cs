namespace Ankieter.Models
{
    public class AnswerSql : BaseEntiity
    {
        public ApplicationUser User { get; set; }
        public string AnwserMongoId { get; set; }
    }
}