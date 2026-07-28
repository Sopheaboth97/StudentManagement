using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

[ApiController]
[Route("api/[controller]")]
public class TeacherController : ControllerBase
{
    private readonly IMongoCollection<Teacher> _teachers;

    public TeacherController(IMongoDatabase database)
    {
        _teachers = database.GetCollection<Teacher>("teachers");
    }

    [HttpGet]
    public ActionResult<List<Teacher>> GetAll()
    {
        return Ok(_teachers.Find(_ => true).ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Teacher> GetById(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var teacher = _teachers.Find(t => t.Id == id).FirstOrDefault();

        if (teacher == null)
            return NotFound($"Teacher with id '{id}' not found.");

        return Ok(teacher);
    }

    [HttpPost]
    public ActionResult<Teacher> Create([FromBody] Teacher newTeacher)
    {
        if (newTeacher == null)
            return BadRequest("Teacher data is required.");

        _teachers.InsertOne(newTeacher);

        return CreatedAtAction(nameof(GetById), new { id = newTeacher.Id }, newTeacher);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] Teacher updatedTeacher)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var existingTeacher = _teachers.Find(t => t.Id == id).FirstOrDefault();

        if (existingTeacher == null)
            return NotFound($"Teacher with id '{id}' not found.");

        updatedTeacher.Id = id;

        var result = _teachers.ReplaceOne(t => t.Id == id, updatedTeacher);

        if (result.ModifiedCount == 0)
            return StatusCode(500, "Update failed.");

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var result = _teachers.DeleteOne(t => t.Id == id);

        if (result.DeletedCount == 0)
            return NotFound($"Teacher with id '{id}' not found.");

        return NoContent();
    }
}
