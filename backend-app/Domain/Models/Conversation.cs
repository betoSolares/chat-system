using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
namespace backend_app.Domain.Models
{
    public class Conversation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("members")]
        public List<string> Members { get; set; }
        [BsonElement("messages")]
        public List<Message> Messages { get; set; }
    }
}