using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

[BsonIgnoreExtraElements]
public class Teachers
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("teacher_id")]
    public string TeacherId { get; set; } = string.Empty;

    [BsonElement("teacher_name")]
    public string TeacherName { get; set; } = string.Empty;

    [BsonElement("gender")]
    public string Gender { get; set; } = string.Empty;

    [BsonElement("Date_Of_Birth")]
    public string DateOfBirth { get; set; } = string.Empty;

    [BsonElement("phone_number")]
    public string phone_number { get; set; } = string.Empty;

    [BsonElement("subjects")]
    public List<string> Subjects { get; set; } = [];

    [BsonElement("salary")]
    public int salary {get;set;}
}