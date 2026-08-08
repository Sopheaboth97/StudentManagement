using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

[ApiController]
[Route("api/[controller]")]
public class SubjectController : ControllerBase
{
    private readonly IMongoCollection<Subject> _subjects;

    public SubjectController(IMongoDatabase database)
    {
        _subjects = database.GetCollection<Subject>("subjects");
    }

    [HttpGet]
    public ActionResult<List<Subject>> GetAll()
    {
        return Ok(_subjects.Find(_ => true).SortBy(s => s.SubjectId).ToList());
    }

    [HttpGet("getsubjectnames")]
    public ActionResult<List<string>> GetSubjectNames()
    {
        var subjectNames = _subjects.Find(_ => true).Project(s => s.SubjectName).ToList().OrderBy(name => name).ToList();
        return Ok(subjectNames);
    }

    [HttpGet("{id}")]
    public ActionResult<Subject> GetById(int id)
    {
        var subject = _subjects.Find(s => s.SubjectId == id).FirstOrDefault();

        if (subject == null)
            return NotFound($"Subject with id '{id}' not found.");

        return Ok(subject);
    }

    [HttpPost]
    public ActionResult<Subject> Create([FromBody] Subject newSubject)
    {
        int maxSubjectId = _subjects.Find(_ => true).SortByDescending(s => s.SubjectId).Limit(1).FirstOrDefault()?.SubjectId ?? 0;
        newSubject.SubjectId = maxSubjectId + 1;

        if (newSubject == null)
            return BadRequest("Subject data is required.");

        _subjects.InsertOne(newSubject);

        return CreatedAtAction(nameof(GetById), new { id = newSubject.SubjectId }, newSubject);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Subject updatedSubject)
    {
        var existingSubject = _subjects.Find(s => s.SubjectId == id).FirstOrDefault();

        if (existingSubject == null)
            return NotFound($"Subject with id '{id}' not found.");

        updatedSubject.Id = existingSubject.Id;
        updatedSubject.SubjectId = id;

        var result = _subjects.ReplaceOne(s => s.SubjectId == id, updatedSubject);

        if (result.MatchedCount == 0)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _subjects.DeleteOne(s => s.SubjectId == id);

        if (result.DeletedCount == 0)
            return NotFound($"Subject with id '{id}' not found.");

        return NoContent();
    }
}
