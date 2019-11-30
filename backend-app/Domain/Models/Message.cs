using MongoDB.Bson.Serialization.Attributes;
namespace backend_app.Domain.Models
{
    public class Message
    {
        [BsonElement("from")]
        public string From { get; set; }
        [BsonElement("content")]
        public string Content { get; set; }
        [BsonElement("isFile")]
        public bool IsFile { get; set; }
        [BsonElement("path")]
        public string Path { get; set; }
    }
}