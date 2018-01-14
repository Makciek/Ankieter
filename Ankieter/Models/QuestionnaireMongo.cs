using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ankieter.Models
{
    public class QuestionnaireMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public AnwserStaticticsMongo AnwsersStaticticsMongo { get; set; }
        public virtual MongoDB.Bson.BsonArray Questions { get; set; }
        public string QuestionnaireSqlId { get; set; }
    }
}