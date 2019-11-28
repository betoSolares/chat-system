using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace backend_app.Domain.Models
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("username")]
        public string Username { get; set; }
        
        [BsonElement("sent")]
        public List<string> Sent { get; set; }
        
        [BsonElement("incoming")]
        public List<string> Incoming { get; set; }
        
        [BsonElement("contacts")]
        public List<string> Contacts { get; set; }
    }
}