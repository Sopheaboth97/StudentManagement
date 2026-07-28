using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class StudentFeePayment
{
    [BsonId]
    public int Id { get; set; }

    [BsonElement("payment_id")]
    public string PaymentId { get; set; } = string.Empty;

    [BsonElement("student_id")]
    public string StudentId { get; set; } = string.Empty;

    [BsonElement("semester")]
    public int Semester { get; set; }

    [BsonElement("amount_paid")]
    public int AmountPaid { get; set; }

    [BsonElement("payment_date")]
    public DateTime PaymentDate { get; set; }

    [BsonElement("payment_method")]
    public string PaymentMethod { get; set; } = string.Empty;

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;
}
