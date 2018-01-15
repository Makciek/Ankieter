using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ankieter.Models
{
    public class QuestionnaireMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId AnwsersStaticticsMongoId { get; set; }
        public List<Question> Questions { get; set; }
        public string QuestionnaireSqlId { get; set; }

        public class Question
        {
            public int Id { get; set; }
            public TypeModel Type { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool IsRequired { get; set; }
            public int TextMinLength { get; set; }
            public int TextMaxLength { get; set; }
            public List<ClicableOptionsModel> ClicableOptions { get; set; }

            public class TypeModel
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }

            public class ClicableOptionsModel
            {
                public int Id { get; set; }
                public string Content { get; set; }
            }
        }
    }
}