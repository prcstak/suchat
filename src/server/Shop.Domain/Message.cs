using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shop.Domain;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public string GroupId { get; set; }
    
    public string Body { get; set; }
    
    public DateTime Created { get; set; }
    
    public User User { get; set; }
    public string UserId { get; set; }
}