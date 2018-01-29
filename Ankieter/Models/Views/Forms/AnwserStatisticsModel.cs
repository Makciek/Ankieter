using System.Collections.Generic;

namespace Ankieter.Models.Views.Forms
{
    public class AnwserStatisticsModel
    {
        public int QuestionId { get; set; }
        public List<KeyValuePair<int, int>> AnswerIdToNumberOfAnwsers { get; set; }
        public List<string> AnwserStringAnwser { get; set; }
    }
}