using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

[ApiController]
[Route("api/[controller]")]
public class MajorController : ControllerBase
{
    private readonly IMongoCollection<Major> _majors;

    public MajorController(IMongoDatabase database)
    {
        _majors = database.GetCollection<Major>("majors");
    }

    [HttpGet]
    public ActionResult<List<Major>> GetAll()
    {
        return Ok(_majors.Find(_ => true).ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Major> GetById(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var major = _majors.Find(m => m.Id == id).FirstOrDefault();
        if (major is null)
            return NotFound();

        return Ok(major);
    }

    [HttpPost]
    public ActionResult<Major> Create([FromBody] Major major)
    {
        _majors.InsertOne(major);
        return CreatedAtAction(nameof(GetById), new { id = major.Id }, major);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] Major updatedMajor)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var existing = _majors.Find(m => m.Id == id).FirstOrDefault();
        if (existing is null)
            return NotFound();

        updatedMajor.Id = id;
        _majors.ReplaceOne(m => m.Id == id, updatedMajor);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var result = _majors.DeleteOne(m => m.Id == id);
        if (result.DeletedCount == 0)
            return NotFound();

        return NoContent();
    }
}
