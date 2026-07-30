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
        return Ok(_teachers.Find(_ => true).SortBy(t => t.TeacherId).ToList());
    }

    [HttpGet("{teacherId}")]
    public ActionResult<Teacher> GetById(int teacherId)
    {
        var teacher = _teachers.Find(t => t.TeacherId == teacherId).FirstOrDefault();

        if (teacher == null)
            return NotFound($"Teacher with id '{teacherId}' not found.");

        return Ok(teacher);
    }


    [HttpPost]
    public ActionResult<Teacher> Create([FromBody] Teacher newTeacher)
    {
        int maxTeacherId = _teachers.Find(_ => true).SortByDescending(t => t.TeacherId).Limit(1).FirstOrDefault()?.TeacherId ?? 0;
        newTeacher.TeacherId = maxTeacherId + 1;

        if (newTeacher == null)
            return BadRequest("Teacher data is required.");

        _teachers.InsertOne(newTeacher);

        return CreatedAtAction(nameof(GetById), new { teacherId = newTeacher.TeacherId }, newTeacher);
    }

    [HttpPut("{teacherId}")]
    public IActionResult Update(int teacherId, [FromBody] Teacher updatedTeacher)
    {
        var existingTeacher = _teachers.Find(t => t.TeacherId == teacherId).FirstOrDefault();

        if (existingTeacher == null)
            return NotFound($"Teacher with id '{teacherId}' not found.");

        updatedTeacher.Id = existingTeacher.Id; // Preserve the original ObjectId

        var result = _teachers.ReplaceOne(t => t.TeacherId == teacherId, updatedTeacher);

        if (result.ModifiedCount == 0)
            return StatusCode(500, "Update failed.");

        return NoContent();
    }

    [HttpDelete("{teacherId}")]
    public IActionResult Delete(int teacherId)
    {
        var result = _teachers.DeleteOne(t => t.TeacherId == teacherId);

        if (result.DeletedCount == 0)
            return NotFound($"Teacher with id '{teacherId}' not found.");

        return NoContent();
    }
    // {
    //     if (!ObjectId.TryParse(id, out _))
    //         return BadRequest("Invalid id format.");

    //     var result = _teachers.DeleteOne(t => t.Id == id);

    //     if (result.DeletedCount == 0)
    //         return NotFound($"Teacher with id '{id}' not found.");

    //     return NoContent();
    // }
}
