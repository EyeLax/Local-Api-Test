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
    public class EpicsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EpicsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Epics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Epic>>> GetEpics([FromQuery] bool includeProject = false, [FromQuery] bool includeTasks = false)
        {
            IQueryable<Epic> query = _context.Epics;

            if (includeProject)
            {
                query = query.Include(e => e.Project);
            }

            if (includeTasks)
            {
                query = query.Include(e => e.Tasks);
            }

            return await query.ToListAsync();
        }




        // GET: api/Epics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Epic>> GetEpic(int id, [FromQuery] bool includeProject = false, [FromQuery] bool includeTasks = false)
        {
            IQueryable<Epic> query = _context.Epics.Where(e => e.Id == id);

            if (includeProject)
            {
                query = query.Include(e => e.Project);
            }

            if (includeTasks)
            {
                query = query.Include(e => e.Tasks);
            }

            var epic = await query.FirstOrDefaultAsync();

            if (epic == null)
            {
                return NotFound();
            }

            return epic;
        }



        /// <summary>
        /// Updates an existing epic
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/epics/1
        ///     {
        ///         "Id": 1,
        ///         "Name": "Updated Epic Name",
        ///         "Description": "Updated description of the Epic",
        ///         "ProjectId": 1
        ///     }
        /// </remarks>
        /// <param name="id">The ID of the epic to update</param>
        /// <param name="epic">The epic data to update</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEpic(int id, Epic epic)
        {
            if (id != epic.Id)
            {
                return BadRequest();
            }

            _context.Entry(epic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EpicExists(id))
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
        /// Post a new epic
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/epics
        ///     {
        ///         "Name": "Epic 1",
        ///         "Description": "Description of Epic 4 in Project Alpha",
        ///         "ProjectId": 1
        ///      }
        /// </remarks>
        /// <param name="epic"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Epic>> PostEpic(Epic epic)
        {
            var project = await _context.Projects.FindAsync(epic.ProjectId);
            if (project == null)
            {
                return NotFound($"Project with ID {epic.ProjectId} not found.");
            }

            _context.Epics.Add(epic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEpic", new { id = epic.Id }, epic);
        }

        // DELETE: api/Epics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEpic(int id)
        {
            var epic = await _context.Epics.FindAsync(id);
            if (epic == null)
            {
                return NotFound();
            }

            _context.Epics.Remove(epic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EpicExists(int id)
        {
            return _context.Epics.Any(e => e.Id == id);
        }
    }
}
