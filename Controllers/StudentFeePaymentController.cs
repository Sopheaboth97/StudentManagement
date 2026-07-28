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
        return Ok(_studentFeePayments.Find(_ => true).ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<StudentFeePayment> GetById(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var payment = _studentFeePayments.Find(p => p.Id == id).FirstOrDefault();
        if (payment is null) return NotFound();
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
    public IActionResult Update(string id, StudentFeePayment updatedPayment)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        updatedPayment.Id = id;
        var result = _studentFeePayments.ReplaceOne(p => p.Id == id, updatedPayment);
        if (result.MatchedCount == 0) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var result = _studentFeePayments.DeleteOne(p => p.Id == id);
        if (result.DeletedCount == 0) return NotFound();
        return NoContent();
    }
}
