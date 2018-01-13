using System.Collections.Generic;

namespace Ankieter.Models.Views.Forms
{
    public class AnwserForm
    {
        public int Id { get; set; }
        public string AnswerName { get; set; }
        public string Answer { get; set; }
        public List<AnswerOption> Answers { get; set; }

        public class AnswerOption
        {
            public int Id { get; set; }
            public bool Value { get; set; }
        }
    }
}