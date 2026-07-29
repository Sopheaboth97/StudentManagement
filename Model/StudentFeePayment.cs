using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class StudentFeePayment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("payment_id")]
    public int PaymentId { get; set; }

    [BsonElement("student_id")]
    public int StudentId { get; set; }

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
