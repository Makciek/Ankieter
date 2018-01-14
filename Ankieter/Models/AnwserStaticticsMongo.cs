using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ankieter.Models
{
    public class AnwserStaticticsMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string QuestionnaireSqlId { get; set; }
        public QuestionnaireMongo QuestionnaireMongo { get; set; }
        public virtual MongoDB.Bson.BsonArray Anwsers { get; set; }
    }
}