using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_Bouvet.Data;
using Api_Bouvet.Models;

namespace Api_Bouvet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks([FromQuery] bool includeEpic = false)
        {
            IQueryable<Models.Task> query = _context.Tasks;

            if (includeEpic)
            {
                query = query.Include(t => t.Epic);
            }

            return await query.ToListAsync();
        }


        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTask(int id, [FromQuery] bool includeEpic = false)
        {
            IQueryable<Models.Task> query = _context.Tasks.Where(t => t.Id == id);

            if (includeEpic)
            {
                query = query.Include(t => t.Epic);
            }

            var task = await query.FirstOrDefaultAsync();

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        /// <summary>
        /// Updates an existing task
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/tasks/1
        ///     {
        ///         "Id": 1,
        ///         "Name": "Updated Task Name",
        ///         "Description": "Updated description of the Task",
        ///         "Responsible": "Jane Doe",
        ///         "EpicId": 1
        ///     }
        /// </remarks>
        /// <param name="id">The ID of the task to update</param>
        /// <param name="task">The task data to update</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Models.Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/tasks
        ///     {
        ///         "Name": "Task 5",
        ///         "Description": "Description of Epic 5 in Epic 1",
        ///         "Responsible": "John Doe",
        ///         "EpicId": 1
        ///      }
        /// </remarks>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Models.Task>> PostTask(Models.Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
