using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly IMongoCollection<Teachers> _Teachers;

    public TeachersController(IMongoDatabase database)
    {
        _Teachers = database.GetCollection<Teachers>("teachers");
    }

    // GET: api/teachers
    // get all
    [HttpGet]
    public ActionResult<List<Teachers>> GetAll()
    {
        return Ok(_Teachers.Find(_ => true).ToList());
    }

    // GET: api/teachers/{id}
    // get one by id
    [HttpGet("{id}")]
    public ActionResult<Teachers> GetById(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var teacher = _Teachers.Find(t => t.Id == id).FirstOrDefault();

        if (teacher == null)
            return NotFound($"Teacher with id '{id}' not found.");

        return Ok(teacher);
    }

    // POST: api/teachers
    // create
    [HttpPost]
    public ActionResult<Teachers> Create([FromBody] Teachers newTeacher)
    {
        if (newTeacher == null)
            return BadRequest("Teacher data is required.");

        _Teachers.InsertOne(newTeacher);

        return CreatedAtAction(nameof(GetById), new { id = newTeacher.Id }, newTeacher);
    }

    // PUT: api/teachers/{id}
    // update
    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] Teachers updatedTeacher)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var existingTeacher = _Teachers.Find(t => t.Id == id).FirstOrDefault();

        if (existingTeacher == null)
            return NotFound($"Teacher with id '{id}' not found.");

        updatedTeacher.Id = id; 

        var result = _Teachers.ReplaceOne(t => t.Id == id, updatedTeacher);

        if (result.ModifiedCount == 0)
            return StatusCode(500, "Update failed.");

        return NoContent();
    }

    // DELETE: api/teachers/{id}
    // delete
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var result = _Teachers.DeleteOne(t => t.Id == id);

        if (result.DeletedCount == 0)
            return NotFound($"Teacher with id '{id}' not found.");

        return NoContent();
    }
}