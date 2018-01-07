using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ankieter.Models
{
    public class QuestionnaireSql : BaseEntiity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public virtual MongoDB.Bson.BsonArray Questions { get; set; }
        public string QuestionnaireMongoId { get; set; }
    }
}