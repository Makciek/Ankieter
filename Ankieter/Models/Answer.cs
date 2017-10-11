namespace Ankieter.Models
{
    public class Answer : BaseEntiity
    {
        public string Value { get; set; }

        public virtual Question Question { get; set; }
        public int QuestionId { get; set; }
    }
}