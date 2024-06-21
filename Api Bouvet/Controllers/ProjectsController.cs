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
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all Projects with optional Epics
        /// </summary>
        /// <param name="includeEpics"></param>
        /// <returns>The list of Projects</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects([FromQuery] bool includeEpics = false)
        {
            IQueryable<Project> query = _context.Projects;

            if (includeEpics)
            {
                query = query.Include(p => p.Epics);
            }

            var projects = await query.ToListAsync();
            return projects;
        }

        /// <summary>
        /// Get project with specified id, with optional Epics
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            // Include Epics in the query
            var project = await _context.Projects.Include(p => p.Epics)
                                                 .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }


        /// <summary>
        /// Update a project with specified id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/projects/1
        ///     {
        ///         "Id": 1,
        ///         "Name": "Updated Project Name",
        ///         "Description": "Updated description of Project",
        ///         "ProjectManager": "Jane Doe"
        ///     }
        /// </remarks>
        /// <param name="id">The ID of the project to update</param>
        /// <param name="project">The project data to update</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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
        /// Create a new Project
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/projects
        ///     {
        ///         "Name": "Project 1",
        ///         "Description": "Description of Project 1",
        ///         "ProjectManager": "John Doe"
        ///      }
        /// </remarks>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            foreach (var epic in project.Epics)
            {
                epic.ProjectId = project.Id; 
                _context.Epics.Add(epic);
                await _context.SaveChangesAsync();

                foreach (var task in epic.Tasks)
                {
                    task.EpicId = epic.Id; 
                    _context.Tasks.Add(task);
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.Id }, project);
        }

        /// <summary>
        /// Delete a project with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
