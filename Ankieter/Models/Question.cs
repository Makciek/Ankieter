using System.Collections.Generic;

namespace Ankieter.Models
{
    public class Question : BaseEntiity
    {
        public string Value { get; set; }

        public virtual ICollection<AnswerSql> Answers { get; set; }
    }
}