using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

[BsonIgnoreExtraElements]
public class Temp
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("temp")]
    public string kabjmrlv_hz { get; set; } = string.Empty;

    
}