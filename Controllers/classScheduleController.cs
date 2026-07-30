using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassScheduleController : ControllerBase
    {
        private readonly IMongoCollection<ClassSchedule> _classSchedules;

        public ClassScheduleController(IMongoDatabase database)
        {
            _classSchedules = database.GetCollection<ClassSchedule>("class_schedule");
        }

        // GET: api/classschedule
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassSchedule>>> GetAll()
        {
            var schedules = await _classSchedules.Find(_ => true).SortBy(s => s.ScheduleId).ToListAsync();
            return Ok(schedules);
        }

        // GET: api/classschedule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassSchedule>> GetById(int id)
        {
            var schedule = await _classSchedules
                .Find(s => s.ScheduleId == id)
                .FirstOrDefaultAsync();

            if (schedule == null)
                return NotFound(new { message = $"Class schedule with id {id} not found" });

            return Ok(schedule);
        }

        // POST: api/classschedule
        [HttpPost]
        public async Task<ActionResult<ClassSchedule>> Create([FromBody] ClassSchedule newSchedule)
        {
            if (newSchedule == null)
                return BadRequest("Class schedule data is required.");

            var lastSchedule = await _classSchedules
                .Find(_ => true)
                .SortByDescending(s => s.ScheduleId)
                .Limit(1)
                .FirstOrDefaultAsync();

            newSchedule.ScheduleId = (lastSchedule?.ScheduleId ?? 0) + 1;

            await _classSchedules.InsertOneAsync(newSchedule);

            return CreatedAtAction(nameof(GetById), new { id = newSchedule.ScheduleId }, newSchedule);
        }

        // PUT: api/classschedule/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClassSchedule updatedSchedule)
        {
            if (updatedSchedule == null)
                return BadRequest("Class schedule data is required.");

            var existing = await _classSchedules
                .Find(s => s.ScheduleId == id)
                .FirstOrDefaultAsync();

            if (existing == null)
                return NotFound(new { message = $"Class schedule with id {id} not found" });

            updatedSchedule.Id = existing.Id;
            updatedSchedule.ScheduleId = id;

            var result = await _classSchedules.ReplaceOneAsync(
                s => s.ScheduleId == id,
                updatedSchedule
            );

            if (result.MatchedCount == 0)
                return NotFound(new { message = $"Class schedule with id {id} not found" });

            return Ok(new { message = "Class schedule updated successfully", data = updatedSchedule });
        }

        // DELETE: api/classschedule/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _classSchedules.DeleteOneAsync(s => s.ScheduleId == id);

            if (result.DeletedCount == 0)
                return NotFound(new { message = $"Class schedule with id {id} not found" });

            return Ok(new { message = "Class schedule deleted successfully" });
        }
    }
}