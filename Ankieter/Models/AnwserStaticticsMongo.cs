using System.Collections.Generic;
using Ankieter.Models.Views.Forms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ankieter.Models
{
    public class AnwserStaticticsMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string QuestionnaireSqlId { get; set; }
        public ObjectId QuestionnaireMongoId { get; set; }

        public int NumberOfAnwsers { get; set; }
        public List<AnwserStatisticsModel> Anwsers { get; set; }
    }
}