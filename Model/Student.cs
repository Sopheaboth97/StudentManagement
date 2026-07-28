using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Student
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("student_id")]
    public int StudentId { get; set; }

    [BsonElement("student_name")]
    public string StudentName { get; set; } = string.Empty;

    [BsonElement("gender")]
    public string Gender { get; set; } = string.Empty;

    [BsonElement("date_of_birth")]
    public DateTime DateOfBirth { get; set; }

    [BsonElement("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("major")]
    public string Major { get; set; } = string.Empty;

    [BsonElement("group_name")]
    public string GroupName { get; set; } = string.Empty;

    [BsonElement("attendances")]
    public List<Attendance> Attendances { get; set; } = [];

    [BsonElement("exams")]
    public List<Exam> Exams { get; set; } = [];
}

[BsonIgnoreExtraElements]
public class Attendance
{
    [BsonElement("date")]
    public DateTime Date { get; set; }

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;
}

[BsonIgnoreExtraElements]
public class Exam
{
    [BsonElement("semester")]
    public int Semester { get; set; }

    [BsonElement("midterm")]
    public Test Midterm { get; set; } = new();

    [BsonElement("final")]
    public Test Final { get; set; } = new();
}

[BsonIgnoreExtraElements]
public class Test
{
    [BsonElement("scores")]
    public List<Score> Scores { get; set; } = [];
}

[BsonIgnoreExtraElements]
public class Score
{
    [BsonElement("subject")]
    public string Subject { get; set; } = string.Empty;

    [BsonElement("score")]
    public int? ScoreValue { get; set; }
}
