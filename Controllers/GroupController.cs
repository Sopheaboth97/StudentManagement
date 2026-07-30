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
        return Ok(_groups.Find(_ => true).SortBy(g => g.GroupId).ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Group> GetById(int id)
    {


        var group = _groups.Find(g => g.GroupId == id).FirstOrDefault();

        if (group == null)
            return NotFound($"Group with id '{id}' not found.");

        return Ok(group);
    }

    // POST: api/group
    [HttpPost]
    public ActionResult<Group> Create([FromBody] Group newGroup)
    {
        int maxGroupId = _groups.Find(_ => true).SortByDescending(g => g.GroupId).Limit(1).FirstOrDefault()?.GroupId ?? 0;
        newGroup.GroupId = maxGroupId + 1;

        if (newGroup == null)
            return BadRequest("Group data is required.");

        _groups.InsertOne(newGroup);

        return CreatedAtAction(nameof(GetById), new { id = newGroup.GroupId }, newGroup);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Group updatedGroup)
    {
        if (updatedGroup == null)
            return BadRequest("Request body is required.");

        var existingGroup = await _groups.Find(g => g.GroupId == id).FirstOrDefaultAsync();

        if (existingGroup == null)
            return NotFound($"Group with id '{id}' not found.");

        // Preserve Mongo's internal _id — client doesn't send this
        updatedGroup.Id = existingGroup.Id;
        updatedGroup.GroupId = id;

        var result = await _groups.FindOneAndReplaceAsync(g => g.GroupId == id, updatedGroup);

        if (result == null)
            return NotFound($"Group with id '{id}' not found.");

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _groups.DeleteOne(g => g.GroupId == id);

        if (result.DeletedCount == 0)
            return NotFound($"Group with id '{id}' not found.");

        return NoContent();
    }
}
