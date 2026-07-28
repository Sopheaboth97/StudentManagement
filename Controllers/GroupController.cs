using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IMongoCollection<Group> _Groups;

    public GroupsController(IMongoDatabase database)
    {
        _Groups = database.GetCollection<Group>("groups");
    }

    // GET: api/groups
    // get all
    [HttpGet]
    public ActionResult<List<Group>> GetAll()
    {
        return Ok(_Groups.Find(_ => true).ToList());
    }

    // GET: api/groups/{id}
    // get one by id
    [HttpGet("{id}")]
    public ActionResult<Group> GetById(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var group = _Groups.Find(g => g.Id == id).FirstOrDefault();

        if (group == null)
            return NotFound($"Group with id '{id}' not found.");

        return Ok(group);
    }

    // POST: api/groups
    // create
    [HttpPost]
    public ActionResult<Group> Create([FromBody] Group newGroup)
    {
        if (newGroup == null)
            return BadRequest("Group data is required.");

        _Groups.InsertOne(newGroup);

        return CreatedAtAction(nameof(GetById), new { id = newGroup.Id }, newGroup);
    }

    // PUT: api/groups/{id}
    // update
    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] Group updatedGroup)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var existingGroup = _Groups.Find(g => g.Id == id).FirstOrDefault();

        if (existingGroup == null)
            return NotFound($"Group with id '{id}' not found.");

        updatedGroup.Id = id; // ensure the id stays the same

        var result = _Groups.ReplaceOne(g => g.Id == id, updatedGroup);

        if (result.ModifiedCount == 0)
            return StatusCode(500, "Update failed.");

        return NoContent();
    }

    // DELETE: api/groups/{id}
    // delete
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Invalid id format.");

        var result = _Groups.DeleteOne(g => g.Id == id);

        if (result.DeletedCount == 0)
            return NotFound($"Group with id '{id}' not found.");

        return NoContent();
    }
}