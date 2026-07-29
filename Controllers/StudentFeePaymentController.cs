using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

[ApiController]
[Route("api/[controller]")]
public class StudentFeePaymentController : ControllerBase
{
    private readonly IMongoCollection<StudentFeePayment> _studentFeePayments;

    public StudentFeePaymentController(IMongoDatabase database)
    {
        _studentFeePayments = database.GetCollection<StudentFeePayment>("students_fees_payments");
    }

    [HttpGet]
    public ActionResult<List<StudentFeePayment>> GetAll()
    {
        var payments = _studentFeePayments.Find(_ => true).ToList();
        return Ok(payments);
    }

    [HttpGet("{id}")]
    public ActionResult<StudentFeePayment> GetById(int id)
    {
        var payment = _studentFeePayments.Find(p => p.PaymentId == id).FirstOrDefault();
        if (payment == null) return NotFound();
        return Ok(payment);
    }

    [HttpPost]
    public ActionResult<StudentFeePayment> Create(StudentFeePayment payment)
    {
        int maxPaymentId = _studentFeePayments.Find(_ => true).SortByDescending(p => p.PaymentId).Limit(1).FirstOrDefault()?.PaymentId ?? 0;
        payment.PaymentId = maxPaymentId + 1;

        _studentFeePayments.InsertOne(payment);
        return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, StudentFeePayment updatedPayment)
    {
        var payment = _studentFeePayments.Find(p => p.PaymentId == id).FirstOrDefault();
        if (payment == null) return NotFound();

        updatedPayment.Id = payment.Id;
        var result = _studentFeePayments.ReplaceOne(p => p.Id == payment.Id, updatedPayment);
        if (result.MatchedCount == 0) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var payment = _studentFeePayments.Find(p => p.PaymentId == id).FirstOrDefault();
        if (payment == null) return NotFound();

        var result = _studentFeePayments.DeleteOne(p => p.Id == payment.Id);
        if (result.DeletedCount == 0) return NotFound();
        return NoContent();
    }
}
