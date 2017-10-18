using System.Collections.Generic;

namespace Ankieter.Models
{
    public class Questionnaire : BaseEntiity
    {
        public string Name { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}