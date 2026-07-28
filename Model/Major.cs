using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Major
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("major_name")]
    public string MajorName { get; set; } = string.Empty;

    [BsonElement("price_per_semester")]
    public int PricePerSemester { get; set; }

    [BsonElement("subjects")]
    public List<string> Subjects { get; set; } = [];

    [BsonElement("Group")]
    public List<string> Group { get; set; } = [];
}
