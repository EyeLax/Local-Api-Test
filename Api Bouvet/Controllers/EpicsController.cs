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



        // PUT: api/Epics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Epics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Epic>> PostEpic(Epic epic)
        {
            // Check if the ProjectId on the epic exists in the database
            var project = await _context.Projects.FindAsync(epic.ProjectId);
            if (project == null)
            {
                // If not, return a NotFound result
                return NotFound($"Project with ID {epic.ProjectId} not found.");
            }

            // If the project exists, add the epic to the database
            _context.Epics.Add(epic);
            await _context.SaveChangesAsync();

            // Return a CreatedAtAction result, pointing to the GetEpic method
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
