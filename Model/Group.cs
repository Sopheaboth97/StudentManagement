using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

[BsonIgnoreExtraElements]
public class Group
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("Group_name")]
    public string GroupName { get; set; } = string.Empty;

    [BsonElement("Major")]
    public string Major { get; set; } = string.Empty;

    [BsonElement("Total_students")]
    public int TotalStudents { get; set; }

    [BsonElement("current_semester")]
    public int currentSemester { get; set; }

    [BsonElement("academic_year")]
    public string academicYear { get; set; } = string.Empty;

    [BsonElement("shift")]
    public string shift { get; set; } = string.Empty;

    [BsonElement("status")]
    public string status { get; set; } = string.Empty;
}

