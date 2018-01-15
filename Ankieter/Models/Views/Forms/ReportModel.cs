using System.Collections.Generic;

namespace Ankieter.Models.Views.Forms
{
    public class ReportModel
    {
        public int QuestionareId { get; set; }
        public int NumberOfUsersAnwsered { get; set; }
        public List<Question> Questions { get; set; }

        public class Question
        {
            public string Name { get; set; }
            public List<AnwserOption> AnwserOptions { get; set; }

            public class AnwserOption
            {
                public string Name { get; set; }
                public int AnwserCount { get; set; }
            }
        }
    }
}