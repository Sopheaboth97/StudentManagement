using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

[ApiController]
[Route("api/[controller]")]
public class MajorController : ControllerBase
{
    private readonly IMongoCollection<Major> _Majors;

    public MajorController(IMongoDatabase database)
    {
        _Majors = database.GetCollection<Major>("majors");
    }

    // GET: api/Majors
    [HttpGet]
    public ActionResult<List<Major>> GetAll()
    {
        return Ok(_Majors.Find(_ => true).ToList());
    }

    // GET: api/Majors/{id}
    [HttpGet("{id}")]
    public ActionResult<Major> GetById(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var Major = _Majors.Find(m => m.Id == id).FirstOrDefault();
        if (Major is null)
            return NotFound();

        return Ok(Major);
    }

    // POST: api/Majors
    [HttpPost]
    public ActionResult<Major> Create([FromBody] Major Major)
    {
        _Majors.InsertOne(Major);
        return CreatedAtAction(nameof(GetById), new { id = Major.Id }, Major);
    }

    // PUT: api/Majors/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] Major updatedMajor)
    {
        var existing = _Majors.Find(m => m.Id == id).FirstOrDefault();
        if (existing is null)
            return NotFound();

        updatedMajor.Id = id;
        _Majors.ReplaceOne(m => m.Id == id, updatedMajor);
        return NoContent();
    }

    // DELETE: api/Majors/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var result = _Majors.DeleteOne(m => m.Id == id);
        if (result.DeletedCount == 0)
            return NotFound();

        return NoContent();
    }
}