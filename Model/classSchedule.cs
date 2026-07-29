using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SchoolManagement.Models
{
    public class classSchedule
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("schedule_id")]
        public int ScheduleId { get; set; }

        [BsonElement("group_name")]
        public string GroupName { get; set; } = string.Empty;

        [BsonElement("major")]
        public string Major { get; set; } = string.Empty;

        [BsonElement("teacher_id")]
        public int TeacherId { get; set; }

        [BsonElement("teacher_name")]
        public string TeacherName { get; set; } = string.Empty;

        [BsonElement("subject")]
        public string Subject { get; set; } = string.Empty;

        [BsonElement("semester")]
        public int Semester { get; set; }

        [BsonElement("academic_year")]
        public string AcademicYear { get; set; } = string.Empty;

        [BsonElement("day_of_week")]
        public string DayOfWeek { get; set; } = string.Empty;

        [BsonElement("start_time")]
        public string StartTime { get; set; } = string.Empty;

        [BsonElement("end_time")]
        public string EndTime { get; set; } = string.Empty;

        [BsonElement("room")]
        public string Room { get; set; } = string.Empty;

        [BsonElement("shift")]
        public string Shift { get; set; } = string.Empty;
    }
}