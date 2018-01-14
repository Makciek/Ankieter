using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ankieter.Models
{
    public class AnswerMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public virtual MongoDB.Bson.BsonArray Anwsers { get; set; }
        public string AnwsersSqlId { get; set; }
    }
}