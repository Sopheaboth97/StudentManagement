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
    public ActionResult<Major> GetById(int id)
    {
        var major = _majors.Find(m => m.MajorId == id).FirstOrDefault();
        if (major is null)
            return NotFound();

        return Ok(major);
    }

    // POST: api/major
    [HttpPost]
    public ActionResult<Major> Create([FromBody] Major major)
    {
        int maxMajorId = _majors.Find(_ => true).SortByDescending(m => m.MajorId).Limit(1).FirstOrDefault()?.MajorId ?? 0;
        major.MajorId = maxMajorId + 1;


        _majors.InsertOne(major);
        return CreatedAtAction(nameof(GetById), new { id = major.MajorId }, major);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Major updatedMajor)
    {
        var existing = _majors.Find(m => m.MajorId == id).FirstOrDefault();
        if (existing is null)
            return NotFound();

        updatedMajor.Id = existing.Id;
        updatedMajor.MajorId = id;

        var result = _majors.ReplaceOne(m => m.MajorId == id, updatedMajor);

        if (result.MatchedCount == 0)
            return NotFound();
        return Ok(updatedMajor);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _majors.DeleteOne(m => m.MajorId == id);
        if (result.DeletedCount == 0)
            return NotFound();

        return Ok("Major deleted successfully.");
    }
}
