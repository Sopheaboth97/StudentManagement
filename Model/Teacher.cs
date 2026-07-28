using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Teacher
{
    [BsonId]
    public int Id { get; set; }

    [BsonElement("teacher_id")]
    public string TeacherId { get; set; } = string.Empty;

    [BsonElement("teacher_name")]
    public string TeacherName { get; set; } = string.Empty;

    [BsonElement("gender")]
    public string Gender { get; set; } = string.Empty;

    [BsonElement("Date_Of_Birth")]
    public DateTime DateOfBirth { get; set; }

    [BsonElement("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [BsonElement("subjects")]
    public List<string> Subjects { get; set; } = [];

    [BsonElement("salary")]
    public int Salary { get; set; }
}
