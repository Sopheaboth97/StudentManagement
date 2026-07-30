using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IMongoCollection<Student> _students;

    public StudentController(IMongoDatabase database)
    {
        _students = database.GetCollection<Student>("students");
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_students.Find(_ => true).SortBy(s => s.StudentId).ToList());
    }

    [HttpGet("{studentId}")]
    public IActionResult GetByStudentId(int studentId)
    {
        var student = _students
            .Find(x => x.StudentId == studentId)
            .FirstOrDefault();

        if (student == null)
            return NotFound($"Student with id '{studentId}' not found.");

        return Ok(student);
    }

    [HttpPost]
    public IActionResult Create(Student student)
    {
        int maxStudentId = _students.Find(_ => true).SortByDescending(t => t.StudentId).Limit(1).FirstOrDefault()?.StudentId ?? 0;
        student.StudentId = maxStudentId + 1;

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

    [HttpPut("{studentId}")]
    public IActionResult Update(int studentId, Student updatedStudent)
    {
        if (updatedStudent == null)
            return BadRequest("Student data is required.");

        updatedStudent.StudentId = studentId;

        var result = _students.ReplaceOne(x => x.StudentId == studentId, updatedStudent);

        if (result.MatchedCount == 0)
            return NotFound($"Student with id '{studentId}' not found.");

        return Ok(updatedStudent);
    }

    [HttpDelete("{studentId}")]
    public IActionResult Delete(int studentId)
    {
        var result = _students.DeleteOne(x => x.StudentId == studentId);

        if (result.DeletedCount == 0)
            return NotFound($"Student with id '{studentId}' not found.");

        return Ok(new { message = "Deleted successfully." });
    }
}
