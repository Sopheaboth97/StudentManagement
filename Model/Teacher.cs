using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Teacher
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("teacher_id")]
    public int TeacherId { get; set; }

    [BsonElement("teacher_name")]
    public string TeacherName { get; set; } = string.Empty;

    [BsonElement("gender")]
    public string Gender { get; set; } = string.Empty;

    [BsonElement("date_of_birth")]
    public DateTime DateOfBirth { get; set; }

    [BsonElement("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [BsonElement("subjects")]
    public List<string> Subjects { get; set; } = [];

    [BsonElement("salary")]
    public int Salary { get; set; }
}
