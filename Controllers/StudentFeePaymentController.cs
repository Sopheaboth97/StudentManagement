using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

[ApiController]
[Route("api/[controller]")]
public class StudentFeePaymentController : ControllerBase
{
    private readonly IMongoCollection<StudentFeePayment> _studentFeePayments;

    public StudentFeePaymentController(IMongoDatabase database)
    {
        _studentFeePayments = database.GetCollection<StudentFeePayment>("students_fees_payments");
    }

    // GET: api/StudentFeePayment
    [HttpGet]
    public ActionResult<List<StudentFeePayment>> GetAll()
    {
        return Ok(_studentFeePayments.Find(_ => true).ToList());
    }

    // GET: api/StudentFeePayment/{id}
    [HttpGet("{id}")]
    public ActionResult<StudentFeePayment> GetById(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");
        var payment = _studentFeePayments.Find(p => p.Id == id).FirstOrDefault();
        if (payment is null) return NotFound();
        return Ok(payment);
    }

    // POST: api/StudentFeePayment
    [HttpPost]
    public ActionResult<StudentFeePayment> Create(StudentFeePayment payment)
    {
        _studentFeePayments.InsertOne(payment);
        return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
    }

    // PUT: api/StudentFeePayment/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, StudentFeePayment updatedPayment)
    {
        var result = _studentFeePayments.ReplaceOne(p => p.Id == id, updatedPayment);
        if (result.MatchedCount == 0) return NotFound();
        return NoContent();
    }

    // DELETE: api/StudentFeePayment/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var result = _studentFeePayments.DeleteOne(p => p.Id == id);
        if (result.DeletedCount == 0) return NotFound();
        return NoContent();
    }
}