using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Group
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("group_id")]
    public int GroupId { get; set; }

    [BsonElement("group_name")]
    public string GroupName { get; set; } = string.Empty;

    [BsonElement("major")]
    public string Major { get; set; } = string.Empty;

    [BsonElement("total_students")]
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
