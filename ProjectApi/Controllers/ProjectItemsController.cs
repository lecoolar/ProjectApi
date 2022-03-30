using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Context;
using ProjectApi.Enums;
using ProjectApi.Filters;
using ProjectApi.Models;
using ProjectApi.Repository;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectItemsController : ControllerBase
    {
        private readonly ProjectItemRepository _projectItemRepository;

        public ProjectItemsController(ProjectItemRepository projectItemRepository)
        {
            _projectItemRepository = projectItemRepository;
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
            try
            {
                var result = await _projectItemRepository.GetProjectItemByFilters(sortBy, filtersSartdate, filterEndDate,
                    filterStatusproject, filterStartPriority, filterEndPriority);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/ProjectItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectItem>> GetProjectItem(long id)
        {
            try
            {
                var result = await _projectItemRepository.GetById(id);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/ProjectItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectItem(long id, ProjectItem projectItem)
        {
            try
            {
                await _projectItemRepository.ReplaceProjectItem(id, projectItem);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/ProjectItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectItem>> PostProjectItem(ProjectItem projectItem)
        {
            try
            {
                await _projectItemRepository.AddProjectItem(projectItem);
                return CreatedAtAction("GetProjectItem", new { id = projectItem.Id }, projectItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/ProjectItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectItem(long id)
        {
            try
            {
                await _projectItemRepository.DeleteProjectItem(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
