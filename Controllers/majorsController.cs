using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

[ApiController]
[Route("api/[controller]")]
public class MajorsController : ControllerBase
{
    private readonly IMongoCollection<Majors> _Majors;

    public MajorsController(IMongoDatabase database)
    {
       _Majors = database.GetCollection<Majors>("majors"); 
    }

    // GET: api/Majors
    [HttpGet]
    public ActionResult<List<Majors>> GetAll()
    {
        return Ok(_Majors.Find(_ => true).ToList());
    }

    // GET: api/Majors/{id}
    [HttpGet("{id}")]
    public ActionResult<Majors> GetById(string id)
    {
        var Majors = _Majors.Find(m => m.Id == id).FirstOrDefault();
        if (Majors is null)
            return NotFound();

        return Ok(Majors);
    }

    // POST: api/Majors
    [HttpPost]
    public ActionResult<Majors> Create([FromBody] Majors Majors)
    {
        _Majors.InsertOne(Majors);
        return CreatedAtAction(nameof(GetById), new { id = Majors.Id }, Majors);
    }

    // PUT: api/Majors/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] Majors updatedMajors)
    {
        var existing = _Majors.Find(m => m.Id == id).FirstOrDefault();
        if (existing is null)
            return NotFound();

        updatedMajors.Id = id;
        _Majors.ReplaceOne(m => m.Id == id, updatedMajors);
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