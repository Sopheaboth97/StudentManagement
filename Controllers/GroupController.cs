using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    private readonly IMongoCollection<Group> _groups;

    public GroupController(IMongoDatabase database)
    {
        _groups = database.GetCollection<Group>("groups");
    }

    [HttpGet]
    public ActionResult<List<Group>> GetAll()
    {
        return Ok(_groups.Find(_ => true).ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Group> GetById(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var group = _groups.Find(g => g.Id == id).FirstOrDefault();

        if (group == null)
            return NotFound($"Group with id '{id}' not found.");

        return Ok(group);
    }

    [HttpPost]
    public ActionResult<Group> Create([FromBody] Group newGroup)
    {
        int maxGroupId = _groups.Find(_ => true).SortByDescending(g => g.GroupId).Limit(1).FirstOrDefault()?.GroupId ?? 0;
        newGroup.GroupId = maxGroupId + 1;

        if (newGroup == null)
            return BadRequest("Group data is required.");

        _groups.InsertOne(newGroup);

        return CreatedAtAction(nameof(GetById), new { id = newGroup.Id }, newGroup);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] Group updatedGroup)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var existingGroup = _groups.Find(g => g.Id == id).FirstOrDefault();

        if (existingGroup == null)
            return NotFound($"Group with id '{id}' not found.");

        updatedGroup.Id = id;

        var result = _groups.ReplaceOne(g => g.Id == id, updatedGroup);

        if (result.ModifiedCount == 0)
            return StatusCode(500, "Update failed.");

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var result = _groups.DeleteOne(g => g.Id == id);

        if (result.DeletedCount == 0)
            return NotFound($"Group with id '{id}' not found.");

        return NoContent();
    }
}
