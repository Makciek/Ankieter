using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ankieter.Models
{
    public class QuestionnaireSql : BaseEntiity
    {
        public string Name { get; set; }
        public string QuestionnaireMongoId { get; set; }
    }
}