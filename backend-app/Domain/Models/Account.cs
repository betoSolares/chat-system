using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend_app.Domain.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string GivenName { get; set; }

        [BsonElement("lastname")]
        public string FamilyName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("online")]
        public bool IsOnline { get; set; }
    }
}