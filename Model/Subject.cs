using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Subject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("subject_id")]
    public int SubjectId { get; set; }

    [BsonElement("subject_name")]
    public string SubjectName { get; set; } = string.Empty;
}
