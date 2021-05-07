using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Context;
using ProjectApi.Enums;
using ProjectApi.Models;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectItemsController : ControllerBase
    {
        private readonly ProjectTasksContext _context;

        public ProjectItemsController(ProjectTasksContext context)
        {
            _context = context;
        }

        // GET: api/ProjectItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectItem>>> GetProjectItem(
            string sortBy = null,
            string filtersSartdate = null,
            string filterEndDate = null,
            string filterStatusproject = null,
            string filterStartPriority = null,
            string filterEndPriority = null)
        {
            var result = await _context.Projects.Include(p => p.TaskItems).ToListAsync();
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "id":
                        result = result.OrderBy(e => e.Id).ToList();
                        break;
                    case "name":
                        result = result.OrderBy(e => e.Name).ToList();
                        break;
                    case "startdate":
                        result = result.OrderBy(e => e.StartDate).ToList();
                        break;
                    case "completiondate":
                        result = result.OrderBy(e => e.CompletionDate).ToList();
                        break;
                    case "statusproject":
                        result = result.OrderBy(e => e.StatusProject).ToList();
                        break;
                    case "priority":
                        result = result.OrderBy(e => e.Priority).ToList();
                        break;
                    case "taskitems":
                        result = result.OrderBy(e => e.TaskItems).ToList();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(filtersSartdate) && !string.IsNullOrEmpty(filterEndDate))
            {
                if (DateTime.TryParse(filtersSartdate, out var startDate)
                    && DateTime.TryParse(filterEndDate, out var endDate))
                {
                    if (startDate > endDate)
                        return BadRequest("Start date cannot be more that end date");
                    result = result
                        .Where(e => e.StartDate >= startDate && e.StartDate <= endDate).ToList();
                }
                else
                {
                    return BadRequest("Incorrect dates");
                }


            }

            if (!string.IsNullOrEmpty(filterStatusproject))
            {
                if (Enum.TryParse<StatusProject>(filterStatusproject, true, out var statusProject))
                {
                    result = result
                        .Where(e => e.StatusProject == statusProject).ToList();
                }
                else
                {
                    return BadRequest("Incorrect Status");
                }
            }

            if (!string.IsNullOrEmpty(filterStartPriority) 
                && !string.IsNullOrEmpty(filterEndPriority))
            {
                if (int.TryParse(filterStartPriority, out var startPriority) 
                    && int.TryParse(filterEndPriority, out var endPriority))
                {
                    if (startPriority < endPriority)
                    {
                        return BadRequest("Start priority cannot be more that end priority");
                    }

                    result = result
                        .Where(e => e.Priority >= startPriority && e.Priority <= endPriority)
                        .ToList();
                }

            }
            return result;
        }

        // GET: api/ProjectItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectItem>> GetProjectItem(long id)
        {
            var projectItem = await _context.Projects.Include(p => p.TaskItems).FirstOrDefaultAsync(t => t.Id == id);

            if (projectItem == null)
            {
                return NotFound();
            }

            return projectItem;
        }

        // PUT: api/ProjectItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectItem(long id, ProjectItem projectItem)
        {
            if (id != projectItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectItemExists(id))
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

        // POST: api/ProjectItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectItem>> PostProjectItem(ProjectItem projectItem)
        {
            _context.Projects.Add(projectItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectItem", new { id = projectItem.Id }, projectItem);
        }

        // DELETE: api/ProjectItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectItem(long id)
        {
            var projectItem = await _context.Projects.FindAsync(id);
            if (projectItem == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(projectItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectItemExists(long id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
