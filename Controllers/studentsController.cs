using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IMongoCollection<Student> _students;

    public StudentsController(IMongoDatabase database)
    {
        _students = database.GetCollection<Student>("students");
    }

    // GET: api/students
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_students.Find(_ => true).ToList());
    }

    // GET: api/students/{studentId}
    [HttpGet("{studentId}")]
    public IActionResult GetByStudentId(string studentId)
    {
        var student = _students
            .Find(x => x.StudentId == studentId)
            .FirstOrDefault();

        if (student == null)
            return NotFound($"Student with id '{studentId}' not found.");

        return Ok(student);
    }

    // POST: api/students
    [HttpPost]
    public IActionResult Create(Student student)
    {
        if (student == null)
            return BadRequest("Student data is required.");

        var existing = _students.Find(x => x.StudentId == student.StudentId).FirstOrDefault();
        if (existing != null)
            return Conflict($"Student with id '{student.StudentId}' already exists.");

        _students.InsertOne(student);

        return CreatedAtAction(
            nameof(GetByStudentId),
            new { studentId = student.StudentId },
            student);
    }

    // PUT: api/students/{studentId}
    [HttpPut("{studentId}")]
    public IActionResult Update(string studentId, Student updatedStudent)
    {
        if (updatedStudent == null)
            return BadRequest("Student data is required.");

        updatedStudent.StudentId = studentId; // keep the route id authoritative

        var result = _students.ReplaceOne(x => x.StudentId == studentId, updatedStudent);

        if (result.MatchedCount == 0)
            return NotFound($"Student with id '{studentId}' not found.");

        return Ok(updatedStudent);
    }

    // DELETE: api/students/{studentId}
    [HttpDelete("{studentId}")]
    public IActionResult Delete(string studentId)
    {
        var result = _students.DeleteOne(x => x.StudentId == studentId);

        if (result.DeletedCount == 0)
            return NotFound($"Student with id '{studentId}' not found.");

        return Ok(new { message = "Deleted successfully." });
    }
}