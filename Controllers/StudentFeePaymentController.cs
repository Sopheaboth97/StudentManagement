using Microsoft.AspNetCore.Mvc;
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
    public ActionResult<StudentFeePayment> GetById(int id)
    {
        var payment = _studentFeePayments.Find(p => p.Id == id).FirstOrDefault();
        if (payment is null) return NotFound();
        return Ok(payment);
    }

    [HttpPost]
    public ActionResult<StudentFeePayment> Create(StudentFeePayment payment)
    {
        _studentFeePayments.InsertOne(payment);
        return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, StudentFeePayment updatedPayment)
    {
        updatedPayment.Id = id;
        var result = _studentFeePayments.ReplaceOne(p => p.Id == id, updatedPayment);
        if (result.MatchedCount == 0) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _studentFeePayments.DeleteOne(p => p.Id == id);
        if (result.DeletedCount == 0) return NotFound();
        return NoContent();
    }
}
