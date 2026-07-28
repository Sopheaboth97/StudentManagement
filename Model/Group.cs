using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Group
{
    [BsonId]
    public int Id { get; set; }

    [BsonElement("Group_name")]
    public string GroupName { get; set; } = string.Empty;

    [BsonElement("Major")]
    public string Major { get; set; } = string.Empty;

    [BsonElement("Total_students")]
    public int TotalStudents { get; set; }

    [BsonElement("current_semester")]
    public int CurrentSemester { get; set; }

    [BsonElement("academic_year")]
    public string AcademicYear { get; set; } = string.Empty;

    [BsonElement("shift")]
    public string Shift { get; set; } = string.Empty;

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;
}
